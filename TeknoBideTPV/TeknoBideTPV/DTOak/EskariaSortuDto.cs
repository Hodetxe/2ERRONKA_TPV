using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class EskariaSortuDto
    {
        public int ErreserbaId { get; set; }
        public double Prezioa { get; set; }
        public double GuztiraBruto { get; set; }
        public double DeskontuKopurua { get; set; }
        public string? DeskontuKodea { get; set; }
        public string? DeskontuMota { get; set; }
        public double? DeskontuBalioa { get; set; }
        public string Egoera { get; set; } = string.Empty;
        public List<EskariaProduktuaSortuDto> Produktuak { get; set; } = new();
    }
}
