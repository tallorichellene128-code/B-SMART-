using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public class BsmartAuditLogForm : Form
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private readonly DataGridView grid = new DataGridView();
        private readonly TextBox search = new TextBox();
        private readonly Button refresh = new Button();

        public BsmartAuditLogForm()
        {
            Text = "B-SMART Activity Log";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(980, 620);

            Label title = new Label
            {
                Text = "User Activity Log",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Location = new Point(22, 18),
                Size = new Size(460, 42)
            };

            search.Name = "txtSearchActivityLog";
            search.Location = new Point(22, 78);
            search.Size = new Size(650, 34);
            search.Font = new Font("Segoe UI", 11F);
            search.TextChanged += (s, e) => LoadLogs(search.Text.Trim());

            refresh.Text = "Refresh";
            refresh.Location = new Point(690, 78);
            refresh.Size = new Size(120, 34);
            refresh.Click += (s, e) => LoadLogs(search.Text.Trim());

            grid.Location = new Point(22, 130);
            grid.Size = new Size(920, 410);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Controls.Add(title);
            Controls.Add(search);
            Controls.Add(refresh);
            Controls.Add(grid);

            Load += (s, e) =>
            {
                if (!Session.IsMayor && !string.Equals(Session.Role, "Captain", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Only the mayor and barangay captain can access the activity log.",
                        "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                BsmartUiService.PrepareForm(this);
                LoadLogs();
            };
        }

        private void LoadLogs(string keyword = "")
        {
            try
            {
                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string scope = Session.IsMayor
                    ? ""
                    : " AND a.barangay_id = @barangayId AND a.role IN ('Captain', 'LGUStaff')";
                string roleScope = Session.IsMayor
                    ? " AND a.role IN ('Mayor', 'Captain', 'LGUStaff')"
                    : "";
                string allowedActivities = roleScope + @"
                    AND (
                        action = 'Login'
                        OR action LIKE 'Add %Health Record%'
                        OR action LIKE 'Update %Health Record%'
                        OR action LIKE 'Add %Resident%'
                        OR action LIKE 'Update %Resident%'
                        OR action LIKE 'Add %Medicine%'
                        OR action LIKE 'Update %Medicine%'
                        OR action LIKE 'Archive %Medicine%'
                        OR action LIKE 'Delete %Medicine%'
                        OR action LIKE '%Medicine%Auto Archived%'
                        OR action LIKE 'Add %Vaccine%'
                        OR action LIKE 'Update %Vaccine%'
                        OR action LIKE 'Archive %Vaccine%'
                        OR action LIKE 'Delete %Vaccine%'
                        OR action LIKE '%Vaccine%Auto Archived%'
                        OR action LIKE 'Add %Inventory%'
                        OR action LIKE 'Update %Inventory%'
                        OR action LIKE 'Approve %Appointment%'
                        OR action LIKE 'Reject %Appointment%'
                        OR action LIKE 'Cancel %Appointment%'
                        OR action LIKE 'Update %Appointment%'
                    )";
                string filter = string.IsNullOrWhiteSpace(keyword) ? "" : @"
                    AND (
                        COALESCE(a.role, '') LIKE @kw
                        OR COALESCE(b.name, '') LIKE @kw
                        OR CONCAT(COALESCE(a.role, ''), '-', COALESCE(b.name, '')) LIKE @kw
                        OR COALESCE(a.action, '') LIKE @kw
                        OR COALESCE(a.entity_type, '') LIKE @kw
                        OR COALESCE(a.details, '') LIKE @kw
                        OR DATE_FORMAT(a.created_at, '%d/%m/%Y') LIKE @kw
                        OR DATE_FORMAT(a.created_at, '%d/%m/%Y %h:%i %p') LIKE @kw
                    )";

                using MySqlCommand cmd = new MySqlCommand(@"
                    SELECT a.created_at AS 'Date/Time',
                           CASE
                               WHEN a.role IN ('Captain', 'LGUStaff') AND COALESCE(b.name, '') <> ''
                                   THEN CONCAT(a.role, '-', b.name)
                               ELSE COALESCE(a.role, '')
                           END AS 'Role',
                           COALESCE(NULLIF(a.details, ''), a.action) AS 'Activity',
                           COALESCE(a.entity_type, '') AS 'Record Type',
                           COALESCE(a.action, '') AS '_Action',
                           COALESCE(a.details, '') AS '_Details'
                    FROM audit_logs a
                    LEFT JOIN barangays b ON b.id = a.barangay_id
                    WHERE 1=1" + scope + allowedActivities + filter + @"
                    ORDER BY a.created_at DESC
                    LIMIT 500", conn);

                if (!Session.IsMayor) cmd.Parameters.AddWithValue("@barangayId", Session.BarangayID);
                if (!string.IsNullOrWhiteSpace(keyword)) cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                DataTable table = new DataTable();
                using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(table);
                foreach (DataRow row in table.Rows)
                {
                    row["Activity"] = FormatActivity(
                        Convert.ToString(row["Role"]) ?? "",
                        Convert.ToString(row["_Action"]) ?? "",
                        Convert.ToString(row["_Details"]) ?? "");
                }
                if (table.Columns.Contains("_Action")) table.Columns.Remove("_Action");
                if (table.Columns.Contains("_Details")) table.Columns.Remove("_Details");
                grid.DataSource = table;
                BsmartUiService.StyleGrid(grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading activity logs: " + ex.Message, "Activity Log",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string FormatActivity(string actor, string action, string details)
        {
            string normalized = (action ?? "").Trim();
            string text = (details ?? "").Trim();
            if (text.StartsWith("Mayor ", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("Captain", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("LGUStaff", StringComparison.OrdinalIgnoreCase))
            {
                return text;
            }

            string prefix = string.IsNullOrWhiteSpace(actor) ? "User" : actor;
            string suffix = string.IsNullOrWhiteSpace(text) ? "" : ": " + text;

            if (normalized.Equals("Login", StringComparison.OrdinalIgnoreCase))
                return prefix + " logged in";

            if (normalized.Contains("Appointment", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Approve", StringComparison.OrdinalIgnoreCase))
                    return prefix + " approved an appointment" + suffix;
                if (normalized.Contains("Reject", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
                    return prefix + " rejected an appointment" + suffix;
                return prefix + " updated an appointment" + suffix;
            }

            if (normalized.Contains("Medicine", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Archive", StringComparison.OrdinalIgnoreCase))
                    return prefix + " archived medicine" + suffix;
                if (normalized.Contains("Delete", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Remove", StringComparison.OrdinalIgnoreCase))
                    return prefix + " deleted medicine" + suffix;
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return prefix + " added medicine" + suffix;
                return prefix + " updated medicine quantity" + suffix;
            }

            if (normalized.Contains("Vaccine", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Archive", StringComparison.OrdinalIgnoreCase))
                    return prefix + " archived vaccine" + suffix;
                if (normalized.Contains("Delete", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Remove", StringComparison.OrdinalIgnoreCase))
                    return prefix + " deleted vaccine" + suffix;
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return prefix + " added vaccine" + suffix;
                return prefix + " updated vaccine quantity" + suffix;
            }

            if (normalized.Contains("Health Record", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return prefix + " added a resident health record" + suffix;
                return prefix + " updated a resident health record" + suffix;
            }

            if (normalized.Contains("Resident", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return prefix + " added a resident record" + suffix;
                return prefix + " updated a resident record" + suffix;
            }

            return string.IsNullOrWhiteSpace(text) ? prefix + " " + normalized.ToLowerInvariant() : text;
        }
    }
}
