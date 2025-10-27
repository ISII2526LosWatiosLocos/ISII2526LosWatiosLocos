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
            if (_context.Alquileres == null)
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

        [HttpPost]
        [Route("Crear-Alquiler")]
        [ProducesResponseType(typeof(AlquileresParaDetalleDTO), 201)] // Created
        [ProducesResponseType(typeof(ValidationProblemDetails),400)] // Bad Request
        [ProducesResponseType(typeof(string), 409)] // Conflict
        public async Task<ActionResult> CreateAlquiler([FromBody] CrearAlquilerDTO alquilerCreate)
        {
            if (_context.Alquileres == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                _logger.LogError("Error: Faltan DbSets (Alquileres, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            // AÑADIR VALIDACIONES !!!!
            

            // Si hay errores, retornar
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Hacer una sola llamada a la BBDD para traer todas las herramientas
            var herramientaNombres = alquilerCreate.Items.Select(i => i.HerramientaId).Distinct().ToList();
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaNombres.Contains(h.Id))
                .ToDictionaryAsync(h => h.Nombre);

            // Validar que todas las herramientas existen
            foreach (var item in alquilerCreate.Items)
            {
                if (!herramientasEnDB.ContainsKey(item.HerramientaId)) // ?????????????????
                {
                    ModelState.AddModelError("AlquilerItems", $"Error: la herramienta '{item.HerramientaId}' no existe");
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Construir DTO de respuesta con la información de los objetos Herramienta
            var alquilerDetalle = new AlquileresParaDetalleDTO(
               
                // ARREGLAR CONSTRUCTOOOR !!!!!!!!!!
                alquilerCreate.ReparacionesItems.Select(ri =>
                {
                    var herramienta = herramientasEnDB[ri.HerramientaNombre];
                    return new AlquilarItemsDTO(
                        herramienta.Nombre,
                        ri.HerramientaDescripcion,
                        ri.HerramientaCantidad,
                        ri.HerramientaPrecio
                    );
                }).ToList()
            );

            // Devolver el DTO simulado
            return CreatedAtAction("GetDetalleHerramientasParaAlquiler", new { }, alquilerDetalle);
        }
    }

}
