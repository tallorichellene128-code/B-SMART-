using System;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartAuditService
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public static void Log(string action, string entityType = "", object? entityId = null, string details = "")
        {
            try
            {
                if (!ShouldRecord(action)) return;

                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                using MySqlCommand cmd = new MySqlCommand(@"
                    INSERT INTO audit_logs (user_id, role, barangay_id, action, entity_type, entity_id, details)
                    VALUES (@userId, @role, @barangayId, @action, @entityType, @entityId, @details)", conn);

                cmd.Parameters.AddWithValue("@userId", Session.UserID == 0 ? (object)DBNull.Value : Session.UserID);
                cmd.Parameters.AddWithValue("@role", string.IsNullOrWhiteSpace(Session.Role) ? (object)DBNull.Value : Session.Role);
                cmd.Parameters.AddWithValue("@barangayId", Session.BarangayID == 0 ? (object)DBNull.Value : Session.BarangayID);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@entityType", string.IsNullOrWhiteSpace(entityType) ? (object)DBNull.Value : entityType);
                cmd.Parameters.AddWithValue("@entityId", entityId == null ? (object)DBNull.Value : Convert.ToString(entityId) ?? "");
                string activityText = BuildActivityText(action, entityType, details);
                cmd.Parameters.AddWithValue("@details", string.IsNullOrWhiteSpace(activityText) ? (object)DBNull.Value : activityText);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Audit logging must never block the user's workflow.
            }
        }

        private static bool ShouldRecord(string action)
        {
            string role = Session.Role ?? "";
            if (!IsOfficialRole(role)) return false;

            string normalized = (action ?? "").Trim();
            if (normalized.Equals("Login", StringComparison.OrdinalIgnoreCase)) return true;

            return normalized.Contains("Health Record", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("Resident", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("Medicine", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("Vaccine", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("Inventory", StringComparison.OrdinalIgnoreCase)
                || normalized.Contains("Appointment", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsOfficialRole(string role)
        {
            return role.Equals("Mayor", StringComparison.OrdinalIgnoreCase)
                || role.Equals("Captain", StringComparison.OrdinalIgnoreCase)
                || role.Equals("LGUStaff", StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildActivityText(string action, string entityType, string details = "")
        {
            string actor = ActorLabel();
            string normalized = (action ?? "").Trim();
            string subject = string.IsNullOrWhiteSpace(entityType) ? normalized : entityType.Trim();
            string item = (details ?? "").Trim();
            string suffix = string.IsNullOrWhiteSpace(item) ? "" : ": " + item;

            if (normalized.Equals("Login", StringComparison.OrdinalIgnoreCase))
                return actor + " logged in";

            if (normalized.Contains("Appointment", StringComparison.OrdinalIgnoreCase)
                || subject.Contains("appointment", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Approve", StringComparison.OrdinalIgnoreCase))
                    return actor + " approved an appointment" + suffix;
                if (normalized.Contains("Reject", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
                    return actor + " rejected an appointment" + suffix;
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Book", StringComparison.OrdinalIgnoreCase))
                    return actor + " booked an appointment" + suffix;
                return actor + " updated an appointment" + suffix;
            }

            if (normalized.Contains("Medicine", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Archive", StringComparison.OrdinalIgnoreCase))
                    return actor + " archived medicine" + suffix;
                if (normalized.Contains("Delete", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Remove", StringComparison.OrdinalIgnoreCase))
                    return actor + " deleted medicine" + suffix;
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return actor + " added medicine" + suffix;
                return actor + " updated medicine quantity" + suffix;
            }

            if (normalized.Contains("Vaccine", StringComparison.OrdinalIgnoreCase))
            {
                if (normalized.Contains("Archive", StringComparison.OrdinalIgnoreCase))
                    return actor + " archived vaccine" + suffix;
                if (normalized.Contains("Delete", StringComparison.OrdinalIgnoreCase)
                    || normalized.Contains("Remove", StringComparison.OrdinalIgnoreCase))
                    return actor + " deleted vaccine" + suffix;
                if (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase))
                    return actor + " added vaccine" + suffix;
                return actor + " updated vaccine quantity" + suffix;
            }

            if (normalized.Contains("Inventory", StringComparison.OrdinalIgnoreCase)
                || subject.Contains("inventory", StringComparison.OrdinalIgnoreCase))
            {
                return actor + " updated inventory" + suffix;
            }

            if (normalized.Contains("Health Record", StringComparison.OrdinalIgnoreCase)
                || subject.Contains("health", StringComparison.OrdinalIgnoreCase))
            {
                return actor + (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase)
                    ? " added a resident health record"
                    : " updated a resident health record");
            }

            if (normalized.Contains("Resident", StringComparison.OrdinalIgnoreCase)
                || subject.Contains("resident", StringComparison.OrdinalIgnoreCase))
            {
                return actor + (normalized.StartsWith("Add ", StringComparison.OrdinalIgnoreCase)
                    ? " added a resident record"
                    : " updated a resident record");
            }

            return actor + " " + normalized.ToLowerInvariant();
        }

        private static string ActorLabel()
        {
            string role = string.IsNullOrWhiteSpace(Session.Role) ? "User" : Session.Role;
            if ((role.Equals("Captain", StringComparison.OrdinalIgnoreCase)
                    || role.Equals("LGUStaff", StringComparison.OrdinalIgnoreCase))
                && !string.IsNullOrWhiteSpace(Session.BarangayName))
            {
                return role + "-" + Session.BarangayName;
            }

            return role;
        }
    }
}
