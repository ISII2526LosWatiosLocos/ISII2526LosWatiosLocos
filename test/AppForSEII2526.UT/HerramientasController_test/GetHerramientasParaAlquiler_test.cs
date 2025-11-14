using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit; // Para el Assert
using Xunit.Abstractions; 

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetHerramientasParaAlquiler_test : AppForSEII25264SqliteUT
    {
        public GetHerramientasParaAlquiler_test()
        // Entradas: FiltroNombre y FiltroMaterial
        // Salidas: Herramientas
        // Complejidad Ciclomatica = 4
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };
            var herramienta = new List<Herramienta>()
            {
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Martillo", "Acero", 15.5f, 99, 5, fabricante[0]),
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Destornillador", "Acero", 7.0f, 99, 3, fabricante[1]),
                new Herramienta(DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Taladro", "Plástico", 5.0f, 99, 10, fabricante[2]),
            };
            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaAlquilar_Ok()
        {
            // Datos esperados
            var herramientasDTO = new List<HerramientasParaAlquilarDTO>()
            {
                new HerramientasParaAlquilarDTO ( "Martillo", "Acero", "Herramientas SA", 15.5f),
                new HerramientasParaAlquilarDTO ( "Destornillador", "Acero", "Utensilios y Más", 7.0f),
                new HerramientasParaAlquilarDTO ( "Taladro", "Plástico", "Todo para Construcción", 5.0f)
            };

            // Casos de prueba
            var herramientasDTO_TC1 = new List<HerramientasParaAlquilarDTO> { herramientasDTO[0], herramientasDTO[1], herramientasDTO[2] };

            var herramientasDTO_TC2 = new List<HerramientasParaAlquilarDTO> { herramientasDTO[1] };

            var herramientasDTO_TC3 = new List<HerramientasParaAlquilarDTO> { herramientasDTO[2] };
            // Colección de todos los casos de prueba
            var allTest = new List<object[]> {
                new object[] {null, null, herramientasDTO_TC1 },
                new object[] { "Destornillador", null, herramientasDTO_TC2 },
                new object[] { null, "Plástico", herramientasDTO_TC3 }
            };

            return allTest;
        }
        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaAlquilar_Ok))]
        public async Task GetHerramientasParaAlquilar_Ok(string? nombre, string? material, List<HerramientasParaAlquilarDTO> expectedResult)
        {
            // Arrange
            var controller = new HerramientasController(_context, null);
            // Act
            var result = await controller.GetHerramientasParaAlquiler(nombre, material);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var herramientasDTOActual = Assert.IsType<List<HerramientasParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedResult, herramientasDTOActual);
        }


    }
}
