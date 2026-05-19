using System;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartAppSettings
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private const int DefaultLowStockThreshold = 10;

        public static int LowStockThreshold
        {
            get => GetInt("inventory.low_stock_threshold", DefaultLowStockThreshold);
            set => Set("inventory.low_stock_threshold", Math.Max(1, value).ToString(CultureInfo.InvariantCulture));
        }

        private static int GetInt(string name, int fallback)
        {
            try
            {
                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                using MySqlCommand cmd = new MySqlCommand("SELECT value_text FROM app_settings WHERE name = @name", conn);
                cmd.Parameters.AddWithValue("@name", name);
                object result = cmd.ExecuteScalar();
                return int.TryParse(Convert.ToString(result), NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                    ? value
                    : fallback;
            }
            catch
            {
                return fallback;
            }
        }

        private static void Set(string name, string value)
        {
            try
            {
                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                using MySqlCommand cmd = new MySqlCommand(@"
                    INSERT INTO app_settings (name, value_text)
                    VALUES (@name, @value)
                    ON DUPLICATE KEY UPDATE value_text = VALUES(value_text)", conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
                BsmartAuditService.Log("Update Setting", "app_settings", name, value);
            }
            catch
            {
                // Settings should never block the main workflow.
            }
        }
    }
}
