using System.Collections.Generic;

namespace _1Erronka_API.DTOak
{
    /// <summary>
    /// Eskariaren informazioa gordetzeko DTOa.
    /// </summary>
    public class EskariaDto
    {
        /// <summary>
        /// Eskariaren identifikadorea.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Eskariaren prezio totala.
        /// </summary>
        public double Prezioa { get; set; }

        /// <summary>
        /// Eskariaren egoera (Adibidez: "Prestatzen", "Zerbitzatuta").
        /// </summary>
        public string Egoera { get; set; } = string.Empty;

        /// <summary>
        /// Eskaria dagokion erreserbaren IDa.
        /// </summary>
        public int ErreserbaId { get; set; }

        /// <summary>
        /// Mahaiaren zenbakia.
        /// </summary>
        public int MahaiaZenbakia { get; set; }

        /// <summary>
        /// Eskarian dauden produktuen zerrenda.
        /// </summary>
        public List<EskariaProduktuaDto> Produktuak { get; set; } = new();
    }

    /// <summary>
    /// Eskaria berri bat sortzeko erabiltzen den DTOa.
    /// </summary>
    public class EskariaSortuDto
    {
        /// <summary>
        /// Erreserbaren IDa.
        /// </summary>
        public int ErreserbaId { get; set; }

        /// <summary>
        /// Eskariaren hasierako prezioa.
        /// </summary>
        public double Prezioa { get; set; }

        /// <summary>
        /// Eskariaren hasierako egoera.
        /// </summary>
        public string Egoera { get; set; } = string.Empty;

        /// <summary>
        /// Eskariari gehituko zaizkion produktuak.
        /// </summary>
        public List<EskariaProduktuaSortuDto> Produktuak { get; set; } = new();
    }

    /// <summary>
    /// Eskaria bateko produktu baten xehetasunak.
    /// </summary>
    public class EskariaProduktuaDto
    {
        /// <summary>
        /// Produktuaren IDa.
        /// </summary>
        public int ProduktuaId { get; set; }

        /// <summary>
        /// Produktuaren izena.
        /// </summary>
        public string ProduktuaIzena { get; set; } = string.Empty;

        /// <summary>
        /// Eskatutako kantitatea.
        /// </summary>
        public int Kantitatea { get; set; }

        /// <summary>
        /// Produktuaren unitateko prezioa.
        /// </summary>
        public double Prezioa { get; set; }
    }

    /// <summary>
    /// Eskaria sortzean produktu bat gehitzeko erabiltzen den DTOa.
    /// </summary>
    public class EskariaProduktuaSortuDto
    {
        /// <summary>
        /// Produktuaren IDa.
        /// </summary>
        public int ProduktuaId { get; set; }

        /// <summary>
        /// Kantitatea.
        /// </summary>
        public int Kantitatea { get; set; }

        /// <summary>
        /// Unitateko prezioa.
        /// </summary>
        public double Prezioa { get; set; }
    }
}
