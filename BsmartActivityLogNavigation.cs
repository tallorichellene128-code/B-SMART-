using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BSMART
{
    internal static class BsmartActivityLogNavigation
    {
        private const string ButtonName = "btnActivityLog";

        public static void Attach(Form form)
        {
            if (form == null || form.IsDisposed) return;
            if (!CanShowActivityLog(form)) return;

            Button? existing = FindControls<Button>(form)
                .FirstOrDefault(b => string.Equals(b.Name, ButtonName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals((b.Text ?? string.Empty).Trim(), "Activity Log", StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                StyleAsIconButton(existing);
                existing.Click -= ActivityLog_Click;
                existing.Click += ActivityLog_Click;
                return;
            }

            Button? notification = FindControls<Button>(form)
                .FirstOrDefault(b => string.Equals(b.Name, "btnNotification", StringComparison.OrdinalIgnoreCase));
            Button? settings = FindControls<Button>(form)
                .FirstOrDefault(b => string.Equals(b.Name, "btnSettings", StringComparison.OrdinalIgnoreCase));
            Button? anchor = notification ?? settings;

            int width = anchor?.Width > 0 ? anchor.Width : 68;
            int height = anchor?.Height > 0 ? anchor.Height : 48;
            int top = anchor?.Top ?? 12;
            int left = anchor != null ? anchor.Left - width - 10 : Math.Max(270, form.ClientSize.Width - 360);
            if (left < 270) left = Math.Max(270, form.ClientSize.Width - width - 250);

            Button activity = new Button
            {
                Name = ButtonName,
                Text = "",
                Size = new Size(width, height),
                Location = new Point(left, top),
            };
            StyleAsIconButton(activity);
            activity.Click += ActivityLog_Click;

            form.Controls.Add(activity);
            activity.BringToFront();
        }

        private static void StyleAsIconButton(Button button)
        {
            button.Text = "";
            button.BackColor = Color.SkyBlue;
            button.BackgroundImage = Properties.Resources.folder__1_;
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.FlatStyle = FlatStyle.Popup;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
        }

        private static bool CanShowActivityLog(Form form)
        {
            if (!(Session.IsMayor || string.Equals(Session.Role, "Captain", StringComparison.OrdinalIgnoreCase)))
                return false;

            string formName = form.GetType().Name;
            if (string.Equals(formName, nameof(Settings2), StringComparison.OrdinalIgnoreCase)
                || string.Equals(formName, nameof(BsmartAuditLogForm), StringComparison.OrdinalIgnoreCase))
                return false;

            return formName.StartsWith("Mayor", StringComparison.OrdinalIgnoreCase)
                || formName.StartsWith("TayudBC", StringComparison.OrdinalIgnoreCase);
        }

        private static void ActivityLog_Click(object? sender, EventArgs e)
        {
            BsmartAuditLogForm? openLog = Application.OpenForms.OfType<BsmartAuditLogForm>().FirstOrDefault();
            if (openLog != null)
            {
                openLog.Activate();
                return;
            }

            Form? owner = (sender as Control)?.FindForm();
            BsmartAuditLogForm log = new BsmartAuditLogForm();
            if (owner == null)
                log.Show();
            else
                log.Show(owner);
        }

        private static System.Collections.Generic.IEnumerable<T> FindControls<T>(Control root) where T : Control
        {
            foreach (Control child in root.Controls)
            {
                if (child is T match)
                    yield return match;

                foreach (T nested in FindControls<T>(child))
                    yield return nested;
            }
        }
    }
}
