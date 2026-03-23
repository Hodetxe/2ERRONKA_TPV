

namespace TeknoBideTPV.DTOak
{   
    public class ProduktuaDto
    {
        public int Id { get; set; }
        public string Izena { get; set; } = string.Empty;
        public double Prezioa { get; set; }
        public string Mota { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
