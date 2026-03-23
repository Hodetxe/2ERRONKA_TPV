using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class ErreserbaSortuDto
    {
        public string BezeroIzena { get; set; } = string.Empty;
        public string Telefonoa { get; set; } = string.Empty;
        public int PertsonaKopurua { get; set; }
        public DateTime EgunaOrdua { get; set; }
        public double PrezioTotala { get; set; } = 0;
        public string FakturaRuta { get; set; } = string.Empty;
        public int LangileaId { get; set; }
        public int MahaiakId { get; set; }
    }

}
