namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Produktu baten informazioa gordetzeko DTOa.
    /// </summary>
    public class ProduktuaDto
    {
        /// <summary>
        /// Produktuaren identifikadorea.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Produktuaren izena.
        /// </summary>
        public string Izena { get; set; } = string.Empty;

        /// <summary>
        /// Produktuaren prezioa.
        /// </summary>
        public double Prezioa { get; set; }

        /// <summary>
        /// Produktu mota (Adibidez: "Edaria", "Platera").
        /// </summary>
        public string Mota { get; set; } = string.Empty;

        /// <summary>
        /// Produktuaren stock kopurua.
        /// </summary>
        public int Stock { get; set; }
    }

    /// <summary>
    /// Osagai baten informazioa gordetzeko DTOa.
    /// </summary>
    public class OsagaiaDto
    {
        /// <summary>
        /// Osagaiaren identifikadorea.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Osagaiaren izena.
        /// </summary>
        public string Izena { get; set; } = string.Empty;

        /// <summary>
        /// Osagaiaren prezioa.
        /// </summary>
        public double Prezioa { get; set; }

        /// <summary>
        /// Osagaiaren stock kopurua.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Osagaiaren hornitzailearen IDa.
        /// </summary>
        public int HornitzaileakId { get; set; }
    }
}
