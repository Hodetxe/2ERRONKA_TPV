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
    public class OsagaiakControllerTest
    {
        private readonly ISessionFactory _dummyFactory;

        public OsagaiakControllerTest()
        {
            var factoryMock = new Mock<ISessionFactory>();
            factoryMock.Setup(f => f.GetCurrentSession()).Returns(new Mock<NHibernate.ISession>().Object);
            _dummyFactory = factoryMock.Object;
        }

        private OsagaiakController CreateController(Mock<_1Erronka_API.Repositorioak.OsagaiaRepository> repoMock)
        {
            return new OsagaiakController(repoMock.Object);
        }

        [Fact]
        public void GetAll_OsagaiakDaudenean_ListaItzuli()
        {
            // Arrange
            var osagaiak = new List<Osagaia>
            {
                new Osagaia { Id = 1, Izena = "Tomate", Prezioa = 2.5, Stock = 100, HornitzaileakId = 1 },
                new Osagaia { Id = 2, Izena = "Queso", Prezioa = 5.0, Stock = 50, HornitzaileakId = 2 }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(osagaiak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoList = Assert.IsType<List<OsagaiaDto>>(okResult.Value);
            Assert.Equal(2, dtoList.Count);
            Assert.Equal(1, dtoList[0].Id);
            Assert.Equal("Tomate", dtoList[0].Izena);
            Assert.Equal(2.5, dtoList[0].Prezioa);
            Assert.Equal(100, dtoList[0].Stock);
            Assert.Equal(1, dtoList[0].HornitzaileakId);
            Assert.Equal(2, dtoList[1].Id);
            Assert.Equal("Queso", dtoList[1].Izena);
            Assert.Equal(5.0, dtoList[1].Prezioa);
            Assert.Equal(50, dtoList[1].Stock);
            Assert.Equal(2, dtoList[1].HornitzaileakId);
        }

        [Fact]
        public void Get_OsagaiaExistitzenDa_OsagaiaItzuli()
        {
            // Arrange
            var osagaia = new Osagaia { Id = 1, Izena = "Tomate", Prezioa = 2.5, Stock = 100, HornitzaileakId = 1 };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(osagaia);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<OsagaiaDto>(okResult.Value);
            Assert.Equal(1, dto.Id);
            Assert.Equal("Tomate", dto.Izena);
            Assert.Equal(2.5, dto.Prezioa);
            Assert.Equal(100, dto.Stock);
            Assert.Equal(1, dto.HornitzaileakId);
        }

        [Fact]
        public void Get_OsagaiaEzDaExistitzen_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns((Osagaia?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}