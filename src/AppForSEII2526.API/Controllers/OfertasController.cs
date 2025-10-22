using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<HerramientasController> _logger;

        public OfertasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Oferta")]
        [ProducesResponseType(typeof(IList<OfertasDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaOferta()
        {
            var ofertas = await _context.Ofertas
                .Include(o => o.MetodosPago)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var ofertasDTO = ofertas.Select(o => new OfertasDTO(
                o.FechaFinal,
                o.FechaInicio,
                o.FechaOferta,
                o.TipoDirigida.ToString(),
                o.MetodosPago.Nombre,
                o.Items.Select(oi => oi.Herramienta.Nombre).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Material).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Fabricante.Nombre).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Precio).FirstOrDefault(),
                o.Items.Select(oi => oi.PrecioFinal).FirstOrDefault()

            )).ToList();

            return Ok(ofertasDTO);
        }
    }
}
