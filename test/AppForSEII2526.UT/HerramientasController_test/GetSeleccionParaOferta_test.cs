/*using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.UT;
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
                new Fabricante(1, "Herramientas SA", herramienta)
            };

            ApplicationUser usuario = new ApplicationUser();

            var oferta = new Oferta(DateTime.Now.AddDays(30), DateTime.Now, DateTime.Now, tipoDirigidaOferta.Socio, new List<OfertaItem>(), new Efectivo());

            var ofertaItems = new List<OfertaItem>()
            {
                new OfertaItem(10, 10, oferta, herramienta[0]),
                new OfertaItem(15, 38.25f, oferta, herramienta[2]),
                new OfertaItem(20, 16, oferta, herramienta[4])

            };
            oferta.Items = ofertaItems;

            // 2. Add the entities to the context
            _context.AddRange(fabricante); // This will add the 'herramienta' list due to the relationship
            _context.Add(usuario);
            _context.Add(oferta); // This will add the 'ofertaItems' list due to the relationship

            // 3. Save the changes to the in-memory database
            _context.SaveChanges();

            // --- Missing Code Ends Here ---
        }

        /*public static IEnumerable<object[]> GetSeleccionParaOferta_Data()
        {
            yield return new object[]
            {
                1, // ofertaId
                new List<HerramientasParaOfertarDTO>() // expectedResult
                {
                    new HerramientasParaOfertarDTO {Nombre = "Martillo", Material = "Acero", PrecioAlquiler = 15.5f, Stock = 5, CantidadEnOferta = 10 },
                    new HerramientasParaOfertarDTO { Id = 2, Nombre = "Destornillador", Material = "Acero", PrecioAlquiler = 7.0f, Stock = 3, CantidadEnOferta = 0 },
                    new HerramientasParaOfertarDTO { Id = 3, Nombre = "Taladro", Material = "Plástico", PrecioAlquiler = 45.0f, Stock = 10, CantidadEnOferta = 15 },
                    new HerramientasParaOfertarDTO { Id = 4, Nombre = "Sierra", Material = "Acero", PrecioAlquiler = 30.0f, Stock = 7, CantidadEnOferta = 0 },
                    new HerramientasParaOfertarDTO { Id = 5, Nombre = "Llave inglesa", Material = "Acero", PrecioAlquiler = 20.0f, Stock = 4, CantidadEnOferta = 20 }
                }
            };
        }
        

    }
}*/
            



        
    

