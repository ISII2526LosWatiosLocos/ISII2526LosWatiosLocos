using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.UT;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetSeleccionParaReparacion_test : AppForSEII25264SqliteUT
    {
        public GetSeleccionParaReparacion_test()
        {
            // Datos de prueba
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };

            var herramienta = new List<Herramienta>()
            {
                new Herramienta("Martillo", "Acero", 15.5f, 99, 5, fabricante[0]),   // 5 días reparación
                new Herramienta("Destornillador", "Acero", 7.0f, 99, 3, fabricante[1]), // 3 días reparación
                new Herramienta("Taladro", "Plástico", 5.0f, 99, 10, fabricante[2]) // 10 días reparación
            };

            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaReparacion_Ok()
        {
            // Datos esperados
            var herramientasDTO = new List<HerramientasParaReparaciónDTO>()
            {
                new HerramientasParaReparaciónDTO("Martillo", "Acero", "Herramientas SA", 15.5f, 5),
                new HerramientasParaReparaciónDTO("Destornillador", "Acero", "Utensilios y Más", 7.0f, 3),
                new  HerramientasParaReparaciónDTO("Taladro", "Plástico", "Todo para Construcción", 5.0f, 10)
            };

            // Caso 1: sin filtros → devuelve todas
            var herramientasDTO_TC1 = new List<HerramientasParaReparaciónDTO> { herramientasDTO[0], herramientasDTO[1], herramientasDTO[2] };

            // Caso 2: filtrar por nombre
            var herramientasDTO_TC2 = new List<HerramientasParaReparaciónDTO> { herramientasDTO[1] }; // "Destornillador"

            // Caso 3: filtrar por tiempo máximo (≤ 5 días)
            var herramientasDTO_TC3 = new List<HerramientasParaReparaciónDTO> { herramientasDTO[0], herramientasDTO[1] };

            return new List<object[]>
            {
                new object[] { null, null,  herramientasDTO_TC1 },
                new object[] { "Utensilios y Más", null, herramientasDTO_TC2 },
                new object[] { null, 5, herramientasDTO_TC3 }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaReparacion_Ok))]
        public async Task GetHerramientasParaReparacion_Ok(string? nombre, int? tiempoMaximo, List<HerramientasParaReparaciónDTO> expectedResult)
        {
          
            var controller = new HerramientasController(_context, null);

        
            var result = await controller.GetHerramientasParaReparación(nombre, tiempoMaximo);

        
            var okResult = Assert.IsType<OkObjectResult>(result);

            var herramientasDTOActual = Assert.IsType<List<HerramientasParaReparaciónDTO>>(okResult.Value);

            Assert.Equal(expectedResult, herramientasDTOActual);
        }
    }
}
