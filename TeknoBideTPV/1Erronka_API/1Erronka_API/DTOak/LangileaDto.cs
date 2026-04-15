namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Langilearen datuak transferitzeko objektua.
    /// </summary>
    public class LangileaDto
    {
        /// <summary>
        /// Langilearen identifikadorea.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Langilearen izena.
        /// </summary>
        public virtual string Izena { get; set; } = string.Empty;

        public virtual string Abizena { get; set; } = string.Empty;

        /// <summary>
        /// Langilearen erabiltzaile izena sisteman.
        /// </summary>
        public virtual string Erabiltzaile_izena { get; set; } = string.Empty;

        /// <summary>
        /// Langilearen kodea.
        /// </summary>
        public int Langile_kodea { get; set; }

        /// <summary>
        /// Langilearen rola (1: administratzailea, 2: zerbitzaria).
        /// </summary>
        public int Rola_id { get; set; }

        /// <summary>
        /// Langilea administratzailea den edo ez (rola_id == 1).
        /// </summary>
        public bool Gerentea { get; set; }
    }
}
