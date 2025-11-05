using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class GetDetalleParaCompra_test : AppForSEII25264SqliteUT
    {
        public GetDetalleParaCompra_test()
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
                new Herramienta("Nombre - Herramienta1", "Material - Herramientas1y2", (float)10.99, 100, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Nombre - Herramienta2", "Material - Herramientas1y2", (float)2.99, 200, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta("Nombre - Herramienta3", "Material - Herramienta3", (float)3.99, 300, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
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

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleParaCompra_NotFound()
        {
            //Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            //Act
            var result = await controller.GetDetalleHerramientasParaCompra(999); // ID de compra que no existe (999)

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleParaCompra_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;
            var controller = new ComprasController(_context, logger);

            var expectedCompra = new ComprasParaDetalleDTO(
                "Fulanito",
                "De Tal",
                "DireccionEnvio - Compra1",
                (float)10.99,
                DateOnly.FromDateTime(DateTime.UtcNow),
                new List<CompraItemsDTO>()
            );
            expectedCompra.Items.Add(new CompraItemsDTO
            (
                100,
                "Nombre - Herramienta1",
                "Material - Herramientas1y2",
                (float)10.99,
                "Descripción - CompraItem1",
                1
            ));

            //Act
            var result = await controller.GetDetalleHerramientasParaCompra(1);

            //Assert 
            // 1. Comprueba que los objetos no sean nulos
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var compraDTOActual = Assert.IsType<ComprasParaDetalleDTO>(okResult.Value);

            // 2. Comprueba las propiedades simples (string, DateOnly, int, etc.)
            Assert.Equal(expectedCompra.Nombre, compraDTOActual.Nombre);
            Assert.Equal(expectedCompra.Apellidos, compraDTOActual.Apellidos);
            Assert.Equal(expectedCompra.DireccionEnvio, compraDTOActual.DireccionEnvio);
            Assert.Equal(expectedCompra.PrecioTotal, compraDTOActual.PrecioTotal);
            Assert.Equal(expectedCompra.FechaCompra, compraDTOActual.FechaCompra);

            // 3. Comprueba las listas o colecciones
            //    Primero, comprueba que tengan el mismo número de elementos
            Assert.Equal(expectedCompra.Items.Count, compraDTOActual.Items.Count);

            // 4. Comprueba los elementos DENTRO de las listas
            //    (En este test, sabes que solo hay un item, en la posición [0])
            var expectedItem = expectedCompra.Items[0];
            var actualItem = compraDTOActual.Items[0];

            Assert.Equal(expectedItem.NombreHerramienta, actualItem.NombreHerramienta);
            Assert.Equal(expectedItem.MaterialHerramienta, actualItem.MaterialHerramienta);
            Assert.Equal(expectedItem.PrecioHerramienta, actualItem.PrecioHerramienta);
            Assert.Equal(expectedItem.DescripcionHerramienta, actualItem.DescripcionHerramienta);
            Assert.Equal(expectedItem.CantidadHerramienta, actualItem.CantidadHerramienta);
        }
    }
}
