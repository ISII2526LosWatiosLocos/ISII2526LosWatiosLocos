using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReparacionesController : ControllerBase
    { //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<HerramientasController> _logger;

        public ReparacionesController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Reparaciones")]
        [ProducesResponseType(typeof(IList<ReparacionesDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaReparación()
        {
            var reparaciones = await _context.Reparaciones
                .Include(r => r.MétodoPago)
                .Include(r => r.ReparaciónItems)
                    .ThenInclude(ri => ri.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var reparacionesDTO = reparaciones.Select(r => new ReparacionesDTO(
                r.Usuario.Nombre,
                r.Usuario.Apellidos,
                r.FechaEntrega,
                r.FechaRecogida,
                r.PrecioTotal,
                r.ReparaciónItems.Select(ri => ri.Herramienta.Nombre).FirstOrDefault()!,
                r.ReparaciónItems.Select(ri => ri.Herramienta.Precio).FirstOrDefault(),
                r.ReparaciónItems.Select(ri => ri.Descripción).FirstOrDefault(),
                r.ReparaciónItems.Select(ri => ri.cantidad).FirstOrDefault()



            )).ToList();

            return Ok(reparacionesDTO);
        }


    }
}
