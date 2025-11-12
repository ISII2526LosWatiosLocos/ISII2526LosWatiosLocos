using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
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
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Martillo", "Acero", 13.95f, 5, fabricante[0]),
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Destornillador", "Acero", 7.0f, 3, fabricante[1]),
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Taladro", "Plástico", 5.0f, 10, fabricante[2]),
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
                13.95f,
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
                19.35f,
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

            var alquilerDTOActual = Assert.IsType<AlquileresParaDetalleDTO>(okResult.Value);

            // 2. Comprueba las propiedades simples (string, DateOnly, int, etc.)
            Assert.Equal(expectedAlquiler.FechaFinal, alquilerDTOActual.FechaFinal);
            Assert.Equal(expectedAlquiler.FechaInicio, alquilerDTOActual.FechaInicio);
            Assert.Equal(expectedAlquiler.FechaAlquiler, alquilerDTOActual.FechaAlquiler);
            Assert.Equal(expectedAlquiler.Nombre, alquilerDTOActual.Nombre);
            Assert.Equal(expectedAlquiler.Apellidos, alquilerDTOActual.Apellidos);
            Assert.Equal(expectedAlquiler.Direccion, alquilerDTOActual.Direccion);

            // 3. Comprueba las listas o colecciones
            //    Primero, comprueba que tengan el mismo número de elementos
            Assert.Equal(expectedAlquiler.Items.Count, alquilerDTOActual.Items.Count);

            // 4. Comprueba los elementos DENTRO de las listas
            //    (En este test, sabes que solo hay un item, en la posición [0])
            var expectedItem = expectedAlquiler.Items[0];
            var actualItem = alquilerDTOActual.Items[0];

            Assert.Equal(expectedItem.NombreItem, actualItem.NombreItem);
            Assert.Equal(expectedItem.MaterialItem, actualItem.MaterialItem);
            Assert.Equal(expectedItem.PrecioItem, actualItem.PrecioItem);
            Assert.Equal(expectedItem.CantidadItem, actualItem.CantidadItem);
        }
    }
}
