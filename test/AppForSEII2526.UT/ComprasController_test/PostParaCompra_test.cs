using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class PostParaCompra_test : AppForSEII25264SqliteUT
    {
        public PostParaCompra_test()
        {
            // Seed de datos en la BBDD de prueba (sigo el orden de dbo.Global.data.sql):

            ApplicationUser Usuario = new ApplicationUser("Fulanito", "De Tal", "fulanitodetal@uclm.es", "111222333", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

            var Fabricantes = new List<Fabricante>()
            {
                new Fabricante("Nombre - Fabricante1", new List<Herramienta>()),
                new Fabricante("Nombre - Fabricante2", new List<Herramienta>()),
            };

            var Herramientas = new List<Herramienta>()
            {
                new Herramienta("Nombre - Herramienta1", "Material - Herramientas1y2", (float)10.99, 1, 100, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Nombre - Herramienta2", "Material - Herramientas1y2", (float)2.99, 2, 200, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Nombre - Herramienta3", "Material - Herramienta3", (float)3.99, 3, 300, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
            };

            // El nombre de un método de pago no puede ser null en la bbdd
            Efectivo efectivo = new Efectivo();
            efectivo.Nombre = "Efectivo";
            TarjetaCredito tarjetaCredito = new TarjetaCredito();
            tarjetaCredito.Nombre = "TarjetaCredito";

            var Compras = new List<Compra>() {
                new Compra("DireccionEnvio - Compra1", DateOnly.FromDateTime(DateTime.UtcNow), (float)10.99, new List<CompraItem>(), efectivo, null),
                new Compra("DireccionEnvio - Compra2", DateOnly.FromDateTime(DateTime.UtcNow), (float)6.98, new List<CompraItem>(), tarjetaCredito, null),
            };

            var Items = new List<CompraItem>(){
                new CompraItem(1, "Descripción - CompraItem1", (float)10.99, null, null),
                new CompraItem(2, "Descripción - CompraItem2", (float)2.99, null, null),
                new CompraItem(3, "Descripción - CompraItem3", (float)3.99, null, null),
            };

            // Entrelazar los datos:

            // Asigno herramientas a los fabricantes
            Fabricantes[0].Herramientas.Add(Herramientas[0]);
            Fabricantes[1].Herramientas.Add(Herramientas[1]);
            Fabricantes[1].Herramientas.Add(Herramientas[2]);

            // Asigno items y fabricantes a las herramientas
            Herramientas[0].CompraItems.Add(Items[0]);
            Herramientas[1].CompraItems.Add(Items[1]);
            Herramientas[2].CompraItems.Add(Items[2]);
            Herramientas[0].Fabricante = Fabricantes[0];
            Herramientas[1].Fabricante = Fabricantes[1];
            Herramientas[2].Fabricante = Fabricantes[1];


            // Asigno herramientas y las compras a los items
            Items[0].Herramienta = Herramientas[0];
            Items[1].Herramienta = Herramientas[1];
            Items[2].Herramienta = Herramientas[2];
            Items[0].Compra = Compras[0];
            Items[1].Compra = Compras[1];
            Items[2].Compra = Compras[1];

            // Asigno items y el usuario a las compras
            Compras[0].CompraItems.Add(Items[0]);
            Compras[1].CompraItems.Add(Items[1]);
            Compras[1].CompraItems.Add(Items[2]);
            Compras[0].Usuario = Usuario;
            Compras[1].Usuario = Usuario;

            // Añado los datos a la BBDD
            _context.Users.Add(Usuario);
            _context.AddRange(Fabricantes);
            _context.AddRange(Herramientas);
            _context.AddRange(Compras);
            _context.AddRange(Items);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> CasosDeUso_CrearCompra()
        {
            var compraItems = new List<CompraItemsDTO> // Los valores coinciden con lo guardado anteriormente en la BBDD
            {
                new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    3
                )
            };

            var compraSinUsuario = new CrearCompraDTO
            {
                Nombre = "Watio", // Usuario no existe
                Apellidos = "Loco",
                MetodoPagoId = 1,
                Items = compraItems,
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraMetodoPagoInvalido = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 999, // <-- ID que no existe
                Items = compraItems,
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraNoItem = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>(), // Lista vacía
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraSinDireccionEnvio = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = compraItems,
                DireccionEnvio = "", // String vacío o null
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemDescripcionNula = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                              //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    null, // Descripción nula
                    1 // Cantidad no igual a 3
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemCantidadCero = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    0 // Cantidad cero
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemCantidadNegativa = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    -3 // Cantidad negativa
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemCantidadMayorQueStockConUnSoloItem = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    4 // Cantidad mayor que Stock
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemCantidadMayorQueStockConVariosItems = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    3
                ),
                new CompraItemsDTO( // Duplico este item para que la Cantidad total sea 6 frente al Stock que sigue siendo 3
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    3
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var compraItemDescripcionNulaCantidadIgualATres = new CrearCompraDTO
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = new List<CompraItemsDTO>
                {
                    new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    null, // Descripción nula
                    3 // Cantidad es igual a 3
                )
                },
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            var allTests = new List<object[]>
            {
                new object[] { compraSinUsuario, "El usuario no existe." },
                new object[] { compraMetodoPagoInvalido, "El MetodoPagoId 999 no existe." },
                new object[] { compraNoItem, "La compra debe incluir al menos una herramienta." },
                new object[] { compraSinDireccionEnvio, "La compra debe tener una dirección de envío." },
                // Los mensajes de error de las herramientas pueden variar según el nombre de ésta:
                new object[] { compraItemDescripcionNula, "La herramienta Nombre - Herramienta3 no tiene descipción." },
                new object[] { compraItemCantidadCero, "La herramienta Nombre - Herramienta3 tiene cantidad cero." },
                new object[] { compraItemCantidadNegativa, "La herramienta Nombre - Herramienta3 tiene cantidad negativa." },
                // Los mensajes de error de las herramientas pueden variar según el nombre de ésta y de la cantidad de sus items y stock total de las herramientas:
                new object[] { compraItemCantidadMayorQueStockConUnSoloItem, "La herramienta Nombre - Herramienta3 tiene stock insuficiente: 3 < 4." }, // Stock < Cantidad
                new object[] { compraItemCantidadMayorQueStockConVariosItems, "La herramienta Nombre - Herramienta3 tiene stock insuficiente: 3 < 6." }, // Stock < Cantidad
                // MODIFICACIÓN
                new object[] { compraItemDescripcionNulaCantidadIgualATres, "¡Error! Estás comprando demasiadas herramientas sin descripción." }
            };

            return allTests;

        }
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(CasosDeUso_CrearCompra))]
        public async Task PostParaCompra_CrearCompra_Test(CrearCompraDTO compraDTO, string mensajeEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(mensajeEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task PostParaCompra_CrearCompra_Exitoso()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            var compraItems = new List<CompraItemsDTO> // Los valores coinciden con lo guardado anteriormente en la BBDD, uso el constructor completo pq no puede tener nulls.
            {
                new CompraItemsDTO(
                    1,                              //IdHerramienta
                    "Nombre - Herramienta1",        //NombreHerramienta
                    "Material - Herramienta1",      //PrecioHerramienta
                    10.99f,                         //PrecioHerramienta
                    "Descripción - CompraItem1",    //DescripciónHerramienta
                    1                               //CantidadHerramienta
                ),
                new CompraItemsDTO(
                    2,
                    "Nombre - Herramienta2",
                    "Material - Herramienta2",
                    2.99f,
                    "Descripción - CompraItem2",
                    2
                ),
                new CompraItemsDTO(
                    3,
                    "Nombre - Herramienta3",
                    "Material - Herramienta3",
                    3.99f,
                    "Descripción - CompraItem3",
                    3
                )
            };

            var compraDTO = new CrearCompraDTO // Compra sin errores
            {
                Nombre = "Fulanito",
                Apellidos = "De Tal",
                MetodoPagoId = 1,
                Items = compraItems,
                DireccionEnvio = "DireccionEnvio - Compra1",
                CorreoElectronico = "fulanitodetal@uclm.es",
                NumeroTelefono = "111222333"
            };

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCompraDTO = Assert.IsType<ComprasParaDetalleDTO>(createdAtActionResult.Value);
            Assert.Equal(compraDTO.Nombre, createdCompraDTO.Nombre);
            Assert.Equal(compraDTO.Apellidos, createdCompraDTO.Apellidos);
            Assert.Equal(compraDTO.DireccionEnvio, createdCompraDTO.DireccionEnvio);
            Assert.Equal(compraDTO.Items.Count, createdCompraDTO.Items.Count);

            // --- 3. Comprobar los items del DTO (¡Importante!) ---
            var item1DTO = createdCompraDTO.Items.FirstOrDefault(i => i.NombreHerramienta == "Nombre - Herramienta1");
            Assert.NotNull(item1DTO);
            Assert.InRange(item1DTO.PrecioHerramienta, 10.989f, 10.991f); // Añadimos un pequeño rango de tolerancia para que no pegue el petardazo

            var item2DTO = createdCompraDTO.Items.FirstOrDefault(i => i.NombreHerramienta == "Nombre - Herramienta2");
            Assert.NotNull(item2DTO);
            Assert.InRange(item2DTO.PrecioHerramienta, 2.989f, 2.991f); // Añadimos un pequeño rango de tolerancia para que no pegue el petardazo
        }
    }
}
            

