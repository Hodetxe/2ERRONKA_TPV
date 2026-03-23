using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class MahaiaDto
    {
        public int Id { get; set; }
        public int Zenbakia { get; set; } = 0;
        public int PertsonaKopurua { get; set; }
        public string Kokapena { get; set; } = string.Empty;
    }
}
