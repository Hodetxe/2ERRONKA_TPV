using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHibernate;
using Xunit;
using _1Erronka_API.Controllers;
using _1Erronka_API.DTOak;
using _1Erronka_API.Modeloak;

namespace _1Erronka_API.Testak
{
    public class MahaiakControllerTest
    {
        private readonly ISessionFactory _dummyFactory;

        public MahaiakControllerTest()
        {
            var factoryMock = new Mock<ISessionFactory>();
            factoryMock.Setup(f => f.GetCurrentSession()).Returns(new Mock<NHibernate.ISession>().Object);
            _dummyFactory = factoryMock.Object;
        }

        private MahaiakController CreateController(Mock<_1Erronka_API.Repositorioak.MahaiaRepository> repoMock)
        {
            return new MahaiakController(repoMock.Object);
        }

        [Fact]
        public void GetAll_MahaiakDaudenean_ListaItzuli()
        {
            // Arrange
            var mahaiak = new List<Mahaia>
            {
                new Mahaia { Id = 1, Zenbakia = 1, PertsonaKopurua = 4, Kokapena = "Barrualdea" },
                new Mahaia { Id = 2, Zenbakia = 2, PertsonaKopurua = 6, Kokapena = "Kanpoaldea" }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(mahaiak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoList = Assert.IsType<List<MahaiaDto>>(okResult.Value);
            Assert.Equal(2, dtoList.Count);
            Assert.Equal(1, dtoList[0].Id);
            Assert.Equal(1, dtoList[0].Zenbakia);
            Assert.Equal(4, dtoList[0].PertsonaKopurua);
            Assert.Equal("Barrualdea", dtoList[0].Kokapena);
            Assert.Equal(2, dtoList[1].Id);
            Assert.Equal(2, dtoList[1].Zenbakia);
            Assert.Equal(6, dtoList[1].PertsonaKopurua);
            Assert.Equal("Kanpoaldea", dtoList[1].Kokapena);
        }

        [Fact]
        public void Create_DatuakZuzenaDirenean_MahaiaSortuEtaOkItzuli()
        {
            // Arrange
            var dto = new MahaiaDto
            {
                Zenbakia = 3,
                PertsonaKopurua = 2,
                Kokapena = "Terasa"
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Add(It.IsAny<Mahaia>()));

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Create(dto);

            // Assert
            Assert.IsType<OkResult>(result);
            repoMock.Verify(r => r.Add(It.Is<Mahaia>(m =>
                m.Zenbakia == 3 &&
                m.PertsonaKopurua == 2 &&
                m.Kokapena == "Terasa")), Times.Once);
        }

        [Fact]
        public void Update_MahaiaExistitzenDa_EguneratuEtaOkItzuli()
        {
            // Arrange
            var mahaia = new Mahaia { Id = 1, Zenbakia = 1, PertsonaKopurua = 4, Kokapena = "Barrualdea" };
            var dto = new MahaiaDto { Zenbakia = 10, PertsonaKopurua = 8, Kokapena = "Kanpoaldea" };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(mahaia);
            repoMock.Setup(r => r.Update(mahaia));

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Update(1, dto);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Equal(10, mahaia.Zenbakia);
            Assert.Equal(8, mahaia.PertsonaKopurua);
            Assert.Equal("Kanpoaldea", mahaia.Kokapena);
            repoMock.Verify(r => r.Update(mahaia), Times.Once);
        }

        [Fact]
        public void Update_MahaiaEzDaExistitzen_NotFoundItzuli()
        {
            // Arrange
            var dto = new MahaiaDto { Zenbakia = 10, PertsonaKopurua = 8, Kokapena = "Kanpoaldea" };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns((Mahaia?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Update(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_MahaiaExistitzenDa_EzabatuEtaOkItzuli()
        {
            // Arrange
            var mahaia = new Mahaia { Id = 1, Zenbakia = 1, PertsonaKopurua = 4, Kokapena = "Barrualdea" };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(mahaia);
            repoMock.Setup(r => r.Delete(mahaia));

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
            repoMock.Verify(r => r.Delete(mahaia), Times.Once);
        }

        [Fact]
        public void Delete_MahaiaEzDaExistitzen_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.MahaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns((Mahaia?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}