using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetSeleccionReparacion_test : AppForSEII25264SqliteUT
    {
        public GetSeleccionReparacion_test()
        {



            ApplicationUser usuario = new ApplicationUser("Pepe ", "Gascón Villalverde", "fulanitodetal@uclm.es", "111222333", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

            var herramienta = new List<Herramienta>()
            {
                new Herramienta(1, "Martillo", "Acero", 15.5f, 5, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(2, "Destornillador", "Acero", 7.0f, 3, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(3, "Taladro", "Plástico", 45.0f, 10, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(4, "Sierra", "Acero", 30.0f, 7, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(5, "Llave inglesa", "Acero", 20.0f, 4, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null)
            };

            var fabricante = new List<Fabricante>()
            {
               
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };



            Efectivo efectivo = new Efectivo();
            efectivo.Nombre = "Efectivo";
            TarjetaCredito tarjetaCredito = new TarjetaCredito();
            tarjetaCredito.Nombre = "TarjetaCredito";



            var Reparacion = new Reparación(
    DateOnly.FromDateTime(DateTime.Now),
    DateOnly.FromDateTime(DateTime.Now),
    125.78f,
    new Efectivo(),
    new List<ReparaciónItem>()
);

            var ReparacionItems = new List<ReparaciónItem>()
            {
                new ReparaciónItem(10, "Reparar sierra", 56.89f,Reparacion, herramienta[0]),
                new ReparaciónItem(30, "Repara radial",70.25f, Reparacion, herramienta[2]),
                new ReparaciónItem(20, "Reparar serrucho",15.67f, Reparacion, herramienta[4])

            };


            Reparacion.ReparaciónItems= ReparacionItems;

            // 2.
            _context.AddRange(fabricante); 
            _context.Add(usuario);
            _context.Add(Reparacion);
         
            _context.AddRange(herramienta);
            _context.AddRange(ReparacionItems);
          
         
            //Guardar los cambio s en la memoria de la database 
        _context.SaveChanges();
        }






        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleParaReparacion_NotFound()
        {
            //Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;

            var controller = new ReparacionesController(_context, logger);

            //Act
            var result = await controller.GetDetalleHerramientasParaReparación(999); 

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleParaReparacion_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;
            var controller = new ReparacionesController(_context, logger);

            var expectedReparacion = new ReparacionesDTO(
                "Pepe", 
                "Gascón Villaverde",
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                77.5f, 
                new List<ReparacionesItemDTO>()
               
            );
            expectedReparacion.ReparacionesItems.Add(new ReparacionesItemDTO
            (
                "Martillo",
                "de Bola",
                2,
                35.95f
            ));

            //Act
            var result = await controller.GetDetalleHerramientasParaReparación(1);

            //Assert 
            // 1. Comprueba que los objetos no sean nulos
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var ReparacionDTOActual = Assert.IsType<ReparacionesDTO>(okResult.Value);

            // 2. Comprueba las propiedades simples (string, DateOnly, int, etc.)
            Assert.Equal(expectedReparacion.nombre, ReparacionDTOActual.nombre);
            Assert.Equal(expectedReparacion.apellidos, ReparacionDTOActual.apellidos);
            Assert.Equal(expectedReparacion.FechaEntrega, ReparacionDTOActual.FechaEntrega);
            Assert.Equal(expectedReparacion.FechaRecogida, ReparacionDTOActual.FechaRecogida);
            Assert.Equal(expectedReparacion.PrecioTotal, ReparacionDTOActual.PrecioTotal);

            // 3. Comprueba las listas o colecciones
            //    Primero, comprueba que tengan el mismo número de elementos
            Assert.Equal(expectedReparacion.ReparacionesItems.Count, ReparacionDTOActual.ReparacionesItems.Count);

            // 4. Comprueba los elementos DENTRO de las listas
            //    (En este test, sabes que solo hay un item, en la posición [0])
            var expectedItem = expectedReparacion.ReparacionesItems[0];
            var actualItem = ReparacionDTOActual.ReparacionesItems[0];

            Assert.Equal(expectedItem.HerramientaNombre, actualItem.HerramientaNombre);
            Assert.Equal(expectedItem.HerramientaDescripcion, actualItem.HerramientaDescripcion);
            Assert.Equal(expectedItem.HerramientaCantidad, actualItem.HerramientaCantidad);
            Assert.Equal(expectedItem.HerramientaPrecio, actualItem.HerramientaPrecio);
     
        }
    }
    }
