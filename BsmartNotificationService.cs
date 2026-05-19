using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class BsmartNotificationService
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private const string AttachedTag = "BsmartNotificationAttached";
        private const string DropdownName = "BsmartLiveNotificationDropdown";
        private const string DotName = "BsmartNotificationRedDot";
        private const string ToastName = "BsmartNotificationToast";
        private static readonly HashSet<string> ToastedForms = new HashSet<string>();
        private static readonly HashSet<string> ToastedItems = new HashSet<string>();
        private static readonly HashSet<string> ViewedNotificationSets = new HashSet<string>();
        private static readonly Dictionary<Form, System.Windows.Forms.Timer> RefreshTimers = new Dictionary<Form, System.Windows.Forms.Timer>();
        private static readonly List<NotificationItem> RecentInventoryArchiveNotifications = new List<NotificationItem>();

        private sealed class NotificationItem
        {
            public string Title { get; set; } = "";
            public string Detail { get; set; } = "";
            public string Time { get; set; } = "";
            public DateTime SortAt { get; set; } = DateTime.MinValue;
            public int Priority { get; set; } = 0;
            public Color Accent { get; set; } = Color.FromArgb(0, 120, 215);
            public string Target { get; set; } = "";
        }

        public static void Attach(Button button, Form owner)
        {
            if (button == null || owner == null) return;
            StartAutoRefresh(owner, button);
            if (Equals(button.Tag, AttachedTag)) return;

            button.Tag = AttachedTag;
            button.Click += (s, e) => Show(owner, button);
            RefreshIndicator(owner, button, showToast: true);
        }

        private static void StartAutoRefresh(Form owner, Button button)
        {
            if (RefreshTimers.ContainsKey(owner)) return;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += (s, e) =>
            {
                if (owner.IsDisposed || button.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    RefreshTimers.Remove(owner);
                    return;
                }

                RefreshIndicator(owner, button, showToast: true);
            };

            owner.FormClosed += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();
                RefreshTimers.Remove(owner);
            };

            RefreshTimers[owner] = timer;
            timer.Start();
        }

        public static void AttachToOpenForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                Button button = FindButton(form.Controls, "btnNotification");
                if (button == null) continue;

                Attach(button, form);
            }
        }

        public static void RefreshOpenIndicators(bool showToast)
        {
            foreach (Form form in Application.OpenForms)
            {
                Button button = FindButton(form.Controls, "btnNotification");
                if (button == null) continue;

                Attach(button, form);
                RefreshIndicator(form, button, showToast);
            }
        }

        public static void Show(Form owner, Control anchor)
        {
            if (owner == null || anchor == null) return;

            Control existing = owner.Controls.Find(DropdownName, false).FirstOrDefault();
            if (existing != null)
            {
                owner.Controls.Remove(existing);
                existing.Dispose();
                return;
            }

            List<NotificationItem> items = LoadNotifications();
            string signature = NotificationSignature(items);
            if (!string.IsNullOrWhiteSpace(signature))
            {
                ViewedNotificationSets.Add(ViewerKey() + ":" + signature);
                MarkViewed(signature);
            }

            if (anchor is Button button)
                SetRedDot(button, false);

            Panel dropdown = BuildDropdown(owner, anchor, items);
            owner.Controls.Add(dropdown);
            dropdown.BringToFront();
        }

        public static void RefreshIndicator(Form owner, Button button, bool showToast)
        {
            List<NotificationItem> items = LoadNotifications();
            string signature = NotificationSignature(items);
            bool alreadyViewed = string.IsNullOrWhiteSpace(signature) ||
                ViewedNotificationSets.Contains(ViewerKey() + ":" + signature) ||
                IsViewed(signature);

            SetRedDot(button, items.Count > 0 && !alreadyViewed);

            if (showToast && items.Count > 0 && !alreadyViewed)
            {
                string key = ViewerKey() + ":" + signature;
                if (ToastedForms.Add(key))
                    ShowToasts(owner, items);
            }
        }

        private static string ViewerKey()
        {
            return $"{Session.Role}:{Session.UserID}:{Session.BarangayID}";
        }

        private static string NotificationSignature(List<NotificationItem> items)
        {
            if (items == null || items.Count == 0) return "";
            return string.Join("|", items.Select(NotificationSignature));
        }

        private static string NotificationSignature(NotificationItem item)
        {
            if (item == null) return "";
            return item.Title + "~" + item.Detail + "~" + item.Time + "~" + item.Target;
        }

        private static bool IsViewed(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature)) return true;

            try
            {
                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                using MySqlCommand cmd = new MySqlCommand(@"
                    SELECT COUNT(*)
                    FROM notification_views
                    WHERE viewer_key = @viewerKey
                      AND signature_hash = @hash", conn);
                cmd.Parameters.AddWithValue("@viewerKey", ViewerKey());
                cmd.Parameters.AddWithValue("@hash", SignatureHash(signature));
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }

        private static void MarkViewed(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature)) return;

            try
            {
                BsmartDatabaseInitializer.EnsureSupportTables();
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                using MySqlCommand cmd = new MySqlCommand(@"
                    INSERT IGNORE INTO notification_views (viewer_key, signature_hash)
                    VALUES (@viewerKey, @hash)", conn);
                cmd.Parameters.AddWithValue("@viewerKey", ViewerKey());
                cmd.Parameters.AddWithValue("@hash", SignatureHash(signature));
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // In-memory viewed state remains active for this session.
            }
        }

        private static string SignatureHash(string signature)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(signature));
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        private static void SetRedDot(Button button, bool visible)
        {
            Control dot = button.Controls.Find(DotName, false).FirstOrDefault();
            if (!visible)
            {
                if (dot != null)
                {
                    button.Controls.Remove(dot);
                    dot.Dispose();
                }
                return;
            }

            if (dot != null)
            {
                dot.Visible = true;
                dot.BringToFront();
                return;
            }

            Panel redDot = new Panel
            {
                Name = DotName,
                Size = new Size(13, 13),
                Location = new Point(button.Width - 15, 2),
                BackColor = Color.Transparent
            };
            redDot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using SolidBrush brush = new SolidBrush(Color.Red);
                e.Graphics.FillEllipse(brush, 1, 1, 11, 11);
            };
            button.Controls.Add(redDot);
            redDot.BringToFront();
        }

        private static void ShowToasts(Form owner, List<NotificationItem> items)
        {
            int shown = 0;
            foreach (NotificationItem item in items.Take(5))
            {
                string itemKey = ViewerKey() + ":" + NotificationSignature(item);
                if (!ToastedItems.Add(itemKey)) continue;

                ShowToast(owner, item, shown);
                shown++;
            }
        }

        private static void ShowToast(Form owner, NotificationItem item, int index)
        {
            Panel toast = new Panel
            {
                Name = ToastName + index.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Size = new Size(360, 82),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = item
            };
            toast.Location = new Point(Math.Max(8, owner.ClientSize.Width - toast.Width - 20), 78 + (index * 90));
            toast.Controls.Add(new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(6, toast.Height),
                BackColor = item.Accent
            });
            toast.Controls.Add(new Label
            {
                Text = item.Title,
                Location = new Point(16, 10),
                Size = new Size(330, 22),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 42, 70),
                AutoEllipsis = true
            });
            toast.Controls.Add(new Label
            {
                Text = item.Detail,
                Location = new Point(16, 34),
                Size = new Size(330, 38),
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(60, 87, 110),
                AutoEllipsis = true
            });
            WireNotificationClick(owner, toast, item);

            owner.Controls.Add(toast);
            toast.BringToFront();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 4500 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();
                if (!toast.IsDisposed && toast.Parent != null)
                {
                    toast.Parent.Controls.Remove(toast);
                    toast.Dispose();
                }
            };
            timer.Start();
        }

        private static Panel BuildDropdown(Form owner, Control anchor, List<NotificationItem> items)
        {
            const int panelWidth = 430;
            const int panelHeight = 440;
            const int headerHeight = 46;

            Point anchorClient = owner.PointToClient(anchor.PointToScreen(Point.Empty));
            int x = anchorClient.X + anchor.Width - panelWidth;
            int y = anchorClient.Y + anchor.Height + 6;
            if (x < 8) x = 8;
            if (y + panelHeight > owner.ClientSize.Height) y = Math.Max(8, anchorClient.Y - panelHeight - 6);

            Panel dropdown = new Panel
            {
                Name = DropdownName,
                Location = new Point(x, y),
                Size = new Size(panelWidth, panelHeight),
                BackColor = Color.FromArgb(232, 246, 253),
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = headerHeight,
                BackColor = Color.FromArgb(0, 153, 204)
            };
            header.Controls.Add(new Label
            {
                Text = "Notifications",
                AutoSize = false,
                Location = new Point(14, 10),
                Size = new Size(panelWidth - 70, 26),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            });

            Button close = new Button
            {
                Text = "X",
                Location = new Point(panelWidth - 42, 8),
                Size = new Size(28, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 153, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            close.FlatAppearance.BorderSize = 0;
            close.Click += (s, e) =>
            {
                owner.Controls.Remove(dropdown);
                dropdown.Dispose();
            };
            header.Controls.Add(close);

            Panel list = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(232, 246, 253),
                Padding = new Padding(8, 8, 8, 8)
            };

            int yOffset = 8;
            if (items.Count == 0)
            {
                list.Controls.Add(new Label
                {
                    Text = "No new notifications.",
                    Location = new Point(18, 22),
                    Size = new Size(panelWidth - 54, 32),
                    ForeColor = Color.FromArgb(35, 70, 90),
                    Font = new Font("Segoe UI", 10)
                });
            }
            else
            {
                foreach (NotificationItem item in items)
                {
                    Panel row = BuildRow(owner, item, panelWidth - 38, yOffset);
                    list.Controls.Add(row);
                    yOffset += row.Height + 8;
                }
            }

            dropdown.Controls.Add(list);
            dropdown.Controls.Add(header);
            return dropdown;
        }

        private static Panel BuildRow(Form owner, NotificationItem item, int width, int y)
        {
            string detail = string.IsNullOrWhiteSpace(item.Time) ? item.Detail : item.Detail + Environment.NewLine + item.Time;
            int detailHeight = Math.Max(42, TextRenderer.MeasureText(detail, new Font("Segoe UI", 9), new Size(width - 34, 0), TextFormatFlags.WordBreak).Height + 6);
            int height = 42 + detailHeight;

            Panel row = new Panel
            {
                Location = new Point(8, y),
                Size = new Size(width, height),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = item
            };

            Panel accent = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(5, height),
                BackColor = item.Accent
            };
            row.Controls.Add(accent);

            row.Controls.Add(new Label
            {
                Text = item.Title,
                Location = new Point(14, 8),
                Size = new Size(width - 26, 22),
                ForeColor = Color.FromArgb(15, 42, 70),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoEllipsis = true,
                Cursor = Cursors.Hand
            });

            row.Controls.Add(new Label
            {
                Text = detail,
                Location = new Point(14, 32),
                Size = new Size(width - 26, detailHeight),
                ForeColor = Color.FromArgb(60, 87, 110),
                Font = new Font("Segoe UI", 9),
                AutoEllipsis = false,
                Cursor = Cursors.Hand
            });
            WireNotificationClick(owner, row, item);

            return row;
        }

        private static void WireNotificationClick(Form owner, Control control, NotificationItem item)
        {
            control.Click += (s, e) => NavigateToNotification(owner, item);
            foreach (Control child in control.Controls)
            {
                child.Cursor = Cursors.Hand;
                WireNotificationClick(owner, child, item);
            }
        }

        private static void NavigateToNotification(Form owner, NotificationItem item)
        {
            if (owner == null || item == null || string.IsNullOrWhiteSpace(item.Target)) return;

            Control dropdown = owner.Controls.Find(DropdownName, false).FirstOrDefault();
            if (dropdown != null)
            {
                owner.Controls.Remove(dropdown);
                dropdown.Dispose();
            }

            foreach (Control toast in owner.Controls.Cast<Control>().Where(c => c.Name.StartsWith(ToastName, StringComparison.Ordinal)).ToList())
            {
                owner.Controls.Remove(toast);
                toast.Dispose();
            }

            Form next = CreateTargetForm(item.Target);
            if (next == null) return;
            BsmartFormNavigator.Open(owner, next);
        }

        private static Form CreateTargetForm(string target)
        {
            string role = (Session.Role ?? "").Trim();
            bool isStaff = role.Equals("LGUStaff", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("LGU Staff", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("Staff", StringComparison.OrdinalIgnoreCase);
            bool isCaptain = role.Equals("Captain", StringComparison.OrdinalIgnoreCase);
            bool isMayor = role.Equals("Mayor", StringComparison.OrdinalIgnoreCase) || Session.IsMayor;
            bool isResident = role.Equals("Resident", StringComparison.OrdinalIgnoreCase);

            switch (target)
            {
                case "Inventory":
                    if (isStaff) return new TayudInventory();
                    if (isMayor) return new MayorViewInventory();
                    return null;
                case "Archive":
                    if (isStaff) return new TayudViewArchive();
                    if (isCaptain) return new TayudBCapViewArch();
                    return null;
                case "Appointments":
                    if (isStaff) return new TayudViewAppointments();
                    if (isResident) return new ResidentAppointment();
                    return null;
                case "ResidentHealthRecords":
                    if (isStaff) return new TayudViewHealth();
                    if (isCaptain) return new TayudBCViewRes();
                    if (isMayor) return new MayorResHealthRec();
                    if (isResident) return new ResViewHealth();
                    return null;
                case "ResidentRecords":
                    if (isStaff) return new TayudViewHealth();
                    if (isCaptain) return new TayudBCViewRes();
                    if (isMayor) return new MayorResRec();
                    return null;
                case "ResidentAccounts":
                    if (isCaptain) return new TayudBCapViewResAcc();
                    if (isMayor) return new MayorResRec();
                    return null;
                default:
                    return null;
            }
        }

        private static List<NotificationItem> LoadNotifications()
        {
            List<NotificationItem> items = new List<NotificationItem>();

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                string role = (Session.Role ?? "").Trim();
                if (role.Equals("LGUStaff", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("LGU Staff", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    items.AddRange(LoadNewResidentRecords(conn, true));
                    items.AddRange(LoadPendingAppointments(conn));
                    items.AddRange(LoadRecentInventoryArchiveNotifications());
                    items.AddRange(LoadLowStockInventory(conn, true));
                    items.AddRange(ArchiveNearExpiryInventory(conn, true));
                }
                else if (role.Equals("Mayor", StringComparison.OrdinalIgnoreCase) || Session.IsMayor)
                {
                    items.AddRange(LoadLowStockInventory(conn, false));
                    items.AddRange(LoadRecentInventoryArchiveNotifications());
                    items.AddRange(ArchiveNearExpiryInventory(conn, false));
                    items.AddRange(LoadNewResidentRecords(conn, false));
                    items.AddRange(LoadResidentRegistrations(conn, false));
                }
                else if (role.Equals("Captain", StringComparison.OrdinalIgnoreCase))
                {
                    items.AddRange(LoadLowStockInventory(conn, true));
                    items.AddRange(LoadRecentInventoryArchiveNotifications());
                    items.AddRange(ArchiveNearExpiryInventory(conn, true));
                    items.AddRange(LoadNewResidentRecords(conn, true));
                    items.AddRange(LoadResidentRegistrations(conn, true));
                }
                else if (role.Equals("Resident", StringComparison.OrdinalIgnoreCase))
                {
                    items.AddRange(LoadApprovedAppointments(conn));
                }
            }
            catch (Exception ex)
            {
                items.Clear();
                items.Add(new NotificationItem
                {
                    Title = "Notification Error",
                    Detail = "Unable to load notifications: " + ex.Message,
                    Accent = Color.FromArgb(205, 55, 55)
                });
            }

            return items
                .OrderByDescending(i => i.SortAt)
                .ThenByDescending(i => i.Priority)
                .Take(12)
                .ToList();
        }

        private static IEnumerable<NotificationItem> LoadRecentInventoryArchiveNotifications()
        {
            DateTime cutoff = DateTime.Now.AddDays(-1);
            RecentInventoryArchiveNotifications.RemoveAll(i => i.SortAt < cutoff);
            return RecentInventoryArchiveNotifications.ToList();
        }

        private static IEnumerable<NotificationItem> LoadLowStockInventory(MySqlConnection conn, bool scoped)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            items.AddRange(LoadLowStockTable(conn, "medicines", "Medicine", scoped));
            items.AddRange(LoadLowStockTable(conn, "vaccines", "Vaccine", scoped));
            return items;
        }

        private static IEnumerable<NotificationItem> LoadLowStockTable(MySqlConnection conn, string table, string label, bool scoped)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            string scopeSql = scoped ? " AND (@brgyId = 0 OR i.barangay_id = @brgyId)" : "";
            int threshold = BsmartAppSettings.LowStockThreshold;

            using MySqlCommand cmd = new MySqlCommand($@"
                SELECT i.id,
                       i.name,
                       COALESCE(i.quantity, 0) AS quantity,
                       COALESCE(b.name, @brgyName, 'No barangay') AS barangay
                FROM {table} i
                LEFT JOIN barangays b ON b.id = i.barangay_id
                WHERE COALESCE(i.is_archived, 0) = 0
                  AND COALESCE(i.quantity, 0) < @threshold{scopeSql}
                ORDER BY COALESCE(i.quantity, 0) ASC, i.name ASC
                LIMIT 8", conn);

            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
            cmd.Parameters.AddWithValue("@threshold", threshold);
            if (scoped) cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int quantity = Convert.ToInt32(reader["quantity"]);
                    items.Add(new NotificationItem
                    {
                        Title = label + " Low Stock",
                        Detail = $"{Text(reader["name"], label)} has only {quantity} unit{(quantity == 1 ? "" : "s")} left in {Text(reader["barangay"], "this barangay")}.",
                        Time = "Restock needed",
                        Priority = 130,
                        SortAt = DateTime.Now.AddMinutes(-quantity),
                        Accent = Color.FromArgb(220, 120, 35),
                        Target = "Inventory"
                    });
            }

            return items;
        }

        private static IEnumerable<NotificationItem> LoadPendingAppointments(MySqlConnection conn)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT a.id,
                       TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(u.first_name, ' ', u.last_name))) AS resident,
                       COALESCE(s.name, 'Health service') AS service_name,
                       a.appt_date,
                       a.time_slot,
                       COALESCE(a.created_at, DATE_ADD('2000-01-01', INTERVAL a.id SECOND)) AS sort_at
                FROM appointments a
                LEFT JOIN users u ON u.id = a.user_id
                LEFT JOIN health_services s ON s.id = a.service_id
                LEFT JOIN barangays b ON b.id = COALESCE(a.barangay_id, u.barangay_id)
                WHERE a.status = 'Pending'
                  AND (@brgyId = 0
                       OR a.barangay_id = @brgyId
                       OR u.barangay_id = @brgyId
                       OR b.name = @brgyName)
                ORDER BY COALESCE(a.created_at, DATE_ADD('2000-01-01', INTERVAL a.id SECOND)) DESC, a.id DESC
                LIMIT 6", conn);

            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime date = reader["appt_date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["appt_date"]);
                items.Add(new NotificationItem
                {
                    Title = "Appointment Needs Approval",
                    Detail = $"{Text(reader["resident"], "Resident")} booked {Text(reader["service_name"], "a service")} on {date:MMM d, yyyy} at {Text(reader["time_slot"], "No time")}.",
                    Time = "Pending approval",
                    Priority = 110,
                    SortAt = reader["sort_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["sort_at"]),
                    Accent = Color.FromArgb(255, 145, 45),
                    Target = "Appointments"
                });
            }

            return items;
        }

        private static IEnumerable<NotificationItem> ArchiveNearExpiryInventory(MySqlConnection conn, bool scoped)
        {
            List<NotificationItem> archived = new List<NotificationItem>();
            archived.AddRange(ArchiveNearExpiryTable(conn, "medicines", "Medicine", scoped));
            archived.AddRange(ArchiveNearExpiryTable(conn, "vaccines", "Vaccine", scoped));
            return archived;
        }

        private static IEnumerable<NotificationItem> ArchiveNearExpiryTable(MySqlConnection conn, string table, string label, bool scoped)
        {
            List<int> ids = new List<int>();
            List<NotificationItem> items = new List<NotificationItem>();

            string scopeSql = scoped ? " AND (@brgyId = 0 OR barangay_id = @brgyId)" : "";
            using (MySqlCommand select = new MySqlCommand($@"
                SELECT id, name, expiry_date
                FROM {table}
                WHERE COALESCE(is_archived, 0) = 0
                  AND expiry_date IS NOT NULL
                  AND expiry_date <= DATE_ADD(CURDATE(), INTERVAL 7 DAY){scopeSql}
                ORDER BY expiry_date ASC
                LIMIT 10", conn))
            {
                if (scoped) select.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                using MySqlDataReader reader = select.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    DateTime expiry = Convert.ToDateTime(reader["expiry_date"]);
                    ids.Add(id);

                    NotificationItem item = new NotificationItem
                    {
                        Title = label + " Auto-Archived",
                        Detail = $"{Text(reader["name"], label)} expires on {expiry:MMM d, yyyy}. It was moved to archive.",
                        Time = "Archived automatically",
                        Priority = 120,
                        SortAt = DateTime.Now,
                        Accent = Color.FromArgb(205, 55, 55),
                        Target = "Archive"
                    };
                    items.Add(item);
                    RememberInventoryArchiveNotification(item);
                }
            }

            foreach (int id in ids)
            {
                using MySqlCommand update = new MySqlCommand($"UPDATE {table} SET is_archived = 1 WHERE id = @id", conn);
                update.Parameters.AddWithValue("@id", id);
                update.ExecuteNonQuery();
                BsmartAuditService.Log(label + " Auto Archived", table, id, "Near-expiry inventory was moved to archive automatically.");
            }

            return items;
        }

        private static void RememberInventoryArchiveNotification(NotificationItem item)
        {
            if (RecentInventoryArchiveNotifications.Any(i =>
                i.Title == item.Title &&
                i.Detail == item.Detail &&
                i.Time == item.Time))
                return;

            RecentInventoryArchiveNotifications.Add(item);
        }

        private static IEnumerable<NotificationItem> LoadNewResidentRecords(MySqlConnection conn, bool scoped)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            string scopeSql = scoped ? " AND (@brgyId = 0 OR h.barangay_id = @brgyId OR b.name = @brgyName)" : "";

            using MySqlCommand cmd = new MySqlCommand($@"
                SELECT h.ID,
                       CONCAT(h.first_name, ' ', h.last_name) AS resident,
                       COALESCE(b.name, 'No barangay') AS barangay,
                       h.Date AS record_date,
                       h.diagnosis,
                       h.treatment,
                       h.assigned_doc_name
                FROM health_records h
                LEFT JOIN barangays b ON b.id = h.barangay_id
                WHERE COALESCE(h.is_archived, 0) = 0{scopeSql}
                ORDER BY h.ID DESC
                LIMIT 6", conn);

            if (scoped)
            {
                cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
            }

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime date = reader["record_date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["record_date"]);
                bool hasHealthRecord = HasHealthRecordDetails(
                    reader["diagnosis"],
                    reader["treatment"],
                    reader["assigned_doc_name"]);
                string resident = Text(reader["resident"], "Resident");
                string barangay = Text(reader["barangay"], "No barangay");

                items.Add(new NotificationItem
                {
                    Title = hasHealthRecord ? "New Resident Health Data" : "New Resident Added",
                    Detail = hasHealthRecord
                        ? $"{resident} has a new health record in {barangay}."
                        : $"{resident} was added as a resident in {barangay}.",
                    Time = date.ToString("MMM d, yyyy"),
                    Priority = 100,
                    SortAt = date.Date.AddSeconds(Convert.ToInt32(reader["ID"])),
                    Accent = hasHealthRecord ? Color.FromArgb(0, 142, 190) : Color.FromArgb(0, 155, 105),
                    Target = hasHealthRecord ? "ResidentHealthRecords" : "ResidentRecords"
                });
            }

            return items;
        }

        private static IEnumerable<NotificationItem> LoadResidentRegistrations(MySqlConnection conn, bool scoped)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            string scopeSql = scoped ? " AND (@brgyId = 0 OR u.barangay_id = @brgyId OR b.name = @brgyName)" : "";

            using MySqlCommand cmd = new MySqlCommand($@"
                SELECT u.id,
                       TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(u.first_name, ' ', u.last_name))) AS resident,
                       COALESCE(b.name, 'No barangay') AS barangay,
                       DATE_ADD('2000-01-01', INTERVAL u.id SECOND) AS sort_at
                FROM users u
                LEFT JOIN barangays b ON b.id = u.barangay_id
                WHERE u.role = 'Resident'
                  AND COALESCE(u.is_archived, 0) = 0{scopeSql}
                ORDER BY u.id DESC
                LIMIT 6", conn);

            if (scoped)
            {
                cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
            }

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new NotificationItem
                {
                    Title = "Resident Account Registered",
                    Detail = $"{Text(reader["resident"], "Resident")} registered a B-SMART account in {Text(reader["barangay"], "No barangay")}.",
                    Time = "Latest account",
                    SortAt = reader["sort_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["sort_at"]),
                    Accent = Color.FromArgb(0, 155, 105),
                    Target = "ResidentAccounts"
                });
            }

            return items;
        }

        private static IEnumerable<NotificationItem> LoadApprovedAppointments(MySqlConnection conn)
        {
            List<NotificationItem> items = new List<NotificationItem>();
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT a.id,
                       COALESCE(s.name, 'Health service') AS service_name,
                       a.appt_date,
                       a.time_slot,
                       a.assigned_doc_name,
                       COALESCE(a.created_at, DATE_ADD('2000-01-01', INTERVAL a.id SECOND)) AS sort_at
                FROM appointments a
                LEFT JOIN health_services s ON s.id = a.service_id
                WHERE a.user_id = @userId
                  AND a.status = 'Confirmed'
                ORDER BY COALESCE(a.created_at, DATE_ADD('2000-01-01', INTERVAL a.id SECOND)) DESC, a.id DESC
                LIMIT 6", conn);

            cmd.Parameters.AddWithValue("@userId", Session.UserID);
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime date = reader["appt_date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["appt_date"]);
                string doctor = Text(reader["assigned_doc_name"], "No assigned doctor/nurse");
                items.Add(new NotificationItem
                {
                    Title = "Appointment Approved",
                    Detail = $"{Text(reader["service_name"], "Your appointment")} on {date:MMM d, yyyy} at {Text(reader["time_slot"], "No time")} was approved. Assigned: {doctor}.",
                    Time = "Confirmed",
                    SortAt = reader["sort_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["sort_at"]),
                    Accent = Color.FromArgb(0, 155, 105),
                    Target = "Appointments"
                });
            }

            return items;
        }

        private static Button FindButton(Control.ControlCollection controls, string name)
        {
            foreach (Control control in controls)
            {
                if (control is Button button && button.Name == name)
                    return button;

                Button child = FindButton(control.Controls, name);
                if (child != null)
                    return child;
            }

            return null;
        }

        private static string Text(object value, string fallback)
        {
            if (value == null || value == DBNull.Value) return fallback;
            string text = Convert.ToString(value)?.Trim();
            return string.IsNullOrWhiteSpace(text) ? fallback : text;
        }

        private static bool HasHealthRecordDetails(params object[] values)
        {
            foreach (object value in values)
            {
                if (value == null || value == DBNull.Value) continue;
                string text = Convert.ToString(value)?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(text)) continue;
                if (string.Equals(text, "N/A", StringComparison.OrdinalIgnoreCase)) continue;
                if (string.Equals(text, "No record", StringComparison.OrdinalIgnoreCase)) continue;
                return true;
            }

            return false;
        }
    }
}
