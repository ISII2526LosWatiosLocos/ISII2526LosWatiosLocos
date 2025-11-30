using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class GetDetalleParaOferta_test : AppForSEII25264SqliteUT
    {
        public GetDetalleParaOferta_test()
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
                new Herramienta("Martillo", "Acero", 15.5f, 99, 5, fabricante[0]),
                new Herramienta("Destornillador", "Acero", 7.0f, 99, 3, fabricante[1]),
                new Herramienta("Taladro", "Plástico", 5.0f, 99, 10, fabricante[2]),
            };

            var ofertaItem = new List<OfertaItem>()
            {
                new OfertaItem(10, 13.95f, null, herramienta[0]),
            };

            var metodoPago = new Efectivo()
            {
                Nombre = "Efectivo"
            };

            ApplicationUser usuario = new ApplicationUser("5", "Jesus", "Arribas", "Jesus.Arribas@alu.uclm.es", "111111112");

            var oferta = new Oferta(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                tipoDirigidaOferta.Cliente,
                ofertaItem,
                metodoPago,
                usuario
            );

            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.AddRange(ofertaItem);
            _context.AddRange(metodoPago);
            _context.AddRange(oferta);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleParaOferta_NotFound()
        {
            //Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);

            //Act
            var result = await controller.GetDetalleHerramientasParaOferta(999); // ID de oferta que no existe (999)

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleParaOferta_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;
            var controller = new OfertasController(_context, logger);

            // Creamos el objeto que ESPERAMOS recibir.
            var expectedOferta = new OfertasParaDetalleDTO(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)), // Fecha Final
                DateOnly.FromDateTime(DateTime.UtcNow),             // Fecha Inicio
                DateOnly.FromDateTime(DateTime.UtcNow),             // Fecha Oferta
                "Cliente",
                "Efectivo",
                new List<OfertaItemsDTO>(),
                "Jesus"
            );

            expectedOferta.Items.Add(new OfertaItemsDTO
            (
                "Martillo",
                "Acero",
                "Herramientas SA",
                15.5f,
                13.95f
            ));

            //Act
            var result = await controller.GetDetalleHerramientasParaOferta(1);

            //Assert 

            // 1. Verificaciones básicas de que la respuesta HTTP es correcta
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOActual = Assert.IsType<OfertasParaDetalleDTO>(okResult.Value);


            Assert.Equal(expectedOferta, ofertaDTOActual);
        }
    }
}
