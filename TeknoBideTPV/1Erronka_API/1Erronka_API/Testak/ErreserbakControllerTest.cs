using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;
using _1Erronka_API.Controllers;
using _1Erronka_API.Domain;
using _1Erronka_API.DTOak;
using _1Erronka_API.Modeloak;

namespace _1Erronka_API.Testak
{
    public class ErreserbakControllerTests
    {
        private readonly ISessionFactory _dummyFactory;

        public ErreserbakControllerTests()
        {
            var factoryMock = new Mock<ISessionFactory>();
            factoryMock.Setup(f => f.GetCurrentSession()).Returns(new Mock<NHibernate.ISession>().Object);
            _dummyFactory = factoryMock.Object;
        }

        private ErreserbakController CreateController(
            Mock<_1Erronka_API.Repositorioak.ErreserbaRepository> repoMock,
            Mock<_1Erronka_API.Repositorioak.EskariaRepository>? eskMock = null,
            Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>? prodMock = null,
            Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>? poMock = null,
            Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>? osaMock = null)
        {
            eskMock ??= new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Loose, _dummyFactory);
            prodMock ??= new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Loose, _dummyFactory);
            poMock ??= new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Loose, _dummyFactory);
            osaMock ??= new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Loose, _dummyFactory);
            return new ErreserbakController(repoMock.Object, eskMock.Object, prodMock.Object, poMock.Object, osaMock.Object);
        }

        [Fact]
        public void GetAll_ErreserbakDaudenean_ListaItzultzenDu()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            var sample = new Erreserba { Id = 1, BezeroIzena = "A", Langilea = new Langilea { Id = 1, Izena = "L" }, Mahaia = new Mahaia { Id = 1 } };
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba> { sample });
            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<ErreserbaDto>>(okResult.Value);
            Assert.Single(list);
            Assert.Equal("A", list[0].BezeroIzena);
        }

        [Fact]
        public void GetAll_ErreserbarikEzDagoenean_ListaHutsaItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba>());
            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<ErreserbaDto>>(okResult.Value);
            Assert.Empty(list);
        }

        [Fact]
        public void Sortu_DtoBaliozkoaDenenean_ErreserbaSortuEtaOkItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            Erreserba captured = null!;
            repoMock.Setup(r => r.Add(It.IsAny<Erreserba>())).Callback<Erreserba>(e => captured = e);
            var controller = CreateController(repoMock);
            var dto = new ErreserbaSortuDto { BezeroIzena = "X", LangileaId = 1, MahaiakId = 1 };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic body = okResult.Value!;
            Assert.Equal("Erreserba sortuta", (string)body.mezua);
            Assert.Equal(captured.Id, (int)body.erreserbaId);
            Assert.Equal("X", captured.BezeroIzena);
        }

        [Fact]
        public void Update_ErreserbaExistitzenDenenean_EguneratuEtaOkItzuli()
        {
            // Arrange
            var sample = new Erreserba { Id = 5, BezeroIzena = "old" };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(5)).Returns(sample);
            repoMock.Setup(r => r.Update(It.IsAny<Erreserba>()));
            var controller = CreateController(repoMock);
            var dto = new ErreserbaSortuDto { BezeroIzena = "new", MahaiakId = 2 };

            // Act
            var result = controller.Update(5, dto);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal("new", sample.BezeroIzena);
            repoMock.Verify(r => r.Update(sample), Times.Once);
        }

        [Fact]
        public void Update_ErreserbaExistitzenEzDenenean_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Erreserba?)null);
            var controller = CreateController(repoMock);
            var dto = new ErreserbaSortuDto();

            // Act
            var result = controller.Update(42, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Ordaindu_ErreserbaExistitzenDenenean_FakturaSortuEtaOkItzuli()
        {
            // Arrange
            var sample = new Erreserba { Id = 10, Ordainduta = 0, Langilea = new Langilea { Izena = "L" } };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);

            var sessionMock = new Mock<NHibernate.ISession>();
            sessionMock.Setup(s => s.Get<Erreserba>(10)).Returns(sample);
            sessionMock.Setup(s => s.BeginTransaction()).Returns(new Mock<NHibernate.ITransaction>().Object);
            sessionMock.Setup(s => s.Update(It.IsAny<object>()));

            var queryMock = new Mock<IQuery>();
            queryMock.Setup(q => q.SetInt32(It.IsAny<string>(), It.IsAny<int>())).Returns(queryMock.Object);
            queryMock.Setup(q => q.ExecuteUpdate()).Returns(0);
            sessionMock.Setup(s => s.CreateQuery(It.IsAny<string>())).Returns(queryMock.Object);

            repoMock.Setup(r => r.OpenSession()).Returns(sessionMock.Object);
            repoMock.Setup(r => r.LortuProduktuakErreserbarako(10)).Returns(new List<EskariaProduktuaDto>());

            var controller = CreateController(repoMock);
            var dto = new ErreserbaOrdainduDto { ErreserbaId = 10, Jasotakoa = 100, Itzulia = 0, Guztira = 50, OrdainketaModua = "Txartela" };

            // Act
            var result = controller.Ordaindu(dto);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(1, sample.Ordainduta);
            Assert.NotNull(sample.FakturaRuta);
        }

        [Fact]
        public void Ordaindu_ErreserbaExistitzenEzDenenean_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            var sessionMock = new Mock<NHibernate.ISession>();
            sessionMock.Setup(s => s.Get<Erreserba>(It.IsAny<int>())).Returns((Erreserba?)null!);
            sessionMock.Setup(s => s.BeginTransaction()).Returns(new Mock<NHibernate.ITransaction>().Object);
            repoMock.Setup(r => r.OpenSession()).Returns(sessionMock.Object);

            var controller = CreateController(repoMock);
            var dto = new ErreserbaOrdainduDto { ErreserbaId = 999 };

            // Act
            var result = controller.Ordaindu(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void SortuTicketPdf_RendererNullDenenean_DefaultErabili()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            var controller = CreateController(repoMock);
            var method = typeof(ErreserbakController).GetMethod("SortuTicketPdf", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var erres = new Erreserba { Id = 1, Langilea = new Langilea { Izena = "L" } };

            // Act
            string ruta = (string)method.Invoke(controller, new object[] { erres, new List<EskariaProduktuaDto>(), 0.0, 0.0, "" })!;

            // Assert
            Assert.Contains("/tiketak/", ruta);
        }

        [Fact]
        public void SortuTicketPdf_RendererBaliozkoaDenenean_Kalkulatu()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Loose, _dummyFactory);
            var controller = CreateController(repoMock);
            var method = typeof(ErreserbakController).GetMethod("SortuTicketPdf", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var erres = new Erreserba { Id = 2, Langilea = new Langilea { Izena = "L" } };

            // Act
            string ruta = (string)method.Invoke(controller, new object[] { erres, new List<EskariaProduktuaDto> { new EskariaProduktuaDto { ProduktuaIzena = "P", Kantitatea = 1, Prezioa = 1 } }, 0.0, 0.0, "Eskudirua" })!;

            // Assert
            Assert.Contains("/tiketak/", ruta);
        }

        [Fact]
        public void GehituTicketEdukia_OrdainketaEskudiruaDenenean_JasotakoaEtaItzulitakoaInprimatu()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Loose, _dummyFactory);
            var controller = CreateController(repoMock);
            var method = typeof(ErreserbakController).GetMethod("GehituTicketEdukia", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var erres = new Erreserba { Id = 3, Langilea = new Langilea { Izena = "L" } };
            using var ms = new MemoryStream();
            using var writer = new iText.Kernel.Pdf.PdfWriter(ms);
            using var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            using var doc = new iText.Layout.Document(pdf);

            // Act
            method.Invoke(controller, new object[] { doc, erres, new List<EskariaProduktuaDto>(), 5.0, 1.0, "Eskudirua" });

            // Assert
            // if no exceptions thrown we consider branch exercised
        }

        [Fact]
        public void GehituTicketEdukia_OrdainketaBesteModuBatDenenean_EzInprimatuJasotakoa()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Loose, _dummyFactory);
            var controller = CreateController(repoMock);
            var method = typeof(ErreserbakController).GetMethod("GehituTicketEdukia", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var erres = new Erreserba { Id = 4, Langilea = new Langilea { Izena = "L" } };
            using var ms = new MemoryStream();
            using var writer = new iText.Kernel.Pdf.PdfWriter(ms);
            using var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            using var doc = new iText.Layout.Document(pdf);

            // Act
            method.Invoke(controller, new object[] { doc, erres, new List<EskariaProduktuaDto>(), 5.0, 1.0, "Txartela" });

            // Assert
            // if no exceptions thrown we consider branch exercised
        }

        [Fact]
        public void Delete_ErreserbaExistitzenDenenean_EzabatuEtaOkItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(7)).Returns(new Erreserba { Id = 7 });
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());
            repoMock.Setup(r => r.Delete(It.IsAny<Erreserba>()));
            var eskMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Loose, _dummyFactory);
            eskMock.Setup(r => r.GetAll()).Returns(new List<Eskaria>());
            var controller = CreateController(repoMock, eskMock);

            // Act
            var result = controller.Delete(7);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }

        [Fact]
        public void Delete_ErreserbaExistitzenEzDenenean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Erreserba?)null);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());
            var pemb = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Loose, _dummyFactory);
            pemb.Setup(r => r.GetAll()).Returns(new List<Eskaria>());
            var controller = CreateController(repoMock, pemb);

            // Act
            var result = controller.Delete(99);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeskargatuTicket_ErreserbaEzDagoenean_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba>());
            var controller = CreateController(repoMock);

            // Act
            var result = controller.DeskargatuTicket(123);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeskargatuTicket_FakturaRutaHutsaDenenean_NotFoundItzuli()
        {
            // Arrange
            var e = new Erreserba { Id = 20, FakturaRuta = "" };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba> { e });
            var controller = CreateController(repoMock);

            // Act
            var result = controller.DeskargatuTicket(20);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeskargatuTicket_PdfFitxategiaEzDagoenean_NotFoundItzuli()
        {
            // Arrange
            var e = new Erreserba { Id = 21, FakturaRuta = "/tiketak/doesnotexist.pdf" };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba> { e });
            var controller = CreateController(repoMock);

            // Act
            var result = controller.DeskargatuTicket(21);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeskargatuTicket_PdfExistitzenDenenean_FileItzuli()
        {
            // Arrange
            var e = new Erreserba { Id = 22, FakturaRuta = "/tiketak/exist.pdf" };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Erreserba> { e });
            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tiketak");
            Directory.CreateDirectory(root);
            var full = Path.Combine(root, "exist.pdf");
            File.WriteAllText(full, "data");
            var controller = CreateController(repoMock);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            // Act
            var result = controller.DeskargatuTicket(22);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);

            // cleanup
            File.Delete(full);
        }
    }
}
