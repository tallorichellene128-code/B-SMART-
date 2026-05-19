using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public class BsmartAppointmentCalendarForm : Form
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private readonly Button btnToday = new Button();
        private readonly Button btnPrev = new Button();
        private readonly Button btnNext = new Button();
        private readonly Label lblMonth = new Label();
        private readonly TableLayoutPanel calendarGrid = new TableLayoutPanel();
        private readonly Panel sidePanel = new Panel();
        private readonly Label lblSideMonth = new Label();
        private readonly FlowLayoutPanel agendaList = new FlowLayoutPanel();

        private DateTime visibleMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        private DateTime selectedDate = DateTime.Today.Date;
        private Dictionary<DateTime, List<AppointmentItem>> appointmentsByDate = new Dictionary<DateTime, List<AppointmentItem>>();

        public BsmartAppointmentCalendarForm()
        {
            Text = "B-SMART Appointment Calendar";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(980, 620);
            MinimumSize = new Size(900, 560);
            BackColor = Color.White;

            BuildLayout();

            Load += (s, e) =>
            {
                BsmartUiService.PrepareForm(this);
                LoadMonth();
            };
        }

        private void BuildLayout()
        {
            Label title = new Label
            {
                Text = "Appointment Calendar",
                Location = new Point(26, 16),
                Size = new Size(430, 44),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 42, 58)
            };

            btnToday.Text = "Today";
            btnToday.Location = new Point(26, 70);
            btnToday.Size = new Size(96, 36);
            btnToday.Click += (s, e) =>
            {
                selectedDate = DateTime.Today.Date;
                visibleMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                LoadMonth();
            };

            btnPrev.Text = "<";
            btnPrev.Location = new Point(136, 70);
            btnPrev.Size = new Size(44, 36);
            btnPrev.Click += (s, e) =>
            {
                visibleMonth = visibleMonth.AddMonths(-1);
                selectedDate = visibleMonth;
                LoadMonth();
            };

            btnNext.Text = ">";
            btnNext.Location = new Point(182, 70);
            btnNext.Size = new Size(44, 36);
            btnNext.Click += (s, e) =>
            {
                visibleMonth = visibleMonth.AddMonths(1);
                selectedDate = visibleMonth;
                LoadMonth();
            };

            lblMonth.Location = new Point(244, 70);
            lblMonth.Size = new Size(250, 36);
            lblMonth.Font = new Font("Segoe UI", 17F, FontStyle.Underline);
            lblMonth.ForeColor = Color.FromArgb(42, 56, 70);
            lblMonth.TextAlign = ContentAlignment.MiddleLeft;

            Panel modeBar = new Panel
            {
                Location = new Point(520, 70),
                Size = new Size(220, 36),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            Label monthMode = new Label
            {
                Text = "Month",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(92, 92, 92),
                ForeColor = Color.White
            };
            modeBar.Controls.Add(monthMode);

            calendarGrid.Location = new Point(26, 124);
            calendarGrid.Size = new Size(680, 420);
            calendarGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            calendarGrid.BackColor = Color.White;
            calendarGrid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            calendarGrid.ColumnCount = 7;
            calendarGrid.RowCount = 7;

            for (int i = 0; i < 7; i++)
                calendarGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 7F));
            calendarGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            for (int i = 0; i < 6; i++)
                calendarGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / 6F));

            sidePanel.Location = new Point(724, 124);
            sidePanel.Size = new Size(220, 420);
            sidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            sidePanel.BackColor = Color.White;
            sidePanel.BorderStyle = BorderStyle.FixedSingle;

            Label sideTitle = new Label
            {
                Text = "Month Overview",
                Location = new Point(14, 12),
                Size = new Size(190, 28),
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 60, 75)
            };

            lblSideMonth.Location = new Point(14, 44);
            lblSideMonth.Size = new Size(190, 28);
            lblSideMonth.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblSideMonth.ForeColor = Color.FromArgb(80, 80, 80);

            agendaList.Location = new Point(14, 84);
            agendaList.Size = new Size(190, 318);
            agendaList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            agendaList.FlowDirection = FlowDirection.TopDown;
            agendaList.WrapContents = false;
            agendaList.AutoScroll = true;
            agendaList.BackColor = Color.White;

            sidePanel.Controls.Add(sideTitle);
            sidePanel.Controls.Add(lblSideMonth);
            sidePanel.Controls.Add(agendaList);

            Controls.Add(title);
            Controls.Add(btnToday);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            Controls.Add(lblMonth);
            Controls.Add(modeBar);
            Controls.Add(calendarGrid);
            Controls.Add(sidePanel);
        }

        private void LoadMonth()
        {
            try
            {
                appointmentsByDate = LoadAppointmentsForMonth(visibleMonth);
                RenderCalendar();
                RenderAgenda();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading calendar appointments: " + ex.Message, "Calendar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<DateTime, List<AppointmentItem>> LoadAppointmentsForMonth(DateTime month)
        {
            DateTime start = StartOfCalendar(month);
            DateTime end = start.AddDays(42);
            DataTable table = new DataTable();

            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            BsmartIdHelper.EnsureCodes(conn);
            using MySqlCommand cmd = new MySqlCommand(@"
                SELECT a.id,
                       DATE(a.appt_date) AS AppointmentDate,
                       a.time_slot,
                       TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(u.first_name, ' ', u.last_name))) AS Resident,
                       COALESCE(s.name, 'Health service') AS Service,
                       COALESCE(a.assigned_doc_name, 'Not Assigned') AS AssignedDoctor,
                       COALESCE(a.status, 'Pending') AS Status
                FROM appointments a
                LEFT JOIN users u ON u.id = a.user_id
                LEFT JOIN health_services s ON s.id = a.service_id
                LEFT JOIN barangays b ON b.id = COALESCE(a.barangay_id, u.barangay_id)
                WHERE DATE(a.appt_date) >= @start
                  AND DATE(a.appt_date) < @end
                  AND (@isMayor = 1 OR a.barangay_id = @barangayId OR u.barangay_id = @barangayId OR b.name = @barangayName)
                ORDER BY a.appt_date ASC, a.time_slot ASC, a.status ASC", conn);

            cmd.Parameters.AddWithValue("@start", start.Date);
            cmd.Parameters.AddWithValue("@end", end.Date);
            cmd.Parameters.AddWithValue("@isMayor", Session.IsMayor ? 1 : 0);
            cmd.Parameters.AddWithValue("@barangayId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@barangayName", Session.BarangayName ?? "");

            using MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(table);

            return table.Rows.Cast<DataRow>()
                .Select(ToAppointmentItem)
                .GroupBy(item => item.Date)
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        private static AppointmentItem ToAppointmentItem(DataRow row)
        {
            return new AppointmentItem
            {
                Id = Convert.ToInt32(row["id"], CultureInfo.InvariantCulture),
                Date = Convert.ToDateTime(row["AppointmentDate"], CultureInfo.InvariantCulture).Date,
                Time = Convert.ToString(row["time_slot"], CultureInfo.InvariantCulture) ?? "",
                Resident = Convert.ToString(row["Resident"], CultureInfo.InvariantCulture) ?? "",
                Service = Convert.ToString(row["Service"], CultureInfo.InvariantCulture) ?? "Health service",
                AssignedDoctor = Convert.ToString(row["AssignedDoctor"], CultureInfo.InvariantCulture) ?? "",
                Status = Convert.ToString(row["Status"], CultureInfo.InvariantCulture) ?? "Pending"
            };
        }

        private void RenderCalendar()
        {
            calendarGrid.SuspendLayout();
            calendarGrid.Controls.Clear();
            lblMonth.Text = visibleMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
            lblSideMonth.Text = visibleMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            string[] headers = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
            for (int i = 0; i < headers.Length; i++)
            {
                Label header = new Label
                {
                    Text = headers[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(58, 72, 84),
                    BackColor = Color.White
                };
                calendarGrid.Controls.Add(header, i, 0);
            }

            DateTime day = StartOfCalendar(visibleMonth);
            for (int row = 1; row <= 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    calendarGrid.Controls.Add(BuildDayCell(day), col, row);
                    day = day.AddDays(1);
                }
            }

            calendarGrid.ResumeLayout();
        }

        private Panel BuildDayCell(DateTime day)
        {
            bool currentMonth = day.Month == visibleMonth.Month;
            bool isToday = day == DateTime.Today.Date;
            bool isSelected = day == selectedDate.Date;

            Panel cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = isSelected ? Color.FromArgb(245, 248, 250) : Color.White,
                Padding = new Padding(4),
                Cursor = Cursors.Hand,
                Tag = day
            };
            cell.Click += DayCell_Click;

            Label dayNumber = new Label
            {
                Text = day.Day.ToString(CultureInfo.InvariantCulture),
                Location = new Point(5, 4),
                AutoSize = true,
                MinimumSize = new Size(36, 26),
                Font = new Font("Segoe UI", 11F, isToday ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = currentMonth ? Color.FromArgb(33, 48, 62) : Color.FromArgb(130, 140, 150),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };
            dayNumber.Click += DayCell_Click;
            dayNumber.Tag = day;
            cell.Controls.Add(dayNumber);

            if (appointmentsByDate.TryGetValue(day.Date, out List<AppointmentItem>? items))
            {
                int top = 34;
                foreach (AppointmentItem item in items.Take(3))
                {
                    Label chip = BuildAppointmentChip(item, top);
                    chip.Tag = day;
                    chip.Click += DayCell_Click;
                    cell.Controls.Add(chip);
                    top += 25;
                }

                if (items.Count > 3)
                {
                    Label more = new Label
                    {
                        Text = "+" + (items.Count - 3) + " more",
                        Location = new Point(8, top),
                        Size = new Size(78, 20),
                        Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(65, 85, 100),
                        BackColor = Color.Transparent
                    };
                    more.Click += DayCell_Click;
                    more.Tag = day;
                    cell.Controls.Add(more);
                }
            }

            return cell;
        }

        private static Label BuildAppointmentChip(AppointmentItem item, int top)
        {
            Color color = StatusColor(item.Status);
            string text = string.IsNullOrWhiteSpace(item.Time)
                ? item.Service
                : item.Time + " " + item.Service;

            return new Label
            {
                Text = text,
                Location = new Point(6, top),
                Size = new Size(84, 22),
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 8.2F, FontStyle.Bold),
                ForeColor = color,
                BackColor = Color.FromArgb(250, 253, 255),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 0, 2, 0),
                Cursor = Cursors.Hand
            };
        }

        private void DayCell_Click(object? sender, EventArgs e)
        {
            if (sender is Control control && control.Tag is DateTime date)
            {
                selectedDate = date.Date;
                if (selectedDate.Month != visibleMonth.Month || selectedDate.Year != visibleMonth.Year)
                    visibleMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);

                RenderCalendar();
                RenderAgenda();
            }
        }

        private void RenderAgenda()
        {
            agendaList.SuspendLayout();
            agendaList.Controls.Clear();

            Label selected = new Label
            {
                Text = selectedDate.ToString("dddd, dd MMM yyyy", CultureInfo.InvariantCulture),
                Size = new Size(166, 28),
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 70),
                BackColor = Color.FromArgb(232, 247, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 10)
            };
            agendaList.Controls.Add(selected);

            if (!appointmentsByDate.TryGetValue(selectedDate.Date, out List<AppointmentItem>? items) || items.Count == 0)
            {
                Label empty = new Label
                {
                    Text = "No appointments for this date.",
                    Size = new Size(166, 54),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(100, 108, 116),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                agendaList.Controls.Add(empty);
                agendaList.ResumeLayout();
                return;
            }

            foreach (AppointmentItem item in items)
                agendaList.Controls.Add(BuildAgendaCard(item));

            agendaList.ResumeLayout();
        }

        private static Panel BuildAgendaCard(AppointmentItem item)
        {
            Color color = StatusColor(item.Status);
            Panel card = new Panel
            {
                Size = new Size(166, 82),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 8),
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel stripe = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(5, 82),
                BackColor = color
            };

            Label service = new Label
            {
                Text = item.Service,
                Location = new Point(12, 6),
                Size = new Size(145, 22),
                AutoEllipsis = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = color
            };

            Label details = new Label
            {
                Text = $"{item.Time} | {item.Resident}\r\n{item.Status}",
                Location = new Point(12, 30),
                Size = new Size(145, 44),
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(60, 68, 78)
            };

            card.Controls.Add(stripe);
            card.Controls.Add(service);
            card.Controls.Add(details);
            return card;
        }

        private static DateTime StartOfCalendar(DateTime month)
        {
            DateTime first = new DateTime(month.Year, month.Month, 1);
            int diff = ((int)first.DayOfWeek + 6) % 7;
            return first.AddDays(-diff).Date;
        }

        private static Color StatusColor(string status)
        {
            string key = (status ?? "").Trim().ToLowerInvariant();
            if (key.Contains("confirm") || key.Contains("approve"))
                return Color.FromArgb(18, 135, 92);
            if (key.Contains("cancel") || key.Contains("reject"))
                return Color.FromArgb(220, 55, 90);
            return Color.FromArgb(255, 92, 30);
        }

        private sealed class AppointmentItem
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Time { get; set; } = "";
            public string Resident { get; set; } = "";
            public string Service { get; set; } = "";
            public string AssignedDoctor { get; set; } = "";
            public string Status { get; set; } = "";
        }
    }
}
