using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.AlquileresDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AppForSEII2526.UT.AlquileresController_test
{
    public class CreateAlquiler_test : AppForSEII25264SqliteUT
    {
        private const string _nombreUsuario = "usuario1";
        private const string _apellidoUsuario = "perez";
        private const string _nombreHerramienta1 = "Herramienta1";
        private const string _nombreHerramienta2 = "Herramienta2";
        private const string _nombreFabricante1 = "Fabricante1";
        private const string _nombreFabricante2 = "Fabricante2";

        public CreateAlquiler_test()
        {
            var fabricante = new List<Fabricante>
            {
                new Fabricante (_nombreFabricante1),
                new Fabricante (_nombreFabricante2)
            };

            var herramienta = new List<Herramienta>
            {
                new Herramienta (_nombreHerramienta1, "Acero" , 10.0f, 5 , 99, fabricante[0]),
                new Herramienta (_nombreHerramienta2, "Madera" ,15.7f, 8 , 99, fabricante[1])
            };

            ApplicationUser usuario = new ApplicationUser("83", _nombreUsuario, _apellidoUsuario, "email_prueba@gmail.com", "222222222");

            var alquiler = new Alquiler(
                "Calle Alameda Aullante",
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                15.7f,
                new List<AlquilarItem>(),
                new Efectivo { Nombre = "Efectivo" },
                usuario
            );

            alquiler.AlquilarItems.Add(new AlquilarItem(9.5f, 2, null, herramienta[0]));

            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.AddRange(usuario);
            _context.AddRange(alquiler);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> CasosDeUso_CrearAlquiler()
        {
            var alquilerNoItem = new CrearAlquilerDTO(
                _nombreUsuario, 
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Calle Balsa Botin", 
                "123456722", 
                "abc@hello.com", 
                new List<AlquilarItemsDTO>());


            var alquilerItems = new List<AlquilarItemsDTO>
            {
                new AlquilarItemsDTO (4, 10), // id, cantidad
                new AlquilarItemsDTO(7, 15)
            };

            var alquilerDireccionInvalida = new CrearAlquilerDTO(
                _nombreUsuario,
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Pisos Picadooos",
                "123456722",
                "abc@hello.com",
                alquilerItems
                );

            var alquilerApplicationUser = new CrearAlquilerDTO(
                "usuario_no_existe",
                "apellido",
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Calle Senorio de la Sal",
                "123456722",
                "abc@hello.com",
                alquilerItems);

            var alquilerNoDisponible = new CrearAlquilerDTO(
                _nombreUsuario,
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Calle Soto Solitario",
                "123456722",
                "abc@hello.com",
                new List<AlquilarItemsDTO>
                                {
                                new AlquilarItemsDTO (4, 10), // id, cantidad
                                new AlquilarItemsDTO(7, 15)
                                }
            );


            var alquilerMetodoPagoInvalido = new CrearAlquilerDTO(
                _nombreUsuario,
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                999, // Id que no existe
                "Calle 24",
                "123456722",
                "abc@hello.com",
                alquilerItems
            );

            var alquilerItemSinCantidad = new CrearAlquilerDTO(
                _nombreUsuario,
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Calle Falsa 123",
                "123456722",
                "abc@hello.com",
                new List<AlquilarItemsDTO>
                {
                    new AlquilarItemsDTO(4, 0) // Cantidad 0 no válida
                }
            );

            var allTests = new List<object[]>
            {
                new object[] { alquilerNoItem, "El alquiler debe incluir al menos una herramienta." },
                // Match controller message exactly (case and included name)
                new object[] { alquilerDireccionInvalida, "¡Error! La dirección de envío debe empezar por la palabra Calle" },
                new object[] { alquilerApplicationUser, $"El Usuario {alquilerApplicationUser.Nombre} {alquilerApplicationUser.Apellidos} no existe." },
                new object[] { alquilerNoDisponible, "La HerramientaId 4 no existe." },
                new object[] { alquilerMetodoPagoInvalido, "El MetodoPagoId 999 no existe." },
                new object[] { alquilerItemSinCantidad, "La cantidad de cada herramienta debe ser mayor que cero." }
            };
            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(CasosDeUso_CrearAlquiler))]
        public async Task Post_Alquiler_CrearAlquiler_Test(CrearAlquilerDTO alquilerDTO, string mensajeEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;

            var controller = new AlquileresController(_context, logger);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(mensajeEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task Post_alquiler_CrearAlquiler_Exitoso()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;

            var controller = new AlquileresController(_context, logger);

            var alquilerItems = new List<AlquilarItemsDTO>
            {
                new AlquilarItemsDTO(1,1),
                new AlquilarItemsDTO (2, 2)
            };
            var alquilerDTO = new CrearAlquilerDTO(

                _nombreUsuario,
                _apellidoUsuario,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                1,
                "Calle Caserio Colesterol",
                "123456722",
                "abc@hello.com",
                alquilerItems
            );

            // --- Pre-cálculo de precios esperados ---
           
            float expectedPrice1 = 10.0f;

            float expectedPrice2 = 31.4f;

            var expectedResponse = new AlquileresParaDetalleDTO(
                alquilerDTO.Nombre,
                alquilerDTO.Apellidos,
                alquilerDTO.Direccion,
                DateOnly.FromDateTime(DateTime.UtcNow),
                expectedPrice1 + expectedPrice2,
                DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                new List<AlquilarItemsDTO>()
                );

            // Añadir items esperados
            expectedResponse.Items.Add(new AlquilarItemsDTO(_nombreHerramienta1, "Acero", 10.0f,1));
            expectedResponse.Items.Add(new AlquilarItemsDTO(_nombreHerramienta2, "Madera", 15.7f,2));

            // --- ACT
            var result = await controller.CreateAlquiler(alquilerDTO);


            // 3. ASSERT (Respuesta HTTP)
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdAlquilerDTO = Assert.IsType<AlquileresParaDetalleDTO>(createdAtActionResult.Value);

            Assert.Equal(expectedResponse, createdAlquilerDTO);
            
            // 4. ASSERT (Base de Datos)
            // El constructor ya creó el alquiler ID=1. Esta nueva debe ser la ID=2.
            var alquilerEnDB = await _context.Alquileres
                                        .Include(o => o.Usuario)
                                        .Include(o => o.MetodoPago)
                                        .Include(o => o.AlquilarItems)
                                        .FirstOrDefaultAsync(o => o.Id == 2); // Busca la nueva oferta

            Assert.NotNull(alquilerEnDB);
            Assert.Equal(2, alquilerEnDB.AlquilarItems.Count);

            // Verificamos un dato clave en BD (ej: precio final calculado guardado correctamente)
            var itemDb = alquilerEnDB.AlquilarItems.First(i => i.HerramientaId == 1);
            Assert.Equal(expectedPrice1, itemDb.Precio, 0.001f); // Usamos tolerancia para float
        }
    }
}