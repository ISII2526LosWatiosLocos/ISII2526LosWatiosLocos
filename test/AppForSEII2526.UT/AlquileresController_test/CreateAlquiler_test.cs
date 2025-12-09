using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.AlquileresDTOs;
using AppForSEII2526.API.Models;

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
                1,
                "Pisos Picadooos",
                "123456722",
                "abc@hello.com",
                alquilerItems
                );

            var alquilerApplicationUser = new CrearAlquilerDTO(
                "usuario_no_existe",
                "apellido",
                1,
                "Calle Senorio de la Sal",
                "123456722",
                "abc@hello.com",
                alquilerItems);

            var alquilerNoDisponible = new CrearAlquilerDTO(
                _nombreUsuario,
                _apellidoUsuario,
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
                999, // Id que no existe
                "Calle 24",
                "123456722",
                "abc@hello.com",
                alquilerItems
            );

            var allTests = new List<object[]>
            {
                new object[] { alquilerNoItem, "El alquiler debe incluir al menos una herramienta." },
                // Match controller message exactly (case and included name)
                new object[] { alquilerDireccionInvalida, "¡Error! La dirección de envío debe empezar por la palabra Calle" },
                new object[] { alquilerApplicationUser, $"El Usuario {alquilerApplicationUser.Nombre} {alquilerApplicationUser.Apellidos} no existe." },
                new object[] { alquilerNoDisponible, "La HerramientaId 4 no existe." },
                new object[] { alquilerMetodoPagoInvalido, "El MetodoPagoId 999 no existe." },
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
                new AlquilarItemsDTO(1,10),
                new AlquilarItemsDTO (2, 15)
            };
            var alquilerDTO = new CrearAlquilerDTO(

                _nombreUsuario,
                _apellidoUsuario,
                1,
                "Calle Caserio Colesterol",
                "123456722",
                "abc@hello.com",
                alquilerItems
            );

            // --- Pre-cálculo de precios esperados ---
           
            float expectedPrice1 = 10.0f;
            // Herramienta 2: 15.7f * (1 - 0.15) = 13.345f
            float expectedPrice2 = 30.0f;

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);


            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdAlquilerDTO = Assert.IsType<AlquileresParaDetalleDTO>(createdAtActionResult.Value);

            Assert.Equal(alquilerDTO.Nombre, createdAlquilerDTO.Nombre);
            Assert.Equal(alquilerDTO.Apellidos, createdAlquilerDTO.Apellidos);
            Assert.Equal(alquilerDTO.Direccion, createdAlquilerDTO.Direccion);

            // --- 3. Comprobar los items del DTO (¡Importante!) ---
            var item1DTO = createdAlquilerDTO.Items.FirstOrDefault(i => i.NombreItem == _nombreHerramienta1);
            Assert.NotNull(item1DTO);
            Assert.Equal(10.0f, item1DTO.PrecioItem);
            var item2DTO = createdAlquilerDTO.Items.FirstOrDefault(i => i.NombreItem == _nombreHerramienta2);
            Assert.NotNull(item2DTO);
            Assert.Equal(15.7f, item2DTO.PrecioItem);

            // --- 4. (¡EL MÁS IMPORTANTE!) Comprobar la Base de Datos ---
            // Tu constructor ya creó el alquiler ID=1. Esta nueva debe ser la ID=2.
            var alquilerEnDB = await _context.Alquileres
                                        .Include(o => o.Usuario)
                                        .Include(o => o.MetodoPago)
                                        .Include(o => o.AlquilarItems)
                                        .FirstOrDefaultAsync(o => o.Id == 2); // Busca la nueva oferta

            Assert.NotNull(alquilerEnDB);
            // Assert.Equal(alquilerDTO., alquilerEnDB.FechaInicio); fechas no??
            Assert.Equal(_nombreUsuario, alquilerEnDB.Usuario.Nombre); // Comprueba el usuario enlazado
            Assert.Equal("Efectivo", alquilerEnDB.MetodoPago.Nombre); // Comprueba el método de pago
            Assert.Equal(2, alquilerEnDB.AlquilarItems.Count); // Comprueba el número de items

            // Comprobar que el precio final se guardó bien en la BBDD
            var item1EnDB = alquilerEnDB.AlquilarItems.FirstOrDefault(i => i.HerramientaId == 1);
            Assert.NotNull(item1EnDB);
            Assert.Equal(expectedPrice1, item1EnDB.Precio);


        }
    }
}