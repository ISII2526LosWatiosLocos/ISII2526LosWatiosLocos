using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.UT;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetSeleccionParaCompra_test : AppForSEII25264SqliteUT
    {
        public GetSeleccionParaCompra_test()
        {
            // Seed de datos en la BBDD de prueba:

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

            var Items = new List<CompraItem>(){
                new CompraItem(1, "Descripción - CompraItem1", (float)1.99, null, null),
                new CompraItem(2, "Descripción - CompraItem2", (float)2.99, null, null),
                new CompraItem(3, "Descripción - CompraItem3", (float)3.99, null, null),
            };

            var Compras = new List<Compra>() {
                new Compra("DireccionEnvio - Compra1", DateOnly.FromDateTime(DateTime.UtcNow), (float)100.99, new List<CompraItem>(), new Efectivo(), null),
                new Compra("DireccionEnvio - Compra2", DateOnly.FromDateTime(DateTime.UtcNow), (float)200.99, new List<CompraItem>(), new TarjetaCredito(), null),
            };

            ApplicationUser Usuario = new ApplicationUser("Fulanito", "De Tal", "fulanitodetal@uclm.es", "111222333", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

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
            _context.AddRange(Fabricantes);
            _context.AddRange(Herramientas);
            _context.AddRange(Compras);
            _context.AddRange(Items);
            _context.Users.Add(Usuario);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaComprar_Ok()
        {
            // Datos esperados
            var herramientasDTO = new List<HerramientasParaComprarDTO>()
            {
                new HerramientasParaComprarDTO("Nombre - Herramienta1", "Material - Herramientas1y2", "Nombre - Fabricante1", 10.99f),
                new HerramientasParaComprarDTO("Nombre - Herramienta2", "Material - Herramientas1y2", "Nombre - Fabricante2", 2.99f),
                new HerramientasParaComprarDTO("Nombre - Herramienta3", "Material - Herramienta3", "Nombre - Fabricante2", 3.99f)
            };

            // Casos de prueba (los defino, especificando qué herramientasDTO deben devolver según los filtros que defina acontinuación en allTest)
            var herramientasDTO_TC1 = new List<HerramientasParaComprarDTO> { herramientasDTO[0], herramientasDTO[1], herramientasDTO[2] };

            var herramientasDTO_TC2 = new List<HerramientasParaComprarDTO> { herramientasDTO[0], herramientasDTO[1] };

            var herramientasDTO_TC3 = new List<HerramientasParaComprarDTO> { herramientasDTO[1], herramientasDTO[2] };
            // Colección de todos los casos de prueba (los filtros deben proporcionar los herramientasDTO que haya especificado arriba según cada uno)
            var allTest = new List<object[]> {
                new object[] { null, null, herramientasDTO_TC1 }, // devuelve todas
                new object[] { "Material - Herramientas1y2", null, herramientasDTO_TC2 }, // devuelve las dos con ese material
                new object[] { null, 3.99f, herramientasDTO_TC3 } // devuelve las de precio igual o inferior a ese
            };

            return allTest;
        }
        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaComprar_Ok))]
        public async Task GetHerramientasParaComprar_Ok(string? material, float? precio, List<HerramientasParaComprarDTO> expectedResult)
        {
                // ARRANGE
            var controller = new HerramientasController(_context, null);

                // ACT
            var result = await controller.GetHerramientasParaCompra(material, precio);

                // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientasDTOActual = Assert.IsType<List<HerramientasParaComprarDTO>>(okResult.Value);
            // comprueba cardinalidad
            Assert.Equal(expectedResult.Count, herramientasDTOActual.Count);
            // compara por propiedades, sin depender del orden
            var expectedOrdered = expectedResult.OrderBy(h => h.Nombre).ToList();
            var actualOrdered = herramientasDTOActual.OrderBy(h => h.Nombre).ToList();

            for (int i = 0; i < expectedOrdered.Count; i++)
            {
                var exp = expectedOrdered[i];
                var act = actualOrdered[i];

                Assert.Equal(exp.Nombre, act.Nombre);
                Assert.Equal(exp.Material, act.Material);
                Assert.Equal(exp.Fabricante, act.Fabricante);
                Assert.True(Math.Abs(exp.Precio - act.Precio) < 0.001f, $"Precio esperado {exp.Precio} pero fue {act.Precio} en {act.Nombre}");
            }
        }
    }
}