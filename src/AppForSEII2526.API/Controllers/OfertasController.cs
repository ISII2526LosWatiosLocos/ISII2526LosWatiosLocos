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
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetalleHerramientasParaOferta()
        {

            if (_context.Ofertas == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }

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
                o.MetodosPago.ToString(),
                o.Items.Select(oi => new OfertaItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Fabricante.Nombre,
                    oi.Herramienta.Precio,
                    oi.PrecioFinal
                )).ToList()
            )).ToList();

            if (ofertas == null)
            {
                _logger.LogError("Error: No se encontraron ofertas.");
                return NotFound();
            }

            return Ok(ofertasDTO);
        }
    }
}
