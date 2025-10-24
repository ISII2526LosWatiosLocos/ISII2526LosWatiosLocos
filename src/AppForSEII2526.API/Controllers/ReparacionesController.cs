using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                r.ReparaciónItems.Select(ri => new ReparacionesItemDTO(
                    ri.Herramienta.Nombre,
                    ri.Descripción,
                    ri.cantidad,
                    ri.Herramienta.Precio
                )).ToList()



            )).ToList();

            return Ok(reparacionesDTO);
        }



        [HttpPost]
        [Route("Crear-Reparación")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReparacionesDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateReparacion([FromBody] ReparacionesDTO reparacionCreate)
        {
            if (_context.Reparaciones == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                _logger.LogError("Error: Faltan DbSets (Reparaciones, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            // Validacion FechaEntrega > hoy
            if (reparacionCreate.FechaEntrega <= DateTime.Today)
                ModelState.AddModelError("FechaEntrega", "Error: la fecha de entrega debe ser posterior a hoy");

            // Validacion FechaRecogida > FechaEntrega
            if (reparacionCreate.FechaRecogida <= reparacionCreate.FechaEntrega)
                ModelState.AddModelError("FechaRecogida", "Error: la fecha de recogida debe ser posterior a la entrega");

            // Validar que haya al menos un item
            if (reparacionCreate.ReparacionesItems == null || reparacionCreate.ReparacionesItems.Count == 0)
                ModelState.AddModelError("ReparacionItems", "Error: debe incluir al menos una herramienta para reparación");

            // Buscar usuario
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Nombre == reparacionCreate.nombre);
            if (usuario == null)
                ModelState.AddModelError("Usuario", "Error: el nombre de usuario no está registrado");

            // Si hay errores, retornar
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Hacer una sola llamada a la BBDD para traer todas las herramientas
            var herramientaNombres = reparacionCreate.ReparacionesItems.Select(i => i.HerramientaNombre).Distinct().ToList();
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaNombres.Contains(h.Nombre))
                .ToDictionaryAsync(h => h.Nombre);

            // Validar que todas las herramientas existen
            foreach (var item in reparacionCreate.ReparacionesItems)
            {
                if (!herramientasEnDB.ContainsKey(item.HerramientaNombre))
                {
                    ModelState.AddModelError("ReparacionItems", $"Error: la herramienta '{item.HerramientaNombre}' no existe");
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Construir DTO de respuesta con la información de los objetos Herramienta
            var reparacionDetalle = new ReparacionesDTO(
                usuario.Nombre,
                usuario.Apellidos,
                reparacionCreate.FechaEntrega,
                reparacionCreate.FechaRecogida,
                reparacionCreate.PrecioTotal,
                reparacionCreate.ReparacionesItems.Select(ri =>
                {
                    var herramienta = herramientasEnDB[ri.HerramientaNombre];
                    return new ReparacionesItemDTO(
                        herramienta.Nombre,   
                        ri.HerramientaDescripcion, 
                        ri.HerramientaCantidad,   
                        herramienta.Precio         
                    );
                }).ToList()
            );

            // Devolver el DTO simulado
            return CreatedAtAction("GetDetalleHerramientasParaReparación", new { }, reparacionDetalle);
        }

    }
}

