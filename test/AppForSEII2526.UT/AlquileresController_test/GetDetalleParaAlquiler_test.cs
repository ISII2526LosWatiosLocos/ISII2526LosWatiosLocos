using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquileresDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquileresController_test
{
    public class GetDetalleParaAlquiler_test : AppForSEII25264SqliteUT
    {
        public GetDetalleParaAlquiler_test()
        {
            //Datos de prueba
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };

            var herramienta = new List<Herramienta>()
            {
                new Herramienta("Martillo", "Acero", 13.95f, 5, 99, fabricante[0]),
                new Herramienta("Destornillador", "Acero", 7.0f, 3, 99, fabricante[1]),
                new Herramienta("Taladro", "Plástico", 5.0f, 10, 99, fabricante[2]),
            };

            var alquilerItem = new List<AlquilarItem>()
            {
                new AlquilarItem(13.95f, 10, null, herramienta[0]), // Precio, cantidad, alquiler, herramienta
            };

            var metodoPago = new Efectivo()
            {
                Nombre = "Efectivo"
            };

            ApplicationUser usuario = new ApplicationUser("5", "Jesus", "Arribas", "Jesus.Arribas@alu.uclm.es", "111111112");

            var alquiler = new Alquiler(
                "La Casa Blanca",
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                139.5f,
                alquilerItem,
                metodoPago,
                usuario
            );

            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.AddRange(alquilerItem);
            _context.AddRange(metodoPago);
            _context.AddRange(alquiler);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleParaAlquiler_NotFound()
        {
            //Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;

            var controller = new AlquileresController(_context, logger);

            //Act
            var nonExistingId = int.MaxValue; // asegurar que no exista en la BD de pruebas
            var result = await controller.GetDetalleHerramientasParaAlquiler(nonExistingId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleParaAlquiler_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;
            var controller = new AlquileresController(_context, logger);
            var expectedAlquiler = new AlquileresParaDetalleDTO(
                "Jesus",
                "Arribas",
                "La Casa Blanca",
                DateOnly.FromDateTime(DateTime.UtcNow),
                139.5f,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                new List<AlquilarItemsDTO>()
            );
            expectedAlquiler.Items.Add(new AlquilarItemsDTO
            (
                "Martillo",
                "Acero",
                13.95f,
                10
            ));

            //Act
            var result = await controller.GetDetalleHerramientasParaAlquiler(1);

            //Assert 
            // 1. Comprueba que los objetos no sean nulos
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Sacamos el DTO real
            var alquilerDTOActual = Assert.IsType<AlquileresParaDetalleDTO>(okResult.Value);

            // Comparamos directamente el objeto completo
            Assert.Equal(expectedAlquiler, alquilerDTOActual);

        }
    }
}
