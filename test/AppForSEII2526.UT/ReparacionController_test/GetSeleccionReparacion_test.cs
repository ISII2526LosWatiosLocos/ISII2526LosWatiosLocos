/*using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
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
                new Fabricante(1, "Herramientas SA", herramienta)
            };

            ApplicationUser usuario = new ApplicationUser();

            var Reparacion = new Reparación(DateTime.Now, DateTime.Now,125.78f, new Efectivo(), new List<ReparaciónItem>() );

            var ReparacionItems = new List<ReparaciónItem>()
            {
                new ReparaciónItem(10, "Reparar sierra", 56.89f,Reparacion, herramienta[0]),
                new ReparaciónItem(30, "Repara radial",70.25f, Reparacion, herramienta[2]),
                new ReparaciónItem(20, "Reparar serrucho",15.67f, Reparacion, herramienta[4])

            };
            Reparacion.ReparaciónItems= ReparacionItems;

            // 2. Add the entities to the context
            _context.AddRange(fabricante); // This will add the 'herramienta' list due to the relationship
            _context.Add(usuario);
            _context.Add(Reparacion);

            // 3. Save the changes to the in-memory database
            _context.SaveChanges();

            // --- Missing Code Ends Here ---
        }
    }
}*/