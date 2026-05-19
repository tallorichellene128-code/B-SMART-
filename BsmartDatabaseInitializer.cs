using System;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartDatabaseInitializer
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private static bool initialized;

        public static void EnsureSupportTables()
        {
            if (initialized) return;

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                Execute(conn, @"
                    CREATE TABLE IF NOT EXISTS audit_logs (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        user_id INT NULL,
                        role VARCHAR(50) NULL,
                        barangay_id INT NULL,
                        action VARCHAR(80) NOT NULL,
                        entity_type VARCHAR(80) NULL,
                        entity_id VARCHAR(80) NULL,
                        details TEXT NULL,
                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        INDEX idx_audit_created_at (created_at),
                        INDEX idx_audit_user (user_id),
                        INDEX idx_audit_barangay (barangay_id)
                    )");

                Execute(conn, @"
                    CREATE TABLE IF NOT EXISTS notification_views (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        viewer_key VARCHAR(120) NOT NULL,
                        signature_hash VARCHAR(64) NOT NULL,
                        viewed_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        UNIQUE KEY uq_notification_view (viewer_key, signature_hash)
                    )");

                Execute(conn, @"
                    CREATE TABLE IF NOT EXISTS app_notifications (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        user_id INT NULL,
                        role VARCHAR(50) NULL,
                        barangay_id INT NULL,
                        title VARCHAR(160) NOT NULL,
                        detail TEXT NOT NULL,
                        category VARCHAR(80) NULL,
                        is_read TINYINT(1) NOT NULL DEFAULT 0,
                        created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        INDEX idx_notifications_user (user_id),
                        INDEX idx_notifications_role (role),
                        INDEX idx_notifications_barangay (barangay_id),
                        INDEX idx_notifications_created_at (created_at)
                    )");

                Execute(conn, @"
                    CREATE TABLE IF NOT EXISTS app_settings (
                        name VARCHAR(120) PRIMARY KEY,
                        value_text TEXT NULL,
                        updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
                    )");

                initialized = true;
            }
            catch
            {
                // Keep startup non-blocking; features that need these tables fall back safely.
            }
        }

        private static void Execute(MySqlConnection conn, string sql)
        {
            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
