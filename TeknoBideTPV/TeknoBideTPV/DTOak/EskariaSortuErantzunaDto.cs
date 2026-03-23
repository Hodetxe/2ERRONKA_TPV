namespace TeknoBideTPV.DTOak
{
    public class EskariaSortuErantzunaDto
    {
        public int EskariaId { get; set; }
        public double PrezioaTotala { get; set; }
        public List<EskariaProduktuaErantzunaDto> Produktuak { get; set; } = new();
    }

    public class EskariaProduktuaErantzunaDto
    {
        public string ProduktuaIzena { get; set; } = string.Empty;
        public int Kantitatea { get; set; }
        public double ProduktuakPrezioaBakarka { get; set; }
        public double ProduktuakPrezioaGuztira { get; set; }
    }
}
