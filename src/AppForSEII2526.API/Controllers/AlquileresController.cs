using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {

        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<HerramientasController> _logger;

        public AlquileresController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Alquiler")]
        // El tipo de respuesta es una lista de AlquileresParaDetalleDTO
        [ProducesResponseType(typeof(IList<AlquileresParaDetalleDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetalleHerramientasParaAlquiler()
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }
            var alquileresParaDetalle = await _context.Alquileres
                .Include(o => o.MétodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.AlquilarItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var alquileresParaDetalleDTO = alquileresParaDetalle.Select(o => new AlquileresParaDetalleDTO(
                o.Usuario.Nombre,
                o.Usuario.Apellidos,
                o.DireccionEnvio,
                o.FechaAlquiler,
                o.PrecioTotal,
                o.FechaInicio,
                o.FechaFin,
                o.AlquilarItems.Select(oi => new AlquilarItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Cantidad
                )).ToList()

            )).ToList();

            if (alquileresParaDetalle == null)
            {
                _logger.LogError("Error: No se encontraron alquileres.");
                return NotFound();
            }

            return Ok(alquileresParaDetalleDTO);
        }
    }

}
