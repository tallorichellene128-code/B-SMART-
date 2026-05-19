using System;
using System.Drawing;
using System.Windows.Forms;

namespace BSMART
{
    internal static class ResidentProfileUiService
    {
        public static void Apply(
            Panel panel,
            Label nameLabel,
            Label ageLabel,
            Label addressLabel,
            Label barangayLabel,
            Label emailLabel)
        {
            if (panel == null) return;

            panel.Padding = new Padding(0);

            Font profileFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            ConfigureLabel(nameLabel, new Point(48, 24), new Size(430, 42), profileFont, Color.Black);
            ConfigureLabel(ageLabel, new Point(48, 78), new Size(360, 42), profileFont, Color.Black);
            ConfigureLabel(addressLabel, new Point(48, 132), new Size(430, 48), profileFont, Color.Black);
            ConfigureLabel(barangayLabel, new Point(540, 78), new Size(430, 42), profileFont, Color.Black);
            ConfigureLabel(emailLabel, new Point(540, 132), new Size(430, 48), profileFont, Color.Black);
        }

        public static void SetValues(
            Label nameLabel,
            Label ageLabel,
            Label addressLabel,
            Label barangayLabel,
            Label emailLabel,
            string name,
            string age,
            string gender,
            string birthday,
            string address,
            string barangay,
            string email,
            string mobile)
        {
            nameLabel.Text = Clean(name, "Resident");
            ageLabel.Text = Clean(age);
            addressLabel.Text = Clean(address);
            barangayLabel.Text = Clean(barangay);
            emailLabel.Text = Clean(email);
        }

        public static string Value(object value, string fallback = "No record")
        {
            if (value == null || value == DBNull.Value) return fallback;
            string text = Convert.ToString(value)?.Trim() ?? "";
            return string.IsNullOrWhiteSpace(text) ? fallback : text;
        }

        public static string DateValue(object value)
        {
            if (value == null || value == DBNull.Value) return "No record";
            if (DateTime.TryParse(Convert.ToString(value), out DateTime date))
                return date.ToString("MMM d, yyyy");
            return Value(value);
        }

        private static string Clean(string value, string fallback = "No record")
        {
            return string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        }

        private static void ConfigureLabel(Label label, Point location, Size size, Font font, Color color)
        {
            label.AutoSize = false;
            label.Location = location;
            label.Size = size;
            label.Font = font;
            label.ForeColor = color;
            label.BackColor = Color.Transparent;
            label.AutoEllipsis = true;
        }
    }
}
