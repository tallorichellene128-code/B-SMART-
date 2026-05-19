using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMART
{
    public static class RegisterData
    {
        public static string FirstName { get; set; }
        public static string MiddleName { get; set; }
        public static string LastName { get; set; }
        public static string Birthday { get; set; }
        public static int Age { get; set; }
        public static string Gender { get; set; }
        public static string PlaceOfBirth { get; set; }
        public static string CivilStatus { get; set; }
        public static string Religion { get; set; }
        public static string Citizenship { get; set; }
        public static string Address { get; set; }
        public static string Email { get; set; }
        public static string MobileNumber { get; set; }
        public static int BarangayID { get; set; }
        public static string BarangayName { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }

        public static void Clear()
        {
            FirstName = MiddleName = LastName = Birthday = Gender = "";
            PlaceOfBirth = CivilStatus = Religion = Citizenship = "";
            Address = Email = MobileNumber = BarangayName = Username = Password = "";
            Age = BarangayID = 0;
        }
    }
}
