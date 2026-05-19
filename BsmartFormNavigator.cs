using System;
using System.Windows.Forms;

namespace BSMART
{
    internal static class BsmartFormNavigator
    {
        public static void Open(Form current, Form next)
        {
            if (current == null)
            {
                ShowPrepared(next);
                return;
            }

            if (next.GetType() == current.GetType())
            {
                next.Dispose();
                current.Activate();
                return;
            }

            current.SuspendLayout();
            ShowPrepared(next);
            current.Hide();
            current.ResumeLayout(false);
        }

        public static void OpenLogin(Form current, Form login)
        {
            Session.Clear();
            Open(current, login);
        }

        private static void ShowPrepared(Form form)
        {
            form.SuspendLayout();
            BsmartUiService.PrepareForm(form);
            form.ResumeLayout(false);
            form.Show();
        }
    }
}
