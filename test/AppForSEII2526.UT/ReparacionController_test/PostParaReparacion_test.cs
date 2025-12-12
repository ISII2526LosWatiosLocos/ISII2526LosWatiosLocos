using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.ReparacionesController_test
{
    public class PostParaReparacion_test : AppForSEII25264SqliteUT
    {
        public PostParaReparacion_test()
        {
            // --- Seed de datos base para las pruebas ---

            var usuario = new ApplicationUser(
                "Fulanito",
                "De Tal",
                "fulanitodetal@uclm.es",
                "+34111222333",
                new List<Compra>(),
                new List<Reparación>(),
                new List<Alquiler>()
            );


            // usuario para porbar la modificación del examen 

            var fabricante1 = new Fabricante("Fabricante1", new List<Herramienta>());
            var fabricante2 = new Fabricante("Fabricante2", new List<Herramienta>());

            var herramientas = new List<Herramienta>()
            {
                new Herramienta("Taladro", "Acero", 50.0f, 99, 10, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), fabricante1),
                new Herramienta("Martillo", "Hierro", 20.0f, 99, 15, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), fabricante2),
                new Herramienta("Sierra", "Acero", 30.0f, 99, 8, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), fabricante2)
            };

            fabricante1.Herramientas.Add(herramientas[0]);
            fabricante2.Herramientas.Add(herramientas[1]);
            fabricante2.Herramientas.Add(herramientas[2]);

            var efectivo = new Efectivo() { Nombre = "Efectivo" };
            var tarjeta = new TarjetaCredito() { Nombre = "TarjetaCredito" };

            _context.Users.Add(usuario);
            // modifcación 
            
            //
            _context.Fabricantes.AddRange(fabricante1, fabricante2);
            _context.Herramientas.AddRange(herramientas);
            _context.MetodosPagos.AddRange(efectivo, tarjeta);
            _context.SaveChanges();
        }

        // --- CASOS DE USO INVÁLIDOS ---
        public static IEnumerable<object[]> CasosDeUso_CrearReparacion()
        {
            var itemsBase = new List<ReparacionesItemDTO>()
            {
                new ReparacionesItemDTO { HerramientaId = 1, HerramientaDescripcion = "Cambio de broca", HerramientaCantidad = 1 },
                new ReparacionesItemDTO { HerramientaId = 2, HerramientaDescripcion = "Reparar mango", HerramientaCantidad = 2 },
            };

            var reparacionUsuarioNoExiste = new CrearReparacionDTO
            {
                Nombre = "Pepe",
                Apellidos = "Grillo",
                MetodoPagoId = 1,
                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                ReparacionesItems = itemsBase
            }; 

            var reparacionMetodoPagoInvalido = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 999,
                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                ReparacionesItems = itemsBase
            };

            var reparacionSinItems = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,

                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                ReparacionesItems = new List<ReparacionesItemDTO>()
            };

            var reparacionFechaEntregaPasada = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,

                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                ReparacionesItems = itemsBase
            };

            var reparacionFechaRecogidaAntesEntrega = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                ReparacionesItems = itemsBase
            };


            var reparacionNumeroInvalido = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                telefono = "111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                ReparacionesItems = itemsBase
            };

            return new List<object[]>
            {
                new object[] { reparacionUsuarioNoExiste, "El Usuario Pepe Grillo no existe." },
                new object[] { reparacionMetodoPagoInvalido, "El MetodoPagoId 999 no existe." },
                new object[] { reparacionSinItems, "Error: debe incluir al menos una herramienta para reparación" },
                new object[] { reparacionFechaEntregaPasada, "Error: la fecha de entrega debe ser posterior a hoy" },
                new object[] { reparacionFechaRecogidaAntesEntrega, "Error: la fecha de recogida debe ser posterior a la entrega" },
                    new object[] { reparacionNumeroInvalido, "Error. El usuario debe de empezar por  +34 " },
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(CasosDeUso_CrearReparacion))]
        public async Task PostParaReparacion_CrearReparacion_Test(CrearReparacionDTO dto, string mensajeEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            var controller = new ReparacionesController(_context, mock.Object);

            // Act
            var result = await controller.CreateReparacion(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var details = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var error = details.Errors.First().Value[0];
            Assert.StartsWith(mensajeEsperado, error);
        }

        // --- CASO EXITOSO ---
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task PostParaReparacion_CrearReparacion_Exitoso()
        {
            // Arrange
            var mock = new Mock<ILogger<ReparacionesController>>();
            var controller = new ReparacionesController(_context, mock.Object);

            var items = new List<ReparacionesItemDTO>()
            {
                new ReparacionesItemDTO{ HerramientaId = 1, HerramientaDescripcion = "Sustitución de broca", HerramientaCantidad = 1 },
                new ReparacionesItemDTO { HerramientaId = 2, HerramientaDescripcion = "Cambio de mango", HerramientaCantidad = 2 },
            };

            var dto = new CrearReparacionDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                telefono = "+34111222333",
                FechaEntrega = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                FechaRecogida = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                ReparacionesItems = items
            };

            // Act
            var result = await controller.CreateReparacion(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var reparacionDTO = Assert.IsType<ReparacionesDTO>(created.Value);

            var expectedDTO = new ReparacionesDTO(
                    "Fulanito",  // nombre
                    "De Tal",    // apellidos
                    dto.FechaEntrega,
                    dto.FechaRecogida,
                    90.0f, // Precio total: Taladro (50 * 1) + Martillo (20 * 2) = 90
                    new List<ReparacionesItemDTO>
                    {
            new ReparacionesItemDTO("Taladro", "Sustitución de broca", 1, 50.0f),
            new ReparacionesItemDTO("Martillo", "Cambio de mango", 2, 20.0f)
                    }
                );

            Assert.Equal(expectedDTO, reparacionDTO);
        }

        }
    }
