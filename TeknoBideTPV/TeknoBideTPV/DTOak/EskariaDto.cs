using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class EskariaDto
    {
        public int Id { get; set; }
        public double Prezioa { get; set; }
        public double GuztiraBruto { get; set; }
        public double DeskontuKopurua { get; set; }
        public string? DeskontuKodea { get; set; }
        public string? DeskontuMota { get; set; }
        public double? DeskontuBalioa { get; set; }
        public string Egoera { get; set; }

        public int ErreserbaId { get; set; }

        public int MahaiaZenbakia { get; set; }

        public List<EskariaProduktuaDto> Produktuak { get; set; }
    }

    public class EskariaProduktuaDto
    {
        public int ProduktuaId { get; set; }
        public string ProduktuaIzena { get; set; }
        public int Kantitatea { get; set; }
        public double Prezioa { get; set; }
    }

}

