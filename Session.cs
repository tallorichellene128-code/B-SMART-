using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMART
{
    public static class Session
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }  // "LGUStaff", "Captain", "Mayor"
        public static int BarangayID { get; set; }  // 0 = Mayor (sees all)
        public static string BarangayName { get; set; } // e.g. "Tayud"

        public static void Clear()
        {
            BarangayID = 0;
            UserID = 0;
            Username = "";
            Role = "";
            BarangayID = 0;
            BarangayName = "";
        }

        // Mayor sees all barangays (BarangayID = 0)
        public static bool IsMayor => Role == "Mayor";
    }
}
