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

            ApplicationUser Usuario = new ApplicationUser("Fulanito", "De Tal", "fulanitodetal@uclm.es", "693163783");

            var Compras = new List<Compra>() {
                new Compra("DireccionEnvio - Compra1", DateTime.Now, (float)100.99, new Efectivo(), Usuario),
                new Compra("DireccionEnvio - Compra2", DateTime.Now, (float)200.99, new TarjetaCredito(), Usuario),
            };

            var Fabricantes = new List<Fabricante>()
            {
                new Fabricante("Nombre - Fabricante1"),
                new Fabricante("Nombre - Fabricante2"),
            };

            var Herramientas = new List<Herramienta>()
            {
                new Herramienta("Nombre - Herramienta1", "Material - Herramienta1", (float)14.99, 200, Fabricantes[0]),
                new Herramienta("Nombre - Herramienta2", "Material - Herramienta2", (float)28.99, 100, Fabricantes[1]),
            };

            var Items = new List<CompraItem>(){
                new CompraItem(5, "Descripción - CompraItem1", (float)14.99, Herramientas[0], Compras[0]),
                new CompraItem(1, "Descripción - CompraItem2", (float)419.99, Herramientas[1], Compras[1]),
            };

            var rental = new Rental("elena.navarro@uclm.es", "Elena Navarro",
                   user, "Avda. España s/n, Albacete 02071",
DateTime.Now, AppForMovies.API.Models.PaymentMethodTypes.CreditCard,
DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                    new List<RentalItem>());
            rental.RentalItems.Add(new RentalItem(movies[0], rental, "My favourite movie"));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(genres);
            _context.AddRange(movies);
            _context.Add(rental);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetRental(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetRental_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;
            var controller = new RentalsController(_context, logger);


            var expectedRental = new RentalDetailDTO(1, DateTime.Now, "elena.navarro@uclm.es", "Elena Navarro",
                        "Avda. España s/n, Albacete 02071", PaymentMethodTypes.CreditCard,
                        DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                        new List<RentalItemDTO>());
            expectedRental.RentalItems.Add(new RentalItemDTO(1, "The lord of the rings", "Sci - Fi", 1.0, "My favourite movie"));

            // Act 
            var result = await controller.GetRental(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);
            var eq = expectedRental.Equals(rentalDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedRental, rentalDTOActual);

        }
    }
}