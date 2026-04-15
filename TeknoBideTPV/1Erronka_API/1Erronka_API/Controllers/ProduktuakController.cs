using Microsoft.AspNetCore.Mvc;
using _1Erronka_API.Repositorioak;
using _1Erronka_API.DTOak;

namespace _1Erronka_API.Controllers
{
    /// <summary>
    /// Produktuak kudeatzeko kontroladorea.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProduktuakController : ControllerBase
    {
        private readonly ProduktuaRepository _repo;

        public ProduktuakController(ProduktuaRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Produktu guztiak lortzen ditu.
        /// </summary>
        /// <returns>Produktu guztien zerrenda DTO formatuan.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var produktuak = _repo.GetAll();

            var dtoList = produktuak.Select(p => new ProduktuaDto
            {
                Id = p.Id,
                Izena = p.Izena,
                Prezioa = p.Prezioa,
                Mota = p.Mota,
                Stock = p.Stock
            }).ToList();

            return Ok(dtoList);
        }

        /// <summary>
        /// Produktu zehatz bat lortzen du bere IDaren bidez.
        /// </summary>
        /// <param name="id">Produktuaren identifikadorea.</param>
        /// <returns>Produktuaren datuak edo NotFound mezua.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var produktua = _repo.Get(id);
            if (produktua == null) return NotFound();

            var dto = new ProduktuaDto
            {
                Id = produktua.Id,
                Izena = produktua.Izena,
                Prezioa = produktua.Prezioa,
                Mota = produktua.Mota,
                Stock = produktua.Stock
            };

            return Ok(dto);
        }
    }
}
