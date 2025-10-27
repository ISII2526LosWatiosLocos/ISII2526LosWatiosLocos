/*
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class GetCompras_test : AppForSEII25264SqliteUT
    {
        public GetCompras_test()
        {
            // Seed de datos en la BBDD de prueba:

            var Fabricantes = new List<Fabricante>()
            {
                new Fabricante("Nombre - Fabricante1", new List<Herramienta>()),
                new Fabricante("Nombre - Fabricante2", new List<Herramienta>()),
            };

            var Herramientas = new List<Herramienta>()
            {
                new Herramienta("Nombre - Herramienta1", "Material - Herramienta1", (float)14.99, 200, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Nombre - Herramienta2", "Material - Herramienta2", (float)28.99, 100, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
            };

            var Items = new List<CompraItem>(){
                new CompraItem(5, "Descripción - CompraItem1", (float)14.99, null, null),
                new CompraItem(1, "Descripción - CompraItem2", (float)419.99, null, null),
            };

            var Compras = new List<Compra>() {
                new Compra("DireccionEnvio - Compra1", DateTime.Now, (float)100.99, new List<CompraItem>(), new Efectivo(), null),
                new Compra("DireccionEnvio - Compra2", DateTime.Now, (float)200.99, new List<CompraItem>(), new TarjetaCredito(), null),
            };

            ApplicationUser Usuario = new ApplicationUser("Fulanito", "De Tal", "fulanitodetal@uclm.es", "693163783", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

            // Añadir cosas

            // Asigno herramientas a los fabricantes
            Fabricantes[0].Herramientas.Add(Herramientas[0]);
            Fabricantes[1].Herramientas.Add(Herramientas[1]);

            // Asigno items y fabricantes a las herramientas
            Herramientas[0].CompraItems.Add(Items[0]);
            Herramientas[1].CompraItems.Add(Items[1]);
            Herramientas[0].Fabricante = Fabricantes[0];
            Herramientas[1].Fabricante = Fabricantes[1];

            // Asigno herramientas y las compras a los items
            Items[0].Herramienta = Herramientas[0];
            Items[1].Herramienta = Herramientas[1];
            Items[0].Compra = Compras[0];
            Items[1].Compra = Compras[1];

            // Asigno items y el usuario a las compras
            Compras[0].CompraItems.Add(Items[0]);
            Compras[1].CompraItems.Add(Items[1]);
            Compras[0].Usuario = Usuario;
            Compras[1].Usuario = Usuario;

            _context.ApplicationUsers.Add(user);
            _context.AddRange(genres);
            _context.AddRange(movies);
            _context.Add(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.GetCompra(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompra_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;
            var controller = new ComprasController(_context, logger);


            var expectedCompra = new ComprasController(1, DateTime.Now, "elena.navarro@uclm.es", "Elena Navarro",
                        "Avda. España s/n, Albacete 02071", PaymentMethodTypes.CreditCard,
                        DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                        new List<CompraItemsDTO>());
            expectedCompra.CompraItems.Add(new CompraItemsDTO(1, "The lord of the rings", "Sci - Fi", 1.0, "My favourite movie"));

            // Act 
            var result = await controller.GetCompra(1);

            //Assert
            //we check that the response type is OK and obtain the compra
            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraDTOActual = Assert.IsType<ComprasParaDetalleDTO>(okResult.Value);
            var eq = expectedCompra.Equals(compraDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedCompra, compraDTOActual);

        }
    }
}
*/