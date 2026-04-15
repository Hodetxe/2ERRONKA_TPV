using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EskariakControllerTests
    {
        private readonly ISessionFactory _dummyFactory;

        public EskariakControllerTests()
        {
            var factoryMock = new Mock<ISessionFactory>();
            factoryMock.Setup(f => f.GetCurrentSession()).Returns(new Mock<NHibernate.ISession>().Object);
            _dummyFactory = factoryMock.Object;
        }

        private EskariakController CreateController(
            Mock<_1Erronka_API.Repositorioak.EskariaRepository> repoMock,
            Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>? produktuaMock = null,
            Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>? erreserbaMock = null,
            Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>? produktuOsagaiaMock = null,
            Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>? osagaiaMock = null)
        {
            produktuaMock ??= new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Loose, _dummyFactory);
            erreserbaMock ??= new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Loose, _dummyFactory);
            produktuOsagaiaMock ??= new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Loose, _dummyFactory);
            osagaiaMock ??= new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Loose, _dummyFactory);

            return new EskariakController(
                repoMock.Object,
                produktuaMock.Object,
                erreserbaMock.Object,
                produktuOsagaiaMock.Object,
                osagaiaMock.Object);
        }

        [Fact]
        public void Sortu_DatuakZuzenaDirenean_EskariaSortuEtaOkItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());
            Eskaria? captured = null;
            repoMock.Setup(r => r.Add(It.IsAny<Eskaria>())).Callback<Eskaria>(e => captured = e);

            var erreserba = new Erreserba { Id = 1, Langilea = new Langilea { Id = 1, Izena = "L" }, Mahaia = new Mahaia { Id = 1 } };
            var erreserbaMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            erreserbaMock.Setup(r => r.Get(1)).Returns(erreserba);

            var produktua = new Produktua { Id = 10, Izena = "P", Prezioa = 5.0, Stock = 10 };
            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);
            produktuaMock.Setup(p => p.Update(It.IsAny<Produktua>()));

            var osagaia = new Osagaia { Id = 100, Izena = "O", Stock = 100 };
            var produktuOsagaiaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuOsagaiaMock.Setup(p => p.GetByProduktuaId(10)).Returns(new List<ProduktuaOsagaia>
            {
                new ProduktuaOsagaia { Produktua = produktua, Osagaia = osagaia, Kantitatea = 1 }
            });
            var osagaiaMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            osagaiaMock.Setup(o => o.Update(It.IsAny<Osagaia>()));

            var controller = CreateController(repoMock, produktuaMock, erreserbaMock, produktuOsagaiaMock, osagaiaMock);

            var dto = new EskariaSortuDto
            {
                ErreserbaId = 1,
                Prezioa = 0,
                Egoera = "Prestatzen",
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 2, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic body = okResult.Value!;
            Assert.Equal(captured!.Id, (int)body.EskariaId);
            Assert.Equal(10.0, (double)body.PrezioaTotala);
            var produktuak = (IEnumerable<object>)body.Produktuak;
            Assert.Single(produktuak);
            Assert.Equal("P", ((dynamic)produktuak.First()).ProduktuaIzena);
            Assert.Equal(2, ((dynamic)produktuak.First()).Kantitatea);
        }

        [Fact]
        public void Sortu_ErreserbaEzExistitzean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var erreserbaMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            erreserbaMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Erreserba?)null);

            var controller = CreateController(repoMock, erreserbaMock: erreserbaMock);
            var dto = new EskariaSortuDto { ErreserbaId = 99 };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erreserba ez da aurkitu", badResult.Value);
        }

        [Fact]
        public void Sortu_ErreserbakLangilerikEzDuenean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var erreserba = new Erreserba { Id = 1, Langilea = null!, Mahaia = new Mahaia { Id = 1 } };
            var erreserbaMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            erreserbaMock.Setup(r => r.Get(1)).Returns(erreserba);

            var controller = CreateController(repoMock, erreserbaMock: erreserbaMock);
            var dto = new EskariaSortuDto { ErreserbaId = 1 };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Erreserbak ez du langilerik asignatuta", badResult.Value);
        }

        [Fact]
        public void Sortu_ProduktuakStockikEzDuenean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var erreserba = new Erreserba { Id = 1, Langilea = new Langilea { Id = 1, Izena = "L" }, Mahaia = new Mahaia { Id = 1 } };
            var erreserbaMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            erreserbaMock.Setup(r => r.Get(1)).Returns(erreserba);

            var produktua = new Produktua { Id = 10, Izena = "P", Prezioa = 5.0, Stock = 0 };
            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);

            var controller = CreateController(repoMock, produktuaMock, erreserbaMock);
            var dto = new EskariaSortuDto
            {
                ErreserbaId = 1,
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 1, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Ez dago stock-ik", badResult.Value.ToString());
        }

        [Fact]
        public void Sortu_OsagaiakStockNahikorikEzDuenean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var erreserba = new Erreserba { Id = 1, Langilea = new Langilea { Id = 1, Izena = "L" }, Mahaia = new Mahaia { Id = 1 } };
            var erreserbaMock = new Mock<_1Erronka_API.Repositorioak.ErreserbaRepository>(MockBehavior.Strict, _dummyFactory);
            erreserbaMock.Setup(r => r.Get(1)).Returns(erreserba);

            var produktua = new Produktua { Id = 10, Izena = "P", Prezioa = 5.0, Stock = 10 };
            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);

            var osagaia = new Osagaia { Id = 100, Izena = "O", Stock = 0 };
            var produktuOsagaiaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuOsagaiaMock.Setup(p => p.GetByProduktuaId(10)).Returns(new List<ProduktuaOsagaia>
            {
                new ProduktuaOsagaia { Produktua = produktua, Osagaia = osagaia, Kantitatea = 1 }
            });

            var controller = CreateController(repoMock, produktuaMock, erreserbaMock, produktuOsagaiaMock);

            var dto = new EskariaSortuDto
            {
                ErreserbaId = 1,
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 1, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Sortu(dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Ez dago nahikoa stock", badResult.Value.ToString());
        }

        [Fact]
        public void Eguneratu_EskariaEzExistitzean_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Eskaria?)null);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var controller = CreateController(repoMock);
            var dto = new EskariaSortuDto();

            // Act
            var result = controller.Eguneratu(999, dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Eskaria ez da aurkitu", badResult.Value);
        }

        [Fact]
        public void Eguneratu_StockGutxiagoProduktuan_BadRequestItzuli()
        {
            // Arrange
            var eskaria = new Eskaria
            {
                Id = 1,
                Produktuak = new List<EskariaProduktua>
                {
                    new EskariaProduktua { Produktua = new Produktua { Id = 10, Izena = "P", Stock = 0 }, Kantitatea = 1, Prezioa = 5.0 }
                }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(eskaria);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(new Produktua { Id = 10, Izena = "P", Stock = 0 });

            var controller = CreateController(repoMock, produktuaMock);
            var dto = new EskariaSortuDto
            {
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 2, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Eguneratu(1, dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Ez dago stock-ik", badResult.Value.ToString());
        }

        [Fact]
        public void Eguneratu_OsagaiakStockNahikorikEzDuenean_BadRequestItzuli()
        {
            // Arrange
            var produktua = new Produktua { Id = 10, Izena = "P", Stock = 10 };
            var eskaria = new Eskaria
            {
                Id = 1,
                Produktuak = new List<EskariaProduktua>
                {
                    new EskariaProduktua { Produktua = produktua, Kantitatea = 1, Prezioa = 5.0 }
                }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(eskaria);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);
            produktuaMock.Setup(p => p.Update(It.IsAny<Produktua>()));

            var osagaia = new Osagaia { Id = 100, Izena = "O", Stock = 0 };
            var produktuOsagaiaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuOsagaiaMock.Setup(p => p.GetByProduktuaId(10)).Returns(new List<ProduktuaOsagaia>
            {
                new ProduktuaOsagaia { Produktua = produktua, Osagaia = osagaia, Kantitatea = 2 }
            });

            var controller = CreateController(repoMock, produktuaMock, null, produktuOsagaiaMock);
            var dto = new EskariaSortuDto
            {
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 2, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Eguneratu(1, dto);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Ez dago nahikoa stock", badResult.Value.ToString());
        }

        [Fact]
        public void Eguneratu_EskariaOndoEguneratzenDenenean_OkTrueItzuli()
        {
            // Arrange
            var produktua = new Produktua { Id = 10, Izena = "P", Stock = 5 };
            var osagaia = new Osagaia { Id = 100, Izena = "O", Stock = 10 };
            var eskaria = new Eskaria
            {
                Id = 1,
                Prezioa = 10,
                Egoera = "Prestatzen",
                Produktuak = new List<EskariaProduktua>
                {
                    new EskariaProduktua { Produktua = produktua, Kantitatea = 1, Prezioa = 5.0 }
                }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(eskaria);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());
            repoMock.Setup(r => r.Update(It.IsAny<Eskaria>()));

            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);
            produktuaMock.Setup(p => p.Update(It.IsAny<Produktua>()));

            var produktuOsagaiaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuOsagaiaMock.Setup(p => p.GetByProduktuaId(10)).Returns(new List<ProduktuaOsagaia>
            {
                new ProduktuaOsagaia { Produktua = produktua, Osagaia = osagaia, Kantitatea = 1 }
            });

            var osagaiaMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            osagaiaMock.Setup(o => o.Update(It.IsAny<Osagaia>()));

            var controller = CreateController(repoMock, produktuaMock, null, produktuOsagaiaMock, osagaiaMock);

            var dto = new EskariaSortuDto
            {
                Prezioa = 5,
                Egoera = "Zerbitzatuta",
                Produktuak = new List<EskariaProduktuaSortuDto>
                {
                    new EskariaProduktuaSortuDto { ProduktuaId = 10, Kantitatea = 2, Prezioa = 5.0 }
                }
            };

            // Act
            var result = controller.Eguneratu(1, dto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(4, produktua.Stock); // 5 - diff (1)
            Assert.Equal(9, osagaia.Stock); // 10 - diff (1)
            Assert.Equal("Zerbitzatuta", eskaria.Egoera);
            Assert.Equal(5, eskaria.Prezioa);
            repoMock.Verify(r => r.Update(eskaria), Times.Once);
        }

        [Fact]
        public void Ezabatu_EskariaEzDaAurkitu_BadRequestItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Eskaria?)null);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());

            var controller = CreateController(repoMock);

            // Act
            var result = controller.Ezabatu(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Ezabatu_EskariaExistitzenDenean_EzabatuEtaOkItzuli()
        {
            // Arrange
            var produktua = new Produktua { Id = 10, Izena = "P", Stock = 5 };
            var osagaia = new Osagaia { Id = 100, Izena = "O", Stock = 5 };
            var eskaria = new Eskaria
            {
                Id = 1,
                Produktuak = new List<EskariaProduktua>
                {
                    new EskariaProduktua { Produktua = produktua, Kantitatea = 2, Prezioa = 5.0 }
                }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(eskaria);
            repoMock.Setup(r => r.ExecuteSerializableTransaction(It.IsAny<Action>())).Callback<Action>(a => a());
            repoMock.Setup(r => r.Delete(It.IsAny<Eskaria>()));

            var produktuaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuaMock.Setup(p => p.Get(10)).Returns(produktua);
            produktuaMock.Setup(p => p.Update(It.IsAny<Produktua>()));

            var produktuOsagaiaMock = new Mock<_1Erronka_API.Repositorioak.ProduktuaOsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            produktuOsagaiaMock.Setup(p => p.GetByProduktuaId(10)).Returns(new List<ProduktuaOsagaia>
            {
                new ProduktuaOsagaia { Produktua = produktua, Osagaia = osagaia, Kantitatea = 1 }
            });

            var osagaiaMock = new Mock<_1Erronka_API.Repositorioak.OsagaiaRepository>(MockBehavior.Strict, _dummyFactory);
            osagaiaMock.Setup(o => o.Update(It.IsAny<Osagaia>()));

            var controller = CreateController(repoMock, produktuaMock, null, produktuOsagaiaMock, osagaiaMock);

            // Act
            var result = controller.Ezabatu(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value!);
            Assert.Equal(7, produktua.Stock);
            Assert.Equal(7, osagaia.Stock);
            repoMock.Verify(r => r.Delete(eskaria), Times.Once);
        }

        [Fact]
        public void GetEskaria_EskariaEzDaAurkitu_NotFoundItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(It.IsAny<int>())).Returns((Eskaria?)null);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskaria(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetEskaria_ExistitzenDenEskaria_OkDtoItzuli()
        {
            // Arrange
            var eskaria = new Eskaria
            {
                Id = 1,
                Prezioa = 12.5,
                Egoera = "Prestatzen",
                Erreserba = new Erreserba { Id = 2 },
                Produktuak = new List<EskariaProduktua>
                {
                    new EskariaProduktua { Produktua = new Produktua { Id = 5, Izena = "X" }, Kantitatea = 3, Prezioa = 4.0 }
                }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.Get(1)).Returns(eskaria);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskaria(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<EskariaDto>(okResult.Value);
            Assert.Equal(1, dto.Id);
            Assert.Equal(12.5, dto.Prezioa);
            Assert.Single(dto.Produktuak);
            Assert.Equal(5, dto.Produktuak[0].ProduktuaId);
        }

        [Fact]
        public void GetEskariakByErreserba_EskariakDaudenean_ListaItzuli()
        {
            // Arrange
            var eskariak = new List<Eskaria>
            {
                new Eskaria { Id = 1, Erreserba = new Erreserba { Id = 10 }, Egoera = "P" },
                new Eskaria { Id = 2, Erreserba = new Erreserba { Id = 20 }, Egoera = "P" }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(eskariak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariakByErreserba(10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Single(list);
            Assert.Equal(10, list[0].ErreserbaId);
        }

        [Fact]
        public void GetEskariakByErreserba_EskariarikEzDagoenean_ListaHutsaItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Eskaria>());

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariakByErreserba(42);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Empty(list);
        }

        [Fact]
        public void GetEskariak_EskariakDaudenean_ListaItzuli()
        {
            // Arrange
            var eskariak = new List<Eskaria>
            {
                new Eskaria { Id = 1, Prezioa = 5, Egoera = "P", Erreserba = new Erreserba { Id = 2, Mahaia = new Mahaia { Zenbakia = 7 } } }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(eskariak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariak();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Single(list);
            Assert.Equal(7, list[0].MahaiaZenbakia);
        }

        [Fact]
        public void GetEskariak_DBHutsikDenenean_ListaHutsaItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Eskaria>());

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariak();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Empty(list);
        }

        [Fact]
        public void GetEskariakByEgoera_EgoeraHorretanEskariakDaudenean_ListaItzuli()
        {
            // Arrange
            var eskariak = new List<Eskaria>
            {
                new Eskaria { Id = 1, Egoera = "Prestatzen", Erreserba = new Erreserba { Id = 2 } },
                new Eskaria { Id = 2, Egoera = "Zerbitzatuta", Erreserba = new Erreserba { Id = 3 } }
            };

            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(eskariak);

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariakByEgoera("prestatzen");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Single(list);
            Assert.Equal("Prestatzen", list[0].Egoera);
        }

        [Fact]
        public void GetEskariakByEgoera_EgoeraHorretanEskariarikEzDagoenean_ListaHutsaItzuli()
        {
            // Arrange
            var repoMock = new Mock<_1Erronka_API.Repositorioak.EskariaRepository>(MockBehavior.Strict, _dummyFactory);
            repoMock.Setup(r => r.GetAll()).Returns(new List<Eskaria>());

            var controller = CreateController(repoMock);

            // Act
            var result = controller.GetEskariakByEgoera("prestatzen");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<EskariaDto>>(okResult.Value);
            Assert.Empty(list);
        }
    }
}
