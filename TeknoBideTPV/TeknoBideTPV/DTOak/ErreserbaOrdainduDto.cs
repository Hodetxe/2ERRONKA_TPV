using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class ErreserbaOrdainduDto
    {
        public int ErreserbaId { get; set; }
        public double Guztira { get; set; }
        public double Jasotakoa { get; set; }
        public double Itzulia { get; set; }
        public int LangileaId { get; set; }
        public string OrdainketaModua { get; set; } = string.Empty;
    }
}
