using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.UT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetSeleccionReparacion_test : AppForSEII25264SqliteUT
    {
        public GetSeleccionReparacion_test()
        {
            var herramienta = new List<Herramienta>()
            {
                new Herramienta(1, "Martillo", "Acero", 15.5f, 5, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(2, "Destornillador", "Acero", 7.0f, 3, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(3, "Taladro", "Plástico", 45.0f, 10, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(4, "Sierra", "Acero", 30.0f, 7, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null),
                new Herramienta(5, "Llave inglesa", "Acero", 20.0f, 4, new List<CompraItem>(), new List<AlquilarItem>(), new List<OfertaItem>(), new List<ReparaciónItem>(), null)
            };

            var fabricante = new List<Fabricante>()
            {
               
                new Fabricante("Herramientas SA"),
                new Fabricante("Utensilios y Más"),
                new Fabricante("Todo para Construcción")
            };

           
            ApplicationUser usuario = new ApplicationUser("Fulanito", "De Tal", "fulanitodetal@uclm.es", "111222333", new List<Compra>(), new List<Reparación>(), new List<Alquiler>());

            var Reparacion = new Reparación(
    DateOnly.FromDateTime(DateTime.Now),
    DateOnly.FromDateTime(DateTime.Now),
    125.78f,
    new Efectivo(),
    new List<ReparaciónItem>()
);

            var ReparacionItems = new List<ReparaciónItem>()
            {
                new ReparaciónItem(10, "Reparar sierra", 56.89f,Reparacion, herramienta[0]),
                new ReparaciónItem(30, "Repara radial",70.25f, Reparacion, herramienta[2]),
                new ReparaciónItem(20, "Reparar serrucho",15.67f, Reparacion, herramienta[4])

            };

 var metodoPago = new Efectivo()
            {
                Nombre = "Efectivo"
            }; 


            Reparacion.ReparaciónItems= ReparacionItems;

            // 2.
            _context.AddRange(fabricante); 
            _context.Add(usuario);
            _context.Add(Reparacion);
         
            _context.AddRange(herramienta);
            _context.AddRange(ReparacionItems);
            _context.AddRange(metodoPago);
         
            //Guardar los cambio s en la memoria de la database 
        _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDetalleParaTReparacion_Ok()
        {
            var ReparacionesItemsDTO = new List<ReparacionesItemDTO>()
            {
                new ReparacionesItemDTO ( "Martillo", "de bola", 15, 40.5f),
                new ReparacionesItemDTO ( "Destornillador", "con cabeza en forma de cruz", 7, 25.6f),
                new ReparacionesItemDTO ( "Taladro", "de tipo percurtor", 2, 34.25f)
            };

            var ReparacionesParaDetalleDTO_TC1 = new ReparacionesDTO(
                "Matías", "Pérez López ",
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                DateOnly.FromDateTime(DateTime.UtcNow),
                235.57f,
                new List<ReparacionesItemDTO> { ReparacionesItemsDTO[0],ReparacionesItemsDTO[1], ReparacionesItemsDTO[2] }
            );

            var allTest = new List<object[]> {
                new object[] {1, ReparacionesParaDetalleDTO_TC1 }
            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDetalleParaTReparacion_Ok))]
        public async Task GetDetalleParaReparacion_Ok(int reparacionId,ReparacionesDTO expectedDTO)
        {
            // Arrange
            var controller = new ReparacionesController(_context, null);
            // Act
            var actionResult = await controller.GetDetalleHerramientasParaReparación(reparacionId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualDTO = Assert.IsType<ReparacionesDTO>(okResult.Value);
            Assert.Equal(expectedDTO, actualDTO);


        }
    }
    }
