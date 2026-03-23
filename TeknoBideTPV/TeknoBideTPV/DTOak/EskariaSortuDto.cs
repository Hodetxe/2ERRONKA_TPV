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
        public string Egoera { get; set; } = string.Empty;
        public List<EskariaProduktuaSortuDto> Produktuak { get; set; } = new();
    }
}
