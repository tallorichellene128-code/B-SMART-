using System;
using System.Linq;
using System.Windows.Forms;

namespace BSMART
{
    internal static class SettingsNavigationService
    {
        private const string AttachedTag = "BsmartSettingsAttached";

        public static void AttachToOpenForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Settings || form is Settings2)
                    continue;

                string formName = form.GetType().Name;
                if (formName.StartsWith("Mayor", StringComparison.OrdinalIgnoreCase) ||
                    form is TayudLGU ||
                    form is TayudManageHealth ||
                    form is TayudBCViewRes)
                    continue;

                Button button = FindButton(form.Controls, "btnSettings");
                if (button == null || Equals(button.Tag, AttachedTag))
                    continue;

                button.Tag = AttachedTag;
                button.Click += (s, e) => OpenSettings(form);
            }
        }

        public static void OpenSettings(Form current)
        {
            Form settings = IsResident() ? new Settings() : new Settings2();
            BsmartFormNavigator.Open(current, settings);
        }

        public static void OpenDashboard(Form current)
        {
            Form dashboard = CreateDashboard();
            BsmartFormNavigator.Open(current, dashboard);
        }

        public static void OpenLogin(Form current)
        {
            if (MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Form login = CreateLogin();
            Session.Clear();
            BsmartFormNavigator.Open(current, login);
        }

        public static bool IsResident()
        {
            return string.Equals(Session.Role, "Resident", StringComparison.OrdinalIgnoreCase);
        }

        private static Form CreateDashboard()
        {
            if (Session.IsMayor || string.Equals(Session.Role, "Mayor", StringComparison.OrdinalIgnoreCase))
                return new MayorDash();

            if (string.Equals(Session.Role, "Captain", StringComparison.OrdinalIgnoreCase))
                return new TayudBCapDash();

            if (string.Equals(Session.Role, "Resident", StringComparison.OrdinalIgnoreCase))
                return new ResidentDash();

            return new TayudLGU();
        }

        private static Form CreateLogin()
        {
            if (Session.IsMayor || string.Equals(Session.Role, "Mayor", StringComparison.OrdinalIgnoreCase))
                return new loginMayor();

            if (string.Equals(Session.Role, "Captain", StringComparison.OrdinalIgnoreCase))
                return new loginBCap();

            if (string.Equals(Session.Role, "Resident", StringComparison.OrdinalIgnoreCase))
                return new loginRes();

            return new loginLGU();
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
    }
}
