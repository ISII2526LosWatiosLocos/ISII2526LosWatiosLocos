using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HerramientasController : ControllerBase
    {

        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<HerramientasController> _logger;

        public HerramientasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Para-Compra")]
        [ProducesResponseType(typeof(IList<ComprasDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaCompra()
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Select(h => new HerramientasDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio))
                .ToListAsync();
            return Ok(herramientas);
        }
    }
}
