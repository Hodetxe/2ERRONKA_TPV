namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Erreserba bat ordaintzeko erabiltzen den datu egitura.
    /// </summary>
    public class ErreserbaOrdainduDto
    {
        /// <summary>
        /// Ordaindu nahi den erreserbaren IDa.
        /// </summary>
        public int ErreserbaId { get; set; }

        /// <summary>
        /// Ordaindu beharreko guztizko kopurua.
        /// </summary>
        public double Guztira { get; set; }

        /// <summary>
        /// Bezeroak emandako diru kopurua (eskudirutan bada).
        /// </summary>
        public double Jasotakoa { get; set; }

        /// <summary>
        /// Bezeroari itzuli beharreko diru kopurua.
        /// </summary>
        public double Itzulia { get; set; }

        /// <summary>
        /// Ordainketa kudeatzen duen langilearen IDa.
        /// </summary>
        public int LangileaId { get; set; }

        /// <summary>
        /// Ordainketa modua (Adibidez: "Eskudirua", "Txartela").
        /// </summary>
        public string OrdainketaModua { get; set; } = string.Empty;
    }
}
