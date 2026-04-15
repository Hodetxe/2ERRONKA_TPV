using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using NHibernate.Linq;
using Xunit;
using _1Erronka_API.Controllers;
using _1Erronka_API.Domain;
using _1Erronka_API.DTOak;

namespace _1Erronka_API.Testak
{
    public class LoginControllerTest
    {
        private string HashPassword(string input)
        {
            using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        [Fact]
        public void Login_LangileaEzDaExistitzen_NotFound()
        {
            // Arrange
            var sessionMock = new Mock<NHibernate.ISession>();
            var langileak = new List<Langilea>(); // empty list
            sessionMock.Setup(s => s.Query<Langilea>()).Returns(langileak.AsQueryable());

            var controller = new LoginController(sessionMock.Object);
            var request = new LoginRequest { Langile_kodea = 1, Pasahitza = "password" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginErantzuna>(okResult.Value);
            Assert.False(response.Ok);
            Assert.Equal("not_found", response.Code);
            Assert.Equal("Langilea ez da existitzen", response.Message);
        }

        [Fact]
        public void Login_PasahitzaOkerra_BadPassword()
        {
            // Arrange
            var sessionMock = new Mock<NHibernate.ISession>();
            var langileak = new List<Langilea>
            {
                new Langilea
                {
                    Id = 1,
                    Langile_kodea = 1,
                    Pasahitza = HashPassword("correctpassword"), // different from request
                    Rola_id = 2,
                    Ezabatua = false
                }
            };
            sessionMock.Setup(s => s.Query<Langilea>()).Returns(langileak.AsQueryable());

            var controller = new LoginController(sessionMock.Object);
            var request = new LoginRequest { Langile_kodea = 1, Pasahitza = "wrongpassword" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginErantzuna>(okResult.Value);
            Assert.False(response.Ok);
            Assert.Equal("bad_password", response.Code);
            Assert.Equal("Pasahitza okerra da", response.Message);
        }

        [Fact]
        public void Login_Ezabatuta_Disbled()
        {
            // Arrange
            var sessionMock = new Mock<NHibernate.ISession>();
            var langileak = new List<Langilea>
            {
                new Langilea
                {
                    Id = 1,
                    Langile_kodea = 1,
                    Pasahitza = HashPassword("password"),
                    Rola_id = 2,
                    Ezabatua = true
                }
            };
            sessionMock.Setup(s => s.Query<Langilea>()).Returns(langileak.AsQueryable());

            var controller = new LoginController(sessionMock.Object);
            var request = new LoginRequest { Langile_kodea = 1, Pasahitza = "password" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginErantzuna>(okResult.Value);
            Assert.False(response.Ok);
            Assert.Equal("disabled", response.Code);
            Assert.Equal("Langilea ezabatuta dago", response.Message);
        }

        [Fact]
        public void Login_EzDaukaTPVBaimenik_Forbidden()
        {
            // Arrange
            var sessionMock = new Mock<NHibernate.ISession>();
            var langileak = new List<Langilea>
            {
                new Langilea
                {
                    Id = 1,
                    Langile_kodea = 1,
                    Pasahitza = HashPassword("password"),
                    Rola_id = 3,
                    Ezabatua = false
                }
            };
            sessionMock.Setup(s => s.Query<Langilea>()).Returns(langileak.AsQueryable());

            var controller = new LoginController(sessionMock.Object);
            var request = new LoginRequest { Langile_kodea = 1, Pasahitza = "password" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginErantzuna>(okResult.Value);
            Assert.False(response.Ok);
            Assert.Equal("forbidden", response.Code);
            Assert.Equal("Ez daukazu TPV-ra sartzeko baimenik", response.Message);
        }

        [Fact]
        public void Login_Zuzena_Success()
        {
            // Arrange
            var sessionMock = new Mock<NHibernate.ISession>();
            var langilea = new Langilea
            {
                Id = 1,
                Izena = "Test Langilea",
                Abizena = "Test Abizena",
                Erabiltzaile_izena = "testuser",
                Langile_kodea = 1,
                Pasahitza = HashPassword("password"),
                Rola_id = 2,
                Ezabatua = false
            };
            var langileak = new List<Langilea> { langilea };
            sessionMock.Setup(s => s.Query<Langilea>()).Returns(langileak.AsQueryable());

            var controller = new LoginController(sessionMock.Object);
            var request = new LoginRequest { Langile_kodea = 1, Pasahitza = "password" };

            // Act
            var result = controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginErantzuna>(okResult.Value);
            Assert.True(response.Ok);
            Assert.Equal("ok", response.Code);
            Assert.Equal("Login zuzena", response.Message);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.Id);
            Assert.Equal("Test Langilea", response.Data.Izena);
            Assert.Equal("testuser", response.Data.Erabiltzaile_izena);
            Assert.Equal(1, response.Data.Langile_kodea);
            Assert.False(response.Data.Gerentea);
            Assert.Equal(2, response.Data.Rola_id);
        }
    }
}
