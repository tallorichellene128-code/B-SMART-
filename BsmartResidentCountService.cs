using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartResidentCountService
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public static int CountResidents(string barangayName = "", int barangayId = 0)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string filterUsers = "";
            string filterHealth = "";
            if (barangayId > 0 || !string.IsNullOrWhiteSpace(barangayName))
            {
                filterUsers = @" AND (u.barangay_id = @barangayId
                                  OR LOWER(TRIM(COALESCE(bu.name, ''))) COLLATE utf8mb4_unicode_ci =
                                     LOWER(TRIM(@barangayName)) COLLATE utf8mb4_unicode_ci)";
                filterHealth = @" AND (h.barangay_id = @barangayId
                                   OR LOWER(TRIM(COALESCE(bh.name, ''))) COLLATE utf8mb4_unicode_ci =
                                      LOWER(TRIM(@barangayName)) COLLATE utf8mb4_unicode_ci)";
            }

            HashSet<string> residents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (MySqlCommand cmd = new MySqlCommand($@"
                SELECT u.resident_code, u.first_name, u.middle_name, u.last_name, u.birthday, u.barangay_id
                FROM users u
                LEFT JOIN barangays bu ON bu.id = u.barangay_id
                WHERE LOWER(TRIM(u.role)) COLLATE utf8mb4_unicode_ci = 'resident' COLLATE utf8mb4_unicode_ci
                  AND COALESCE(u.is_archived, 0) = 0
                  {filterUsers}", conn))
            {
                AddScopeParameters(cmd, barangayName, barangayId);
                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) residents.Add(BuildKey(reader));
            }

            using (MySqlCommand cmd = new MySqlCommand($@"
                SELECT h.resident_code, h.first_name, h.middle_name, h.last_name, h.birthday, h.barangay_id
                FROM health_records h
                LEFT JOIN barangays bh ON bh.id = h.barangay_id
                WHERE COALESCE(h.is_archived, 0) = 0
                  {filterHealth}", conn))
            {
                AddScopeParameters(cmd, barangayName, barangayId);
                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) residents.Add(BuildKey(reader));
            }

            residents.Remove("");
            return residents.Count;
        }

        private static void AddScopeParameters(MySqlCommand cmd, string barangayName, int barangayId)
        {
            cmd.Parameters.AddWithValue("@barangayId", barangayId);
            cmd.Parameters.AddWithValue("@barangayName", barangayName ?? "");
        }

        private static string BuildKey(MySqlDataReader reader)
        {
            string code = Text(reader, "resident_code");
            if (!string.IsNullOrWhiteSpace(code)) return "CODE|" + code.Trim();

            string birthday = "";
            object birthdayValue = reader["birthday"];
            if (birthdayValue != DBNull.Value && DateTime.TryParse(Convert.ToString(birthdayValue), out DateTime parsed))
                birthday = parsed.ToString("yyyy-MM-dd");

            string barangayId = Text(reader, "barangay_id");
            return string.Join("|",
                "PERSON",
                Text(reader, "first_name").Trim().ToLowerInvariant(),
                Text(reader, "middle_name").Trim().ToLowerInvariant(),
                Text(reader, "last_name").Trim().ToLowerInvariant(),
                birthday,
                barangayId);
        }

        private static string Text(MySqlDataReader reader, string field)
        {
            object value = reader[field];
            return value == DBNull.Value ? "" : Convert.ToString(value) ?? "";
        }
    }
}
