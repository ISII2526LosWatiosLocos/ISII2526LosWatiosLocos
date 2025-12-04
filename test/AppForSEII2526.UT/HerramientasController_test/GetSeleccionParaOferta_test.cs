using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.OfertasDTOs;
using AppForSEII2526.UT;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetSeleccionParaOferta_test : AppForSEII25264SqliteUT
    {
        public GetSeleccionParaOferta_test()
        {
            //Datos de prueba
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };

            var herramienta = new List<Herramienta>()
            {
                new Herramienta("Martillo", "Acero", 15.5f, 99, 5, fabricante[0]),
                new Herramienta("Destornillador", "Acero", 7.0f, 99, 3, fabricante[1]),
                new Herramienta("Taladro", "Plástico", 5.0f, 99, 10, fabricante[2]),
            };



            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetHerramientasParaOfertar_Ok()
        {
            // Datos esperados
            var herramientasDTO = new List<HerramientasParaOfertarDTO>()
            {
                new HerramientasParaOfertarDTO (1, "Martillo", "Acero", "Herramientas SA", 15.5f),
                new HerramientasParaOfertarDTO (2, "Destornillador", "Acero", "Utensilios y Más", 7.0f),
                new HerramientasParaOfertarDTO (3, "Taladro", "Plástico", "Todo para Construcción", 5.0f)
            };

            // Casos de prueba
            var herramientasDTO_TC1 = new List<HerramientasParaOfertarDTO> { herramientasDTO[0], herramientasDTO[1], herramientasDTO[2] };

            var herramientasDTO_TC2 = new List<HerramientasParaOfertarDTO> { herramientasDTO[1] };

            var herramientasDTO_TC3 = new List<HerramientasParaOfertarDTO> { herramientasDTO[2] };
            // Colección de todos los casos de prueba
            var allTest = new List<object[]> {
                new object[] {null, null, herramientasDTO_TC1 },
                new object[] { "Utensilios y Más", null, herramientasDTO_TC2 },
                new object[] { null, 5.0f, herramientasDTO_TC3 }
            };

            return allTest;
        }
        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasParaOfertar_Ok))]
        public async Task GetHerramientasParaOfertar_Ok(string? fabricante, float? precioMaximo, List<HerramientasParaOfertarDTO> expectedResult)
        {
            // Arrange
            var controller = new HerramientasController(_context, null);
            // Act
            var result = await controller.GetHerramientasParaOferta(fabricante, precioMaximo);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var herramientasDTOActual = Assert.IsType<List<HerramientasParaOfertarDTO>>(okResult.Value);
            Assert.Equal(expectedResult, herramientasDTOActual);
        }
    }
}

    



        
    

