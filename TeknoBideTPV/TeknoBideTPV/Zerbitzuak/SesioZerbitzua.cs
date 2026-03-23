using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.Zerbitzuak
{
    internal class SesioZerbitzua
    {
        public static int LangileaId { get; set; }
        public static string Izena { get; set; }
        public static bool Gerentea { get; set; }

        public static void Logout()
        {
            LangileaId = 0;
            Izena = "";
            Gerentea = false;
        }
    }
}
