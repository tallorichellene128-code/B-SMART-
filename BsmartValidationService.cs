using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BSMART
{
    internal static class BsmartValidationService
    {
        public static bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            try
            {
                _ = new MailAddress(value.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhilippineMobile(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string digits = Regex.Replace(value, @"\D", "");
            return Regex.IsMatch(digits, @"^(09\d{9}|639\d{9})$");
        }

        public static bool IsValidAge(int age)
        {
            return age >= 0 && age <= 120;
        }

        public static bool IsPastOrToday(DateTime date)
        {
            return date.Date <= DateTime.Today;
        }

        public static bool IsFutureOrToday(DateTime date)
        {
            return date.Date >= DateTime.Today;
        }
    }
}
