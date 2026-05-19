using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartIdHelper
    {
        public static void EnsureCodes(MySqlConnection conn)
        {
            EnsureColumn(conn, "health_records", "resident_code", "VARCHAR(20) NULL");
            EnsureColumn(conn, "health_records", "middle_name", "VARCHAR(100) NULL");
            EnsureColumn(conn, "health_records", "address", "VARCHAR(255) NULL");
            EnsureColumn(conn, "health_records", "violation", "TEXT NULL");
            EnsureColumn(conn, "users", "resident_code", "VARCHAR(20) NULL");
            EnsureColumn(conn, "medicines", "medicine_code", "VARCHAR(20) NULL");
            EnsureColumn(conn, "vaccines", "vaccine_code", "VARCHAR(20) NULL");
            EnsureColumn(conn, "appointments", "appointment_code", "VARCHAR(20) NULL");
            BackfillResidentCodes(conn);
            BackfillUserResidentCodes(conn);
            BackfillItemCodes(conn, "medicines", "medicine_code", "MED");
            BackfillItemCodes(conn, "vaccines", "vaccine_code", "VAC");
            BackfillAppointmentCodes(conn);
            BackfillAppointmentBarangays(conn);
        }

        public static string GetOrCreateResidentCode(MySqlConnection conn, string firstName, string lastName, DateTime birthday, int barangayId)
        {
            return GetOrCreateResidentCode(conn, firstName, "", lastName, birthday, barangayId);
        }

        public static string GetOrCreateResidentCode(MySqlConnection conn, string firstName, string middleName, string lastName, DateTime birthday, int barangayId)
        {
            return GetOrCreateResidentCode(conn, firstName, middleName, lastName, birthday, barangayId, "");
        }

        public static string GetOrCreateResidentCode(MySqlConnection conn, string firstName, string middleName, string lastName, DateTime birthday, int barangayId, string address)
        {
            EnsureCodes(conn);
            string existing = FindExistingResidentCode(conn, firstName, middleName, lastName, birthday, barangayId, address);
            if (!string.IsNullOrWhiteSpace(existing)) return existing;

            return NextResidentCode(conn, barangayId);
        }

        public static string NextMedicineCode(MySqlConnection conn, int barangayId)
        {
            EnsureCodes(conn);
            return NextCode(conn, "medicines", "medicine_code", PrefixForBarangay(GetBarangayName(conn, barangayId)) + "MED", barangayId);
        }

        public static string NextVaccineCode(MySqlConnection conn, int barangayId)
        {
            EnsureCodes(conn);
            return NextCode(conn, "vaccines", "vaccine_code", PrefixForBarangay(GetBarangayName(conn, barangayId)) + "VAC", barangayId);
        }

        public static string NextAppointmentCode(MySqlConnection conn, int barangayId)
        {
            EnsureCodes(conn);
            return NextCode(conn, "appointments", "appointment_code", PrefixForBarangay(GetBarangayName(conn, barangayId)) + "APP", barangayId);
        }

        private static void EnsureColumn(MySqlConnection conn, string tableName, string columnName, string definition)
        {
            using MySqlCommand check = new MySqlCommand(@"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = @table
                  AND COLUMN_NAME = @column", conn);
            check.Parameters.AddWithValue("@table", tableName);
            check.Parameters.AddWithValue("@column", columnName);
            if (Convert.ToInt32(check.ExecuteScalar(), CultureInfo.InvariantCulture) > 0) return;

            using MySqlCommand alter = new MySqlCommand($"ALTER TABLE {tableName} ADD COLUMN {columnName} {definition}", conn);
            alter.ExecuteNonQuery();
        }

        private static void BackfillResidentCodes(MySqlConnection conn)
        {
            using MySqlCommand select = new MySqlCommand(@"
                SELECT h.ID, h.first_name, h.middle_name, h.last_name, h.birthday, h.barangay_id, b.name AS barangay
                FROM health_records h
                LEFT JOIN barangays b ON b.id = h.barangay_id
                WHERE h.resident_code IS NULL OR h.resident_code = ''
                ORDER BY h.barangay_id, h.ID", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(select).Fill(rows);

            Dictionary<string, string> assigned = new Dictionary<string, string>();
            Dictionary<int, int> counters = GetResidentCounters(conn);

            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string barangay = Text(row["barangay"]);
                string prefix = PrefixForBarangay(barangay);
                string key = barangayId + "|" + Text(row["first_name"]).Trim().ToLowerInvariant() + "|"
                    + Text(row["middle_name"]).Trim().ToLowerInvariant() + "|"
                    + Text(row["last_name"]).Trim().ToLowerInvariant() + "|" + BirthdayKey(row["birthday"]);

                if (!assigned.TryGetValue(key, out string code))
                {
                    code = FindExistingResidentCode(conn, Text(row["first_name"]), Text(row["middle_name"]), Text(row["last_name"]),
                        DateValue(row["birthday"]), barangayId, "");
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        counters.TryGetValue(barangayId, out int next);
                        next++;
                        counters[barangayId] = next;
                        code = prefix + next.ToString("000", CultureInfo.InvariantCulture);
                    }
                    assigned[key] = code;
                }

                UpdateCode(conn, "health_records", "resident_code", "ID", ToInt(row["ID"]), code);
            }
        }

        private static void BackfillUserResidentCodes(MySqlConnection conn)
        {
            using MySqlCommand select = new MySqlCommand(@"
                SELECT u.id, u.first_name, u.middle_name, u.last_name, u.birthday, u.address, u.barangay_id, b.name AS barangay
                FROM users u
                LEFT JOIN barangays b ON b.id = u.barangay_id
                WHERE LOWER(TRIM(u.role)) = 'resident'
                  AND (u.resident_code IS NULL OR u.resident_code = '')
                ORDER BY u.barangay_id, u.id", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(select).Fill(rows);

            Dictionary<string, string> assigned = new Dictionary<string, string>();
            Dictionary<int, int> counters = GetResidentCounters(conn);

            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string barangay = Text(row["barangay"]);
                string prefix = PrefixForBarangay(barangay);
                string key = barangayId + "|" + Text(row["first_name"]).Trim().ToLowerInvariant() + "|"
                    + Text(row["middle_name"]).Trim().ToLowerInvariant() + "|"
                    + Text(row["last_name"]).Trim().ToLowerInvariant() + "|" + BirthdayKey(row["birthday"]);

                if (!assigned.TryGetValue(key, out string code))
                {
                    code = FindExistingResidentCode(conn, Text(row["first_name"]), Text(row["middle_name"]), Text(row["last_name"]),
                        DateValue(row["birthday"]), barangayId, Text(row.Table.Columns.Contains("address") ? row["address"] : ""));
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        counters.TryGetValue(barangayId, out int next);
                        next++;
                        counters[barangayId] = next;
                        code = prefix + next.ToString("000", CultureInfo.InvariantCulture);
                    }
                    assigned[key] = code;
                }

                UpdateCode(conn, "users", "resident_code", "id", ToInt(row["id"]), code);
            }
        }

        private static string FindExistingResidentCode(MySqlConnection conn, string firstName, string middleName, string lastName, DateTime birthday, int barangayId)
        {
            return FindExistingResidentCode(conn, firstName, middleName, lastName, birthday, barangayId, "");
        }

        private static string FindExistingResidentCode(MySqlConnection conn, string firstName, string middleName, string lastName, DateTime birthday, int barangayId, string address)
        {
            using MySqlCommand find = new MySqlCommand(@"
                SELECT resident_code
                FROM (
                    SELECT resident_code COLLATE utf8mb4_unicode_ci AS resident_code, 0 AS source_order, ID AS row_id
                    FROM health_records
                    WHERE barangay_id = @barangayId
                      AND LOWER(TRIM(first_name)) = LOWER(TRIM(@firstName))
                      AND LOWER(TRIM(COALESCE(middle_name, ''))) = LOWER(TRIM(@middleName))
                      AND LOWER(TRIM(last_name)) = LOWER(TRIM(@lastName))
                      AND (@hasBirthday = 0 OR birthday IS NULL OR DATE(birthday) = @birthday)
                      AND (@address = '' OR LOWER(TRIM(COALESCE(address, ''))) = LOWER(TRIM(@address)))
                      AND resident_code IS NOT NULL
                      AND resident_code <> ''
                    UNION ALL
                    SELECT resident_code COLLATE utf8mb4_unicode_ci AS resident_code, 1 AS source_order, id AS row_id
                    FROM users
                    WHERE barangay_id = @barangayId
                      AND LOWER(TRIM(role)) = 'resident'
                      AND LOWER(TRIM(first_name)) = LOWER(TRIM(@firstName))
                      AND LOWER(TRIM(COALESCE(middle_name, ''))) = LOWER(TRIM(@middleName))
                      AND LOWER(TRIM(last_name)) = LOWER(TRIM(@lastName))
                      AND (@hasBirthday = 0 OR birthday IS NULL OR DATE(birthday) = @birthday)
                      AND (@address = '' OR LOWER(TRIM(COALESCE(address, ''))) = LOWER(TRIM(@address)))
                      AND resident_code IS NOT NULL
                      AND resident_code <> ''
                ) matched
                ORDER BY source_order, row_id
                LIMIT 1", conn);
            find.Parameters.AddWithValue("@barangayId", barangayId);
            find.Parameters.AddWithValue("@firstName", firstName.Trim());
            find.Parameters.AddWithValue("@middleName", (middleName ?? "").Trim());
            find.Parameters.AddWithValue("@lastName", lastName.Trim());
            find.Parameters.AddWithValue("@address", (address ?? "").Trim());
            bool hasBirthday = birthday > DateTime.MinValue;
            find.Parameters.AddWithValue("@hasBirthday", hasBirthday ? 1 : 0);
            find.Parameters.AddWithValue("@birthday", hasBirthday ? birthday.Date : DateTime.Today);
            object existing = find.ExecuteScalar();
            return existing == null || existing == DBNull.Value
                ? ""
                : Convert.ToString(existing, CultureInfo.InvariantCulture) ?? "";
        }

        private static string NextResidentCode(MySqlConnection conn, int barangayId)
        {
            string prefix = PrefixForBarangay(GetBarangayName(conn, barangayId));
            Dictionary<int, int> counters = GetResidentCounters(conn);
            counters.TryGetValue(barangayId, out int next);
            return prefix + (next + 1).ToString("000", CultureInfo.InvariantCulture);
        }

        private static void BackfillItemCodes(MySqlConnection conn, string tableName, string codeColumn, string suffix)
        {
            using MySqlCommand select = new MySqlCommand($@"
                SELECT t.id, t.barangay_id, b.name AS barangay
                FROM {tableName} t
                LEFT JOIN barangays b ON b.id = t.barangay_id
                WHERE t.{codeColumn} IS NULL OR t.{codeColumn} = ''
                ORDER BY t.barangay_id, t.id", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(select).Fill(rows);

            Dictionary<int, int> counters = GetCounters(conn, tableName, codeColumn, suffix);
            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string prefix = PrefixForBarangay(Text(row["barangay"])) + suffix;
                counters.TryGetValue(barangayId, out int next);
                next++;
                counters[barangayId] = next;
                string code = prefix + next.ToString("000", CultureInfo.InvariantCulture);
                UpdateCode(conn, tableName, codeColumn, "id", ToInt(row["id"]), code);
            }
        }

        private static void BackfillAppointmentCodes(MySqlConnection conn)
        {
            using MySqlCommand select = new MySqlCommand(@"
                SELECT a.id, COALESCE(a.barangay_id, u.barangay_id) AS barangay_id, b.name AS barangay
                FROM appointments a
                LEFT JOIN users u ON u.id = a.user_id
                LEFT JOIN barangays b ON b.id = COALESCE(a.barangay_id, u.barangay_id)
                WHERE a.appointment_code IS NULL OR a.appointment_code = ''
                ORDER BY COALESCE(a.barangay_id, u.barangay_id), a.id", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(select).Fill(rows);

            Dictionary<int, int> counters = GetCounters(conn, "appointments", "appointment_code", "APP");
            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string prefix = PrefixForBarangay(Text(row["barangay"])) + "APP";
                counters.TryGetValue(barangayId, out int next);
                next++;
                counters[barangayId] = next;
                string code = prefix + next.ToString("000", CultureInfo.InvariantCulture);
                if (barangayId > 0)
                {
                    using MySqlCommand updateBarangay = new MySqlCommand(
                        "UPDATE appointments SET barangay_id = COALESCE(barangay_id, @barangayId) WHERE id = @id", conn);
                    updateBarangay.Parameters.AddWithValue("@barangayId", barangayId);
                    updateBarangay.Parameters.AddWithValue("@id", ToInt(row["id"]));
                    updateBarangay.ExecuteNonQuery();
                }
                UpdateCode(conn, "appointments", "appointment_code", "id", ToInt(row["id"]), code);
            }
        }

        private static void BackfillAppointmentBarangays(MySqlConnection conn)
        {
            using MySqlCommand update = new MySqlCommand(@"
                UPDATE appointments a
                INNER JOIN users u ON u.id = a.user_id
                SET a.barangay_id = u.barangay_id
                WHERE a.barangay_id IS NULL
                  AND u.barangay_id IS NOT NULL", conn);
            update.ExecuteNonQuery();
        }

        private static Dictionary<int, int> GetCounters(MySqlConnection conn, string tableName, string codeColumn, string suffix)
        {
            using MySqlCommand cmd = new MySqlCommand($@"
                SELECT t.barangay_id, b.name AS barangay, t.{codeColumn} AS code
                FROM {tableName} t
                LEFT JOIN barangays b ON b.id = t.barangay_id
                WHERE t.{codeColumn} IS NOT NULL AND t.{codeColumn} <> ''", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(cmd).Fill(rows);

            Dictionary<int, int> counters = new Dictionary<int, int>();
            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string prefix = PrefixForBarangay(Text(row["barangay"])) + suffix;
                string code = Text(row["code"]);
                if (!code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
                if (!int.TryParse(code.Substring(prefix.Length), NumberStyles.Integer, CultureInfo.InvariantCulture, out int number)) continue;
                if (!counters.ContainsKey(barangayId) || counters[barangayId] < number)
                    counters[barangayId] = number;
            }

            return counters;
        }

        private static Dictionary<int, int> GetResidentCounters(MySqlConnection conn)
        {
            Dictionary<int, int> counters = GetCounters(conn, "health_records", "resident_code", "");
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT u.barangay_id, b.name AS barangay, u.resident_code AS code
                FROM users u
                LEFT JOIN barangays b ON b.id = u.barangay_id
                WHERE LOWER(TRIM(u.role)) = 'resident'
                  AND u.resident_code IS NOT NULL
                  AND u.resident_code <> ''", conn);
            DataTable rows = new DataTable();
            new MySqlDataAdapter(cmd).Fill(rows);

            foreach (DataRow row in rows.Rows)
            {
                int barangayId = ToInt(row["barangay_id"]);
                string prefix = PrefixForBarangay(Text(row["barangay"]));
                string code = Text(row["code"]);
                if (!code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
                if (!int.TryParse(code.Substring(prefix.Length), NumberStyles.Integer, CultureInfo.InvariantCulture, out int number)) continue;
                if (!counters.ContainsKey(barangayId) || counters[barangayId] < number)
                    counters[barangayId] = number;
            }

            return counters;
        }

        private static string NextCode(MySqlConnection conn, string tableName, string codeColumn, string prefix, int barangayId)
        {
            using MySqlCommand cmd = new MySqlCommand($@"
                SELECT {codeColumn}
                FROM {tableName}
                WHERE barangay_id = @barangayId
                  AND {codeColumn} LIKE @prefix COLLATE utf8mb4_0900_ai_ci
                ORDER BY {codeColumn} DESC
                LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@barangayId", barangayId);
            cmd.Parameters.AddWithValue("@prefix", prefix + "%");
            string lastCode = Convert.ToString(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) ?? "";
            int next = 1;
            if (lastCode.Length > prefix.Length
                && int.TryParse(lastCode.Substring(prefix.Length), NumberStyles.Integer, CultureInfo.InvariantCulture, out int lastNumber))
                next = lastNumber + 1;
            return prefix + next.ToString("000", CultureInfo.InvariantCulture);
        }

        private static string GetBarangayName(MySqlConnection conn, int barangayId)
        {
            using MySqlCommand cmd = new MySqlCommand("SELECT name FROM barangays WHERE id = @id LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@id", barangayId);
            return Convert.ToString(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) ?? "";
        }

        private static void UpdateCode(MySqlConnection conn, string tableName, string codeColumn, string idColumn, int id, string code)
        {
            using MySqlCommand update = new MySqlCommand($"UPDATE {tableName} SET {codeColumn} = @code WHERE {idColumn} = @id", conn);
            update.Parameters.AddWithValue("@code", code);
            update.Parameters.AddWithValue("@id", id);
            update.ExecuteNonQuery();
        }

        private static string PrefixForBarangay(string barangay)
        {
            switch ((barangay ?? "").Trim().ToLowerInvariant())
            {
                case "cabadiangan": return "CB";
                case "calero": return "CL";
                case "catarman": return "CT";
                case "cotcot": return "CC";
                case "jubay": return "JB";
                case "lataban": return "LB";
                case "mulao": return "ML";
                case "poblacion": return "PB";
                case "san roque": return "SR";
                case "san vicente": return "SV";
                case "santa cruz": return "SC";
                case "tabla": return "TB";
                case "tayud": return "TD";
                case "yati": return "YT";
                default: return "BR";
            }
        }

        private static string BirthdayKey(object value)
        {
            return value == null || value == DBNull.Value
                ? ""
                : Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static DateTime DateValue(object value)
        {
            return value == null || value == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(value, CultureInfo.InvariantCulture).Date;
        }

        private static string Text(object value)
        {
            return value == null || value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }

        private static int ToInt(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }
    }
}
