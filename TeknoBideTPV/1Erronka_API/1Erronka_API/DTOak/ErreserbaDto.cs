using System;

namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Erreserbaren datuak transferitzeko objektua.
    /// </summary>
    public class ErreserbaDto
    {
        /// <summary>
        /// Erreserbaren identifikadorea.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Bezeroaren izena.
        /// </summary>
        public string BezeroIzena { get; set; }

        /// <summary>
        /// Bezeroaren telefono zenbakia.
        /// </summary>
        public string Telefonoa { get; set; }

        /// <summary>
        /// Erreserbarako pertsona kopurua.
        /// </summary>
        public int PertsonaKopurua { get; set; }

        /// <summary>
        /// Erreserbaren data eta ordua.
        /// </summary>
        public DateTime EgunaOrdua { get; set; }

        /// <summary>
        /// Erreserbaren prezio totala.
        /// </summary>
        public double PrezioTotala { get; set; }

        /// <summary>
        /// Erreserba ordainduta dagoen edo ez (1: Bai, 0: Ez).
        /// </summary>
        public int Ordainduta { get; set; } = 0;

        /// <summary>
        /// Sortutako fakturaren fitxategi ruta.
        /// </summary>
        public string FakturaRuta { get; set; }

        /// <summary>
        /// Erreserba kudeatu duen langilearen IDa.
        /// </summary>
        public int LangileaId { get; set; }

        /// <summary>
        /// Erreserba kudeatu duen langilearen izena.
        /// </summary>
        public string LangileaIzena { get; set; }

        /// <summary>
        /// Erreserba hau dagokion mahaiaren IDa.
        /// </summary>
        public int MahaiakId { get; set; }
    }

    /// <summary>
    /// Erreserba berri bat sortzeko edo eguneratzeko erabiltzen den DTOa.
    /// </summary>
    public class ErreserbaSortuDto
    {
        /// <summary>
        /// Bezeroaren izena.
        /// </summary>
        public string BezeroIzena { get; set; } = string.Empty;

        /// <summary>
        /// Bezeroaren telefono zenbakia.
        /// </summary>
        public string Telefonoa { get; set; } = string.Empty;

        /// <summary>
        /// Pertsona kopurua.
        /// </summary>
        public int PertsonaKopurua { get; set; }

        /// <summary>
        /// Erreserbaren data eta ordua.
        /// </summary>
        public DateTime EgunaOrdua { get; set; } 

        /// <summary>
        /// Prezio totala (hasieran 0 izan daiteke).
        /// </summary>
        public double PrezioTotala { get; set; }

        /// <summary>
        /// Fakturaren ruta (aukerakoa).
        /// </summary>
        public string FakturaRuta { get; set; } = string.Empty;

        /// <summary>
        /// Langilearen IDa.
        /// </summary>
        public int LangileaId { get; set; }

        /// <summary>
        /// Mahaiaren IDa.
        /// </summary>
        public int MahaiakId { get; set; }
    }
}
