using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ReparacionesController_test
{
    public class GetDetalleParaReparacion_test : AppForSEII25264SqliteUT
    {
        // Motivo de por qué se usan estas variables explicado en la respuesta.
        // Necesitamos fechas futuras predecibles para la lógica de Reparaciones.
        private readonly DateOnly _fechaEntrega;
        private readonly DateOnly _fechaRecogida;

        public GetDetalleParaReparacion_test()
        {
            // Definimos fechas futuras exactas
            _fechaEntrega = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
            _fechaRecogida = DateOnly.FromDateTime(DateTime.Today.AddDays(5));

            // Seed de datos en la BBDD de prueba (siguiendo la estructura de tu compañero):

            // 1. Crear Entidades
            ApplicationUser usuario = new ApplicationUser("Juan", "Pérez", "juanperez@uclm.es", "123456789", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

            var fabricantes = new List<Fabricante>()
            {
                new Fabricante("Fabricante-Test-Reparacion", new List<Herramienta>())
            };

            var herramientas = new List<Herramienta>()
            {
                new Herramienta("Martillo Pro", "Acero Forjado", 15.50f, 99, 50, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Destornillador Estrella", "Cromo-Vanadio", 5.25f, 99, 100, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null)
            };

            // El nombre de un método de pago no puede ser null en la bbdd
            Efectivo efectivo = new Efectivo();
            efectivo.Nombre = "Efectivo";

            var reparaciones = new List<Reparación>()
            {
                new Reparación
                {
                    FechaEntrega = _fechaEntrega,
                    FechaRecogida = _fechaRecogida,
                    PrecioTotal = 26.00f, // (1 * 15.50) + (2 * 5.25) = 26.00
                    ReparaciónItems = new List<ReparaciónItem>()
                    
                }
            };

            var items = new List<ReparaciónItem>()
            {
                new ReparaciónItem { cantidad = 1, Descripción = "Arreglar mango", Precio = 15.50f /* Este precio es del item, no de la herramienta */ },
                new ReparaciónItem { cantidad = 2, Descripción = "Cambiar punta", Precio = 10.50f /* (5.25 * 2) */ }
            };

            // 2. Entrelazar los datos:

            // Asigno herramientas al fabricante
            fabricantes[0].Herramientas.AddRange(herramientas);

            // Asigno fabricante a las herramientas
            herramientas[0].Fabricante = fabricantes[0];
            herramientas[1].Fabricante = fabricantes[0];

            // Asigno items a las herramientas
            herramientas[0].ReparaciónItems.Add(items[0]);
            herramientas[1].ReparaciónItems.Add(items[1]);

            // Asigno herramientas y la reparación a los items
            items[0].Herramienta = herramientas[0];
            items[1].Herramienta = herramientas[1];
            items[0].Reparacion = reparaciones[0];
            items[1].Reparacion = reparaciones[0];

            // Asigno items, usuario y método de pago a la reparación
            reparaciones[0].ReparaciónItems.AddRange(items);
            reparaciones[0].Usuario = usuario;
            reparaciones[0].MétodoPago = efectivo;

            // Asigno reparación al usuario
            usuario.Reparaciones.Add(reparaciones[0]);

            // 3. Añado los datos a la BBDD
            _context.Users.Add(usuario);
            _context.Fabricantes.AddRange(fabricantes);
            _context.Herramientas.AddRange(herramientas);
            _context.MetodosPagos.Add(efectivo);
            _context.Reparaciones.AddRange(reparaciones);
            _context.ReparaciónItems.AddRange(items);

            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleHerramientasParaReparación_NotFound()
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
        public async Task GetDetalleHerramientasParaReparación_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            ILogger<ReparacionesController> logger = mock.Object;
            var controller = new ReparacionesController(_context, logger);

            var expectedReparacion = new ReparacionesDTO(
                "Juan",     
                "Pérez",    
                _fechaEntrega,  
                _fechaRecogida, 
                26.00f,     
                new List<ReparacionesItemDTO>()
            );

            // El DTO de item se construye con: (nombre, descripcion, cantidad, precioHerramienta)
            expectedReparacion.ReparacionesItems.Add(new ReparacionesItemDTO(
                "Martillo Pro",         
                "Arreglar mango",       
                1,                      
                15.50f                  
            ));
            expectedReparacion.ReparacionesItems.Add(new ReparacionesItemDTO(
                "Destornillador Estrella", 
                "Cambiar punta",           
                2,                         
                5.25f                      
            ));

            //Act
            // Buscamos la reparación con Id = 1 (la única que hemos añadido)
            var result = await controller.GetDetalleHerramientasParaReparación(1);

            //Assert
            // 1. Comprueba que los objetos no sean nulos
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);

            // 2.
        
            var reparacionesDTOList = Assert.IsType<List<ReparacionesDTO>>(okResult.Value);

            // 3. Comprueba que la lista contenga un solo elemento
            Assert.Single(reparacionesDTOList);
            var reparacionDTOActual = reparacionesDTOList[0];


            Assert.Equal(expectedReparacion, reparacionDTOActual);

        }
    }
}