using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public class BsmartResidentTimelineForm : Form
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private readonly DataGridView grid = new DataGridView();

        public BsmartResidentTimelineForm()
        {
            Text = "B-SMART Resident History Timeline";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(980, 620);

            Label title = new Label
            {
                Text = "Resident History Timeline",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Location = new Point(22, 18),
                Size = new Size(560, 42)
            };

            grid.Location = new Point(22, 78);
            grid.Size = new Size(920, 460);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(title);
            Controls.Add(grid);

            Load += (s, e) =>
            {
                BsmartUiService.PrepareForm(this);
                LoadTimeline();
            };
        }

        private void LoadTimeline()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                BsmartIdHelper.EnsureCodes(conn);
                using MySqlCommand cmd = new MySqlCommand(@"
                    SELECT *
                    FROM (
                        SELECT h.Date AS event_date,
                               CAST('Health Record' AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS event_type,
                               CAST(COALESCE(h.resident_code, CAST(h.ID AS CHAR)) AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS reference_no,
                               CAST(CONCAT(COALESCE(h.Diagnosis, ''), CASE WHEN COALESCE(h.Treatment, '') = '' THEN '' ELSE CONCAT(' - ', h.Treatment) END) AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS details
                        FROM health_records h
                        INNER JOIN users u ON u.id = @userId
                        WHERE COALESCE(h.is_archived, 0) = 0
                          AND (
                              (h.resident_code IS NOT NULL
                               AND h.resident_code <> ''
                               AND u.resident_code IS NOT NULL
                               AND u.resident_code <> ''
                               AND h.resident_code COLLATE utf8mb4_unicode_ci = u.resident_code COLLATE utf8mb4_unicode_ci)
                              OR (
                                  h.barangay_id = u.barangay_id
                                  AND LOWER(TRIM(COALESCE(h.first_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND LOWER(TRIM(COALESCE(h.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND LOWER(TRIM(COALESCE(h.last_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                              )
                          )
                        UNION ALL
                        SELECT a.appt_date AS event_date,
                               CAST('Appointment' AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS event_type,
                               CAST(COALESCE(NULLIF(a.appointment_code, ''), CAST(a.id AS CHAR)) AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS reference_no,
                               CAST(CONCAT(COALESCE(s.name, 'Health service'), ' - ', a.status) AS CHAR CHARACTER SET utf8mb4) COLLATE utf8mb4_unicode_ci AS details
                        FROM appointments a
                        LEFT JOIN health_services s ON s.id = a.service_id
                        WHERE a.user_id = @userId
                    ) timeline
                    ORDER BY event_date DESC", conn);
                cmd.Parameters.AddWithValue("@userId", Session.UserID);

                DataTable table = new DataTable();
                using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(table);
                grid.DataSource = table;
                BsmartUiService.StyleGrid(grid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading resident timeline: " + ex.Message, "Timeline",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
