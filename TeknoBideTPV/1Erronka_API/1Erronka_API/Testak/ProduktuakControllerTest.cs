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
    public class ProduktuakControllerTest
    {
        private readonly ISessionFactory _dummyFactory;

        public ProduktuakControllerTest()
        {
            var factoryMock = new Mock<ISessionFactory>();
            factoryMock.Setup(f => f.GetCurrentSession()).Returns(new Mock<NHibernate.ISession>().Object);
            _dummyFactory = factoryMock.Object;
        }

        private ProduktuakController CreateController(Mock<_1Erronka_API.Repositorioak.ProduktuaRepository> repoMock)
        {
            return new ProduktuakController(repoMock.Object);
        }

        [Fact]
        public void GetAll_ProduktuakDaudenean_ListaItzuli()
        {
            // Arrange
            var produktuak = new List<Produktua>
            {
                new Produktua { Id = 1, Izena = "Pizza Margherita", Prezioa = 12.5, Mota = "Pizza", Stock = 10 },
                new Produktua { Id = 2, Izena = "Coca-Cola", Prezioa = 2.0, Mota = "Bebida", Stock = 50 }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(produktuak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dtoList = Assert.IsType<List<ProduktuaDto>>(okResult.Value);
            Assert.Equal(2, dtoList.Count);
            Assert.Equal(1, dtoList[0].Id);
            Assert.Equal("Pizza Margherita", dtoList[0].Izena);
            Assert.Equal(12.5, dtoList[0].Prezioa);
            Assert.Equal("Pizza", dtoList[0].Mota);
            Assert.Equal(10, dtoList[0].Stock);
            Assert.Equal(2, dtoList[1].Id);
            Assert.Equal("Coca-Cola", dtoList[1].Izena);
            Assert.Equal(2.0, dtoList[1].Prezioa);
            Assert.Equal("Bebida", dtoList[1].Mota);
            Assert.Equal(50, dtoList[1].Stock);
        }

        [Fact]
        public void Get_ProduktuaExistitzenDa_ProduktuaItzuli()
        {
            // Arrange
            var produktua = new Produktua { Id = 1, Izena = "Pizza Margherita", Prezioa = 12.5, Mota = "Pizza", Stock = 10 };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(produktua);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ProduktuaDto>(okResult.Value);
            Assert.Equal(1, dto.Id);
            Assert.Equal("Pizza Margherita", dto.Izena);
            Assert.Equal(12.5, dto.Prezioa);
            Assert.Equal("Pizza", dto.Mota);
            Assert.Equal(10, dto.Stock);
        }

        [Fact]
        public void Get_ProduktuaEzDaExistitzen_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns((Produktua?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ProduktuaExistitzenDa_StockEguneratu()
        {
            // Arrange
            var produktua = new Produktua { Id = 5, Izena = "Ura", Prezioa = 1.0, Mota = "Bebida", Stock = 3 };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(5)).Returns(produktua);
            repoMock.Setup(r => r.UpdateStock(5, 12));

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Update(5, new ProduktuaDto { Id = 5, Stock = 12 });

            // Assert
            Assert.IsType<OkResult>(result);
            repoMock.Verify(r => r.UpdateStock(5, 12), Times.Once);
        }

        [Fact]
        public void Update_ProduktuaEzDaExistitzen_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(5)).Returns((Produktua?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Update(5, new ProduktuaDto { Id = 5, Stock = 12 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_StockNegatiboa_BadRequestItzuli()
        {
            // Arrange
            var produktua = new Produktua { Id = 5, Izena = "Ura", Prezioa = 1.0, Mota = "Bebida", Stock = 3 };
            var repoMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(5)).Returns(produktua);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Update(5, new ProduktuaDto { Id = 5, Stock = -1 });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
