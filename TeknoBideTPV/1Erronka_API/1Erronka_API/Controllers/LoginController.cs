using _1Erronka_API;
using _1Erronka_API.Domain;
using _1Erronka_API.DTOak;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using NHibernate.Linq;

namespace _1Erronka_API.Controllers

{

/// <summary>
    /// Erabiltzaileen saio-hasiera kudeatzen duen kontroladorea.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly NHibernate.ISession _session;

        public LoginController(NHibernate.ISession session)
        {
            _session = session;
        }
        /// <summary>
        /// Erabiltzailearen saioa hasten du.
        /// </summary>
        /// <param name="request">Saio-hasierako eskaeraren datuak.</param>
        /// <returns>Saio-hasieraren emaitza eta, arrakasta izanez gero, erabiltzailearen datuak.</returns>
               [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try 
            {
                var langilea = _session.Query<Langilea>()
                    .FirstOrDefault(u => u.Langile_kodea == request.Langile_kodea);

                if (langilea == null)
                {
                    return Ok(new LoginErantzuna
                    {
                        Ok = false,
                        Code = "not_found",
                        Message = "Langilea ez da existitzen"
                    });
                }

                if (langilea.Ezabatua)
                {
                    return Ok(new LoginErantzuna
                    {
                        Ok = false,
                        Code = "disabled",
                        Message = "Langilea ezabatuta dago"
                    });
                }

                var pasahitzaHash = HashPassword(request.Pasahitza);
                if (langilea.Pasahitza != request.Pasahitza && langilea.Pasahitza != pasahitzaHash)
                {
                    return Ok(new LoginErantzuna
                    {
                        Ok = false,
                        Code = "bad_password",
                        Message = "Pasahitza okerra da"
                    });
                }

                if (langilea.Rola_id != 1 && langilea.Rola_id != 2)
                {
                    return Ok(new LoginErantzuna
                    {
                        Ok = false,
                        Code = "forbidden",
                        Message = "Ez daukazu TPV-ra sartzeko baimenik"
                    });
                }

                return Ok(new LoginErantzuna
                {
                    Ok = true,
                    Code = "ok",
                    Message = "Login zuzena",
                    Data = new LangileaDto
                    {
                        Id = langilea.Id,
                        Izena = langilea.Izena,
                        Abizena = langilea.Abizena,
                        Erabiltzaile_izena = langilea.Erabiltzaile_izena,
                        Langile_kodea = langilea.Langile_kodea,
                        Rola_id = langilea.Rola_id,
                        Gerentea = langilea.Rola_id == 1
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginErantzuna
                {
                    Ok = false,
                    Code = "internal_error",
                    Message = $"Errorea APIan: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

    private string HashPassword(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }

    /// <summary>
    /// Saio-hasierako eskaeraren datuak.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Langilearen kodea.
        /// </summary>
        public int Langile_kodea { get; set; }

        /// <summary>
        /// Langilearen pasahitza.
        /// </summary>
        public string Pasahitza { get; set; }
    }

    /// <summary>
    /// Saio-hasieraren erantzuna.
    /// </summary>
    public class LoginErantzuna
    {
        /// <summary>
        /// Erantzuna zuzena den edo ez.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// Erantzunaren kodea.
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Erantzunaren mezua.
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Langilearen datuak (aukerakoa).
        /// </summary>
        public LangileaDto? Data { get; set; }
    }

}
