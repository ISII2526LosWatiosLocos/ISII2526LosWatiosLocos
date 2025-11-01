using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class GetDetalleParaOferta_test : AppForSEII25264SqliteUT
    {
        public GetDetalleParaOferta_test()
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
                new Herramienta("Martillo", "Acero", 15.5f, 5, fabricante[0]),
                new Herramienta("Destornillador", "Acero", 7.0f, 3, fabricante[1]),
                new Herramienta("Taladro", "Plástico", 5.0f, 10, fabricante[2]),
            };

            var ofertaItem = new List<OfertaItem>()
            {
                new OfertaItem(10, 13.95f, null, herramienta[0]),
                new OfertaItem(20, 5.6f, null, herramienta[1]),
                new OfertaItem(15, 4.25f, null, herramienta[2]),
            };

            var metodoPago = new Efectivo()
            {
                Nombre = "Efectivo"
            };

            var oferta = new Oferta(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                tipoDirigidaOferta.Cliente,
                ofertaItem,
                metodoPago
            );

            //Añadimos a la bbdd los datos de prueba
            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.AddRange(ofertaItem);
            _context.AddRange(metodoPago);
            _context.AddRange(oferta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDetalleParaOferta_Ok()
        {
            var ofertaItemsDTO = new List<OfertaItemsDTO>()
            {
                new OfertaItemsDTO ( "Martillo", "Acero", "Herramientas SA", 15.5f, 13.95f),
                new OfertaItemsDTO ( "Destornillador", "Acero", "Utensilios y Más", 7.0f, 5.6f),
                new OfertaItemsDTO ( "Taladro", "Plástico", "Todo para Construcción", 5.0f, 4.25f)
            };

            var ofertasParaDetalleDTO_TC1 = new OfertasParaDetalleDTO(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow),
                "Cliente",
                "Efectivo",
                new List<OfertaItemsDTO> { ofertaItemsDTO[0], ofertaItemsDTO[1], ofertaItemsDTO[2] }
            );

            var allTest = new List<object[]> {
                new object[] {1, ofertasParaDetalleDTO_TC1 }
            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDetalleParaOferta_Ok))]
        public async Task GetDetalleParaOferta_Ok(int ofertaId, OfertasParaDetalleDTO expectedDTO)
        {
            // Arrange
            var controller = new OfertasController(_context, null);
            // Act
            var actionResult = await controller.GetDetalleHerramientasParaOferta(ofertaId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualDTO = Assert.IsType<OfertasParaDetalleDTO>(okResult.Value);
            Assert.Equal(expectedDTO, actualDTO);


        }
    }
}
