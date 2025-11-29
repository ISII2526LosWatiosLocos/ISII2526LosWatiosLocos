using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class Post_oferta_test : AppForSEII25264SqliteUT
    {
        private const string _nombreUsuario = "usuario1";

        private const string _nombreHerramienta1 = "Herramienta1";
        private const string _nombreHerramienta2 = "Herramienta2";
        private const string _nombreFabricante1 = "Fabricante1";
        private const string _nombreFabricante2 = "Fabricante2";

        public Post_oferta_test()
        {
            var fabricante = new List<Fabricante>
            {
                new Fabricante (_nombreFabricante1),
                new Fabricante (_nombreFabricante2)
            };

            var herramienta = new List<Herramienta>
            {
                new Herramienta (_nombreHerramienta1, "Acero" , 10.0f, 99, 5 ,fabricante[0]),
                new Herramienta (_nombreHerramienta2, "Madera" ,15.7f, 99, 8 , fabricante[1])
            };

            ApplicationUser usuario = new ApplicationUser("1", _nombreUsuario, "García", "email_prueba@gmail.com", "222222222");

            var oferta = new Oferta(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                tipoDirigidaOferta.Cliente,
                new List<OfertaItem>(),
                new Efectivo { Nombre = "Efectivo" },
                usuario
            );

            oferta.Items.Add(new OfertaItem(5, 9.5f, null, herramienta[0]));

            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.AddRange(usuario);
            _context.AddRange(oferta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> CasosDeUso_CrearOferta()
        {
            var ofertaNoItem = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = new List<CrearOfertaItemDTO>()
            };

            var ofertaItems = new List<CrearOfertaItemDTO>
            {
                new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 10 },
                new CrearOfertaItemDTO { HerramientaId = 2, PorcentajeDescuento = 15 }
            };

            var ofertaFromBeforeToday = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = ofertaItems
            };

            var ofertaToBeforeFrom = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = ofertaItems
            };

            var ofertaApplicationUser = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = "usuario_no_existe",
                Items = ofertaItems
            };

            var ofertaNoDisponible = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = new List<CrearOfertaItemDTO>
                {
                    new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 10 },
                    new CrearOfertaItemDTO { HerramientaId = 999, PorcentajeDescuento = 15 } // Herramienta no existente
                }
            };

            var ofertaMetodoPagoInvalido = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 999, // <-- ID que no existe
                nombreUsuario = _nombreUsuario,
                Items = ofertaItems
            };

            var ofertaTipoInvalido = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "TipoRaro", // <-- Valor que no es 'Cliente' ni 'Socio'
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = ofertaItems
            };

            var ofertaPorcentajeInvalido = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = new List<CrearOfertaItemDTO>
                {
                // El porcentaje 91 no es válido (debe ser <= 90)
                new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 91 }
                }
            };

            var ofertaPorcentajeCero = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = new List<CrearOfertaItemDTO> 
                {
                // El porcentaje 0 no es válido (debe ser > 0)
                new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 0 }
                
                }
            };

            var ofertaDeUnaSemana = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1,
                nombreUsuario = _nombreUsuario,
                Items = new List<CrearOfertaItemDTO>
                {
                new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 10 }

                }
            };

            var allTests = new List<object[]>
            {
                new object[] { ofertaNoItem, "La oferta debe incluir al menos una herramienta." },
                new object[] { ofertaFromBeforeToday, "La fecha de inicio debe ser posterior a hoy." },
                new object[] { ofertaToBeforeFrom, "La fecha final debe ser posterior a la fecha de inicio." },
                new object[] { ofertaApplicationUser, "El usuario no existe." },
                new object[] { ofertaNoDisponible, "La HerramientaId 999 no existe." },
                new object[] { ofertaMetodoPagoInvalido, "El MetodoPagoId 999 no existe." },
                new object[] { ofertaTipoInvalido, "El valor 'TipoRaro' no es válido. Use 'Socio', 'Cliente' o déjelo vacío."},
                // El mensaje de error del porcentaje puede variar según el nombre de tu herramienta
                new object[] { ofertaPorcentajeInvalido, "El porcentaje 91% para 'Herramienta1' no es válido. Debe estar entre 1 y 90." },
                new object[] { ofertaPorcentajeCero, "El porcentaje 0% para 'Herramienta1' no es válido. Debe estar entre 1 y 90." },
                new object[] { ofertaDeUnaSemana , "ERROR! La oferta debe durar al menos una semana" }
            };

            return allTests;

        }
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(CasosDeUso_CrearOferta))]
        public async Task Post_oferta_CrearOferta_Test(CrearOfertaDTO ofertaDTO, string mensajeEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);

            // Act
            var result = await controller.CrearOferta(ofertaDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(mensajeEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task Post_oferta_CrearOferta_Exitoso()
        {
            // 1. ARRANGE
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;
            var controller = new OfertasController(_context, logger);

            // Datos de entrada (Input)
            var ofertaItems = new List<CrearOfertaItemDTO>
            {
                new CrearOfertaItemDTO { HerramientaId = 1, PorcentajeDescuento = 10 },
                new CrearOfertaItemDTO { HerramientaId = 2, PorcentajeDescuento = 15 }
            };

            var ofertaDTO = new CrearOfertaDTO
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                FechaFinal = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                TipoDirigida = "Cliente",
                MetodoPagoId = 1, // Corresponde a "Efectivo" según tu Seed
                nombreUsuario = _nombreUsuario,
                Items = ofertaItems
            };

            // Construimos el objeto de RESPUESTA ESPERADA (Expected Output)
            // Aquí definimos qué debería devolver el controlador si todo sale bien.
            var expectedResponse = new OfertasParaDetalleDTO(
                ofertaDTO.FechaFinal,
                ofertaDTO.FechaInicio,
                DateOnly.FromDateTime(DateTime.UtcNow), // Fecha de creación (hoy)
                "Cliente",
                "Efectivo",
                new List<OfertaItemsDTO>(),
                _nombreUsuario
            );

            // Añadimos los items calculados que esperamos recibir
            // Item 1: Precio 10.0, Descuento 10% -> 9.0
            expectedResponse.Items.Add(new OfertaItemsDTO(
                _nombreHerramienta1, "Acero", _nombreFabricante1, 10.0f, 9.0f));

            // Item 2: Precio 15.7, Descuento 15% -> 13.345
            expectedResponse.Items.Add(new OfertaItemsDTO(
                _nombreHerramienta2, "Madera", _nombreFabricante2, 15.7f, 13.345f));


            // 2. ACT
            var result = await controller.CrearOferta(ofertaDTO);


            // 3. ASSERT (Respuesta HTTP)
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdOfertaDTO = Assert.IsType<OfertasParaDetalleDTO>(createdAtActionResult.Value);

            Assert.Equal(expectedResponse, createdOfertaDTO);


            // 4. ASSERT (Base de Datos)
            // no solo que el controlador devolvió el DTO correcto.
            var ofertaEnDB = await _context.Ofertas
                                    .Include(o => o.Usuario)
                                    .Include(o => o.MetodosPago)
                                    .Include(o => o.Items)
                                    .FirstOrDefaultAsync(o => o.Id == 2); // ID 2 porque la 1 se crea en el constructor

            Assert.NotNull(ofertaEnDB);
            Assert.Equal(ofertaDTO.FechaInicio, ofertaEnDB.FechaInicio);
            Assert.Equal(2, ofertaEnDB.Items.Count);

            // Verificamos un dato clave en BD (ej: precio final calculado guardado correctamente)
            var itemDb = ofertaEnDB.Items.First(i => i.HerramientaId == 1);
            Assert.Equal(9.0f, itemDb.PrecioFinal, 0.001f); // Usamos tolerancia para float
        }
    }
}
            

