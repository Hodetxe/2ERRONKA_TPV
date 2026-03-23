namespace TeknoBideTPV.DTOak
{
    public class ErreserbaDto
    {
        public int Id { get; set; }
        public string BezeroIzena { get; set; } = string.Empty;
        public string Telefonoa { get; set; } = string.Empty;
        public int PertsonaKopurua { get; set; }
        public DateTime EgunaOrdua { get; set; } 
        public string Eguna => EgunaOrdua.ToString("dd/MM/yyyy");
        public string Ordua => EgunaOrdua.ToString("HH:mm");

        public double PrezioTotala { get; set; }
        public int Ordainduta { get; set; }   

        public string FakturaRuta { get; set; } = string.Empty;
        public int LangileaId { get; set; }
        public string LangileaIzena { get; set; } = string.Empty;
        public int MahaiakId { get; set; }

        public int MahaiaZenbakia { get; set; }

        public string OrdaindutaTestua => Ordainduta == 1 ? "Bai" : "Ez";
    }
}
