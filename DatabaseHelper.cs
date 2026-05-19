using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal class DatabaseHelper
    {
        // --------------------------------------------------------------------
        //  CONNECTION STRING
        //  Change Pwd= to your MySQL root password (leave blank if none)
        // --------------------------------------------------------------------
        private static readonly string connStr =
            "Server=localhost;Database=bsmart;Uid=root;Pwd=;";

        // -- Get a ready-to-use open connection -------------------------------
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        // --------------------------------------------------------------------
        //  HEALTH RECORDS — SELECT
        // --------------------------------------------------------------------

        /// <summary>Returns all active (non-archived) health records.</summary>
        public static DataTable GetAllRecords()
        {
            return RunQuery(@"
                SELECT record_id, first_name, last_name, age, gender,
                       birthday, record_date, diagnosis, treatment
                FROM   health_records
                WHERE  is_archived = 0
                ORDER  BY record_date DESC");
        }

        /// <summary>Returns records matching keyword in name/diagnosis/treatment.</summary>
        public static DataTable SearchRecords(string keyword)
        {
            string sql = @"
                SELECT record_id, first_name, last_name, age, gender,
                       birthday, record_date, diagnosis, treatment
                FROM   health_records
                WHERE  is_archived = 0
                  AND (first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci OR last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                    OR diagnosis  LIKE @kw COLLATE utf8mb4_0900_ai_ci OR treatment  LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                ORDER  BY record_date DESC";

            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@kw", $"%{keyword}%");
                var dt = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        /// <summary>Returns all archived records.</summary>
        public static DataTable GetArchivedRecords()
        {
            return RunQuery(@"
                SELECT record_id, first_name, last_name, age, gender,
                       birthday, record_date, diagnosis, treatment
                FROM   health_records
                WHERE  is_archived = 1
                ORDER  BY updated_at DESC");
        }

        // --------------------------------------------------------------------
        //  HEALTH RECORDS — INSERT
        // --------------------------------------------------------------------
        public static bool AddRecord(string firstName, string lastName, int age,
                                     string gender, DateTime birthday,
                                     DateTime recordDate, string diagnosis,
                                     string treatment)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    // 1. Get or create resident
                    int residentId = GetOrCreateResident(conn,
                        firstName, lastName, age, gender, birthday);

                    // 2. Insert health record
                    string sql = @"
                        INSERT INTO health_records
                            (resident_id, first_name, last_name, age, gender,
                             birthday, record_date, diagnosis, treatment)
                        VALUES
                            (@rid, @fn, @ln, @age, @gender,
                             @bday, @rdate, @diag, @treat)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@rid", residentId);
                        cmd.Parameters.AddWithValue("@fn", firstName);
                        cmd.Parameters.AddWithValue("@ln", lastName);
                        cmd.Parameters.AddWithValue("@age", age);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@bday", birthday.Date);
                        cmd.Parameters.AddWithValue("@rdate", recordDate.Date);
                        cmd.Parameters.AddWithValue("@diag", diagnosis);
                        cmd.Parameters.AddWithValue("@treat", treatment);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Add Record", ex); return false;
            }
        }

        // --------------------------------------------------------------------
        //  HEALTH RECORDS — UPDATE
        // --------------------------------------------------------------------
        public static bool UpdateRecord(int recordId,
                                        string firstName, string lastName,
                                        int age, string gender,
                                        DateTime birthday, DateTime recordDate,
                                        string diagnosis, string treatment)
        {
            try
            {
                string sql = @"
                    UPDATE health_records SET
                        first_name  = @fn,  last_name  = @ln,
                        age         = @age, gender     = @gender,
                        birthday    = @bday,record_date= @rdate,
                        diagnosis   = @diag,treatment  = @treat
                    WHERE record_id = @id";

                using (var conn = GetConnection())
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@fn", firstName);
                    cmd.Parameters.AddWithValue("@ln", lastName);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@bday", birthday.Date);
                    cmd.Parameters.AddWithValue("@rdate", recordDate.Date);
                    cmd.Parameters.AddWithValue("@diag", diagnosis);
                    cmd.Parameters.AddWithValue("@treat", treatment);
                    cmd.Parameters.AddWithValue("@id", recordId);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Update Record", ex); return false;
            }
        }

        // --------------------------------------------------------------------
        //  HEALTH RECORDS — ARCHIVE (soft delete)
        // --------------------------------------------------------------------
        public static bool ArchiveRecord(int recordId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    // Mark archived
                    RunNonQuery(conn,
                        "UPDATE health_records SET is_archived=1 WHERE record_id=@id",
                        ("@id", recordId));

                    // Log it
                    RunNonQuery(conn,
                        "INSERT INTO archive_log (record_id) VALUES (@id)",
                        ("@id", recordId));
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Archive Record", ex); return false;
            }
        }

        // --------------------------------------------------------------------
        //  HEALTH RECORDS — DELETE (permanent)
        // --------------------------------------------------------------------
        public static bool DeleteRecord(int recordId)
        {
            try
            {
                using (var conn = GetConnection())
                    RunNonQuery(conn,
                        "DELETE FROM health_records WHERE record_id=@id",
                        ("@id", recordId));
                return true;
            }
            catch (Exception ex)
            {
                ShowError("Delete Record", ex); return false;
            }
        }

        // --------------------------------------------------------------------
        //  DASHBOARD STATS (for TayudLGU)
        // --------------------------------------------------------------------
        public static int GetTotalResidents()
        {
            var result = RunScalar("SELECT COUNT(*) FROM residents");
            return result == null ? 0 : Convert.ToInt32(result);
        }

        public static int GetTotalRecords()
        {
            var result = RunScalar(
                "SELECT COUNT(*) FROM health_records WHERE is_archived=0");
            return result == null ? 0 : Convert.ToInt32(result);
        }

        public static string GetMostCommonDisease()
        {
            var result = RunScalar(@"
                SELECT   diagnosis
                FROM     health_records
                WHERE    is_archived = 0
                GROUP BY diagnosis
                ORDER BY COUNT(*) DESC
                LIMIT    1");
            return result?.ToString() ?? "N/A";
        }

        // --------------------------------------------------------------------
        //  INVENTORY
        // --------------------------------------------------------------------
        public static DataTable GetInventory()
        {
            return RunQuery("SELECT * FROM inventory ORDER BY item_name");
        }

        public static bool AddInventoryItem(string name, int qty, string unit, int threshold)
        {
            try
            {
                string sql = @"
                    INSERT INTO inventory (item_name, quantity, unit, threshold)
                    VALUES (@name, @qty, @unit, @thresh)";
                using (var conn = GetConnection())
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@unit", unit);
                    cmd.Parameters.AddWithValue("@thresh", threshold);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex) { ShowError("Add Inventory", ex); return false; }
        }

        // --------------------------------------------------------------------
        //  PRIVATE HELPERS
        // --------------------------------------------------------------------

        /// <summary>Finds resident by name+birthday or inserts and returns new id.</summary>
        private static int GetOrCreateResident(MySqlConnection conn,
            string fn, string ln, int age, string gender, DateTime bday)
        {
            string find = @"
                SELECT resident_id FROM residents
                WHERE  first_name=@fn AND last_name=@ln AND birthday=@bday LIMIT 1";

            using (var cmd = new MySqlCommand(find, conn))
            {
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.Parameters.AddWithValue("@bday", bday.Date);
                var r = cmd.ExecuteScalar();
                if (r != null) return Convert.ToInt32(r);
            }

            string insert = @"
                INSERT INTO residents (first_name,last_name,age,gender,birthday)
                VALUES (@fn,@ln,@age,@gender,@bday)";
            using (var cmd = new MySqlCommand(insert, conn))
            {
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@bday", bday.Date);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        /// <summary>Run a SELECT and return a filled DataTable.</summary>
        private static DataTable RunQuery(string sql)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                var dt = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        /// <summary>Run a scalar query and return the single value.</summary>
        private static object RunScalar(string sql)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
                return cmd.ExecuteScalar();
        }

        /// <summary>Run a non-query (INSERT/UPDATE/DELETE) with named params.</summary>
        private static void RunNonQuery(MySqlConnection conn, string sql,
            params (string name, object value)[] parms)
        {
            using (var cmd = new MySqlCommand(sql, conn))
            {
                foreach (var (name, value) in parms)
                    cmd.Parameters.AddWithValue(name, value);
                cmd.ExecuteNonQuery();
            }
        }

        private static void ShowError(string context, Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(
                $"{context} failed:\n{ex.Message}", "Database Error",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}