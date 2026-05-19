using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartBackupService
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private static readonly string[] DefaultTables =
        {
            "barangays",
            "users",
            "health_records",
            "appointments",
            "health_services",
            "medicines",
            "vaccines",
            "audit_logs",
            "app_notifications"
        };

        public static void ExportBackupWithDialog()
        {
            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "SQL Backup (*.sql)|*.sql",
                FileName = "BSMART-Backup-" + DateTime.Now.ToString("yyyyMMdd-HHmm", CultureInfo.InvariantCulture) + ".sql"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                ExportBackup(dialog.FileName);
                BsmartAuditService.Log("Database Backup", "database", null, Path.GetFileName(dialog.FileName));
                MessageBox.Show("Database backup created successfully.", "Backup",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating backup: " + ex.Message, "Backup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RestoreBackupWithDialog()
        {
            using OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "SQL Backup (*.sql)|*.sql",
                Title = "Select BSMART SQL backup"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            if (MessageBox.Show("Restore this SQL backup into the current database? Existing duplicate rows will be skipped when the backup uses INSERT IGNORE.",
                    "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                RestoreBackup(dialog.FileName);
                BsmartAuditService.Log("Database Restore", "database", null, Path.GetFileName(dialog.FileName));
                MessageBox.Show("Database restore completed.", "Restore",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring backup: " + ex.Message, "Restore Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ExportBackup(string filePath)
        {
            BsmartDatabaseInitializer.EnsureSupportTables();

            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("/* BSMART database backup */");
            sql.AppendLine("/* Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " */");
            sql.AppendLine("/* Scope: " + BackupScopeLabel() + " */");
            sql.AppendLine("SET FOREIGN_KEY_CHECKS = 0;");
            sql.AppendLine();

            foreach (string table in DefaultTables.Where(t => TableExists(conn, t)))
            {
                DataTable data = new DataTable();
                using (MySqlCommand select = CreateScopedSelectCommand(conn, table))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(select))
                    adapter.Fill(data);

                sql.AppendLine("/* Table: " + table + " */");
                foreach (DataRow row in data.Rows)
                {
                    string columns = string.Join(", ", data.Columns.Cast<DataColumn>().Select(c => "`" + c.ColumnName + "`"));
                    string values = string.Join(", ", data.Columns.Cast<DataColumn>().Select(c => SqlValue(row[c])));
                    sql.AppendLine($"INSERT IGNORE INTO `{table}` ({columns}) VALUES ({values});");
                }
                sql.AppendLine();
            }

            sql.AppendLine("SET FOREIGN_KEY_CHECKS = 1;");
            File.WriteAllText(filePath, sql.ToString(), new UTF8Encoding(false));
        }

        private static MySqlCommand CreateScopedSelectCommand(MySqlConnection conn, string table)
        {
            string query = $"SELECT * FROM `{table}`";
            MySqlCommand cmd;

            if (Session.IsMayor || Session.BarangayID == 0)
            {
                cmd = new MySqlCommand(query, conn);
                return cmd;
            }

            string barangayFilter = BarangayFilterFor(table);
            cmd = new MySqlCommand(query + barangayFilter, conn);

            if (!string.IsNullOrWhiteSpace(barangayFilter))
            {
                cmd.Parameters.AddWithValue("@barangayId", Session.BarangayID);
                cmd.Parameters.AddWithValue("@barangayName", Session.BarangayName ?? "");
            }

            return cmd;
        }

        private static string BarangayFilterFor(string table)
        {
            return table switch
            {
                "barangays" => " WHERE id = @barangayId OR name = @barangayName",
                "users" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "health_records" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "appointments" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "medicines" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "vaccines" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "audit_logs" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                "app_notifications" => " WHERE barangay_id = @barangayId OR barangay_id IN (SELECT id FROM barangays WHERE name = @barangayName)",
                _ => ""
            };
        }

        private static string BackupScopeLabel()
        {
            if (Session.IsMayor || Session.BarangayID == 0) return "Mayor visible records / all barangays";
            return "Barangay only - " + (Session.BarangayName ?? "Unknown");
        }

        public static void RestoreBackup(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Backup file was not found.", filePath);

            string sql = File.ReadAllText(filePath, Encoding.UTF8).TrimStart('\uFEFF');
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(sql, conn)
            {
                CommandTimeout = 0
            };
            cmd.ExecuteNonQuery();
        }

        private static bool TableExists(MySqlConnection conn, string table)
        {
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT COUNT(*)
                FROM information_schema.tables
                WHERE table_schema = DATABASE()
                  AND table_name = @table", conn);
            cmd.Parameters.AddWithValue("@table", table);
            return Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) > 0;
        }

        private static string SqlValue(object value)
        {
            if (value == null || value == DBNull.Value) return "NULL";
            if (value is DateTime dt) return "'" + dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
            if (value is bool b) return b ? "1" : "0";
            if (value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal)
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "NULL";

            return "'" + MySqlHelper.EscapeString(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "") + "'";
        }
    }
}
