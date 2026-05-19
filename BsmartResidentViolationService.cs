using System;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartResidentViolationService
    {
        public static bool HasViolation(MySqlConnection conn, int userId)
        {
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT EXISTS(
                    SELECT 1
                    FROM users u
                    INNER JOIN health_records h
                        ON COALESCE(h.is_archived, 0) = 0
                       AND h.violation IS NOT NULL
                       AND TRIM(h.violation) <> ''
                       AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                       AND LOWER(TRIM(COALESCE(h.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                       AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                       AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                       AND (h.barangay_id = u.barangay_id OR h.barangay_id IS NULL OR u.barangay_id IS NULL)
                    WHERE u.id = @userId
                )", conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
        }

        public static string ExistsSql(string userAlias)
        {
            return $@"EXISTS (
                SELECT 1
                FROM health_records h
                WHERE COALESCE(h.is_archived, 0) = 0
                  AND h.violation IS NOT NULL
                  AND TRIM(h.violation) <> ''
                  AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE({userAlias}.first_name, ''))) COLLATE utf8mb4_unicode_ci
                  AND LOWER(TRIM(COALESCE(h.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE({userAlias}.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                  AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE({userAlias}.last_name, ''))) COLLATE utf8mb4_unicode_ci
                  AND ({userAlias}.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE({userAlias}.birthday))
                  AND (h.barangay_id = {userAlias}.barangay_id OR h.barangay_id IS NULL OR {userAlias}.barangay_id IS NULL)
            )";
        }
    }
}
