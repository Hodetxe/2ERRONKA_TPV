namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Mahaiaren informazioa gordetzeko DTOa.
    /// </summary>
    public class MahaiaDto
    {
        /// <summary>
        /// Mahaiaren identifikadorea.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mahaiaren zenbakia.
        /// </summary>
        public int Zenbakia { get; set; }

        /// <summary>
        /// Mahaian sar daitekeen pertsona kopurua.
        /// </summary>
        public int PertsonaKopurua { get; set; }

        /// <summary>
        /// Mahaiaren kokapena (Adibidez: "Terraza", "Barruan").
        /// </summary>
        public string Kokapena { get; set; } = string.Empty;
    }
}
