using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BSMART
{
    public class RoundedBtn : Button
    {
        public int CornerRadius { get; set; } = 6;
        private bool _hover;

        public RoundedBtn()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            int d = CornerRadius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            using var brush = new SolidBrush(_hover ? ControlPaint.Dark(BackColor, 0.05f) : BackColor);
            e.Graphics.FillPath(brush, path);

            var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            using var tb = new SolidBrush(ForeColor);
            e.Graphics.DrawString(Text, Font, tb, ClientRectangle, sf);
        }
    }

    public class NotificationData
    {
        public string Initial { get; set; }
        public Color AvatarColor { get; set; }
        public string Message { get; set; }
        public string TimeAgo { get; set; }
        public bool IsUnread { get; set; }
    }

    public class NotifItem : Panel
    {
        public NotifItem(NotificationData data)
        {
            BackColor = data.IsUnread ? Color.FromArgb(225, 240, 255) : Color.White;
            Height = 80;
            Dock = DockStyle.Top;

            if (data.IsUnread)
            {
                var dot = new Panel { Size = new Size(10, 10), BackColor = Color.Transparent };
                dot.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(0, 120, 215)), 0, 0, 9, 9);
                };
                Controls.Add(dot);
                dot.Location = new Point(Width - 18, 12);
                Resize += (s, e) => dot.Location = new Point(Width - 18, 12);
            }

            var av = new Panel { Size = new Size(42, 42), Location = new Point(10, 18) };
            av.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(data.AvatarColor);
                e.Graphics.FillEllipse(b, 0, 0, 41, 41);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(
                    data.Initial,
                    new Font("Segoe UI", 14, FontStyle.Bold),
                    Brushes.White,
                    new Rectangle(0, 0, 41, 41), sf);
            };
            Controls.Add(av);

            var msg = new Label
            {
                Text = data.Message,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(62, 12),
                Size = new Size(Width - 82, 38),
                AutoSize = false
            };
            Controls.Add(msg);
            Resize += (s, e) => msg.Width = Width - 82;

            var time = new Label
            {
                Text = data.TimeAgo,
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Location = new Point(62, 52),
                AutoSize = true
            };
            Controls.Add(time);

            Controls.Add(new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(210, 210, 210)
            });
        }
    }

    public class NotificationDropdown : Panel
    {
        private Panel _scrollPanel;
        private Form _owner;
        private readonly Func<List<NotificationData>> _loader;

        public NotificationDropdown(Form owner, Func<List<NotificationData>> loader)
        {
            _owner = owner;
            _loader = loader;

            Size = new Size(360, 420);
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;

            owner.Controls.Add(this);
            BringToFront();
            BuildShell();
        }

        private void BuildShell()
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(0, 102, 204)
            };

            var title = new Label
            {
                Text = "🔔  Notifications",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            header.Controls.Add(title);
            Controls.Add(header);

            _scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(0, 4, 0, 4)
            };
            Controls.Add(_scrollPanel);
        }

        private void Populate()
        {
            _scrollPanel.Controls.Clear();

            List<NotificationData> items;
            try { items = _loader(); }
            catch { items = new List<NotificationData>(); }

            if (items == null || items.Count == 0)
            {
                _scrollPanel.Controls.Add(new Label
                {
                    Text = "No notifications.",
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Top,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                return;
            }

            bool shownNew = false;
            bool shownEarlier = false;

            var controls = new List<Control>();

            foreach (var d in items)
            {
                if (d.IsUnread && !shownNew)
                {
                    controls.Add(MakeSectionLabel("New"));
                    shownNew = true;
                }
                else if (!d.IsUnread && !shownEarlier)
                {
                    controls.Add(MakeSectionLabel("Earlier"));
                    shownEarlier = true;
                }
                controls.Add(new NotifItem(d));
            }

            controls.Reverse();
            foreach (var c in controls)
                _scrollPanel.Controls.Add(c);
        }

        private Label MakeSectionLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(100, 100, 100),
            Height = 28,
            Dock = DockStyle.Top,
            Padding = new Padding(12, 6, 0, 0),
            BackColor = Color.FromArgb(245, 245, 245)
        };

        public void ShowBelow(Control button)
        {
            Populate();
            var pt = _owner.PointToClient(
                button.Parent.PointToScreen(
                    new Point(button.Right - Width, button.Bottom + 4)));
            Location = pt;
            BringToFront();
            Visible = true;
        }
    }
}