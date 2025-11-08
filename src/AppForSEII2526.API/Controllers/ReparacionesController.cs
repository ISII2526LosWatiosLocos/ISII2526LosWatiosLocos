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
        private readonly ILogger<ReparacionesController> _logger;

        public ReparacionesController(ApplicationDbContext context, ILogger<ReparacionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Reparaciones")]
        [ProducesResponseType(typeof(IList<ReparacionesDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaReparación(int id)
        {
            var reparaciones = await _context.Reparaciones
                .Include(r => r.MétodoPago)
                 .Include(r => r.Usuario)
                .Include(r => r.ReparaciónItems)
                    .ThenInclude(ri => ri.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                             .Where(r => r.Id == id)
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

            if (reparacionesDTO == null || reparacionesDTO.Count == 0)
                return NotFound();


            return Ok(reparacionesDTO);
        }



        [HttpPost]
        [Route("Crear-Reparación")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ReparacionesDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateReparacion([FromBody] CrearReparacionDTO reparacionCreate)
        {
            if (_context.Reparaciones == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                _logger.LogError("Error: Faltan DbSets (Reparaciones, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }
            var metodoPago = await _context.MetodosPagos.FindAsync(reparacionCreate.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(reparacionCreate.MetodoPagoId), $"El MetodoPagoId {reparacionCreate.MetodoPagoId} no existe.");
          
            // Validacion FechaEntrega > hoy
            if (reparacionCreate.FechaEntrega <= DateOnly.FromDateTime(DateTime.Now))
                ModelState.AddModelError("FechaEntrega", "Error: la fecha de entrega debe ser posterior a hoy");

            // Validacion FechaRecogida > FechaEntrega
            if (reparacionCreate.FechaRecogida <= reparacionCreate.FechaEntrega)
                ModelState.AddModelError("FechaRecogida", "Error: la fecha de recogida debe ser posterior a la entrega");

            // Validar que haya al menos un item
            if (reparacionCreate.ReparacionesItems == null || reparacionCreate.ReparacionesItems.Count == 0)
                ModelState.AddModelError("ReparacionItems", "Error: debe incluir al menos una herramienta para reparación");

            // Buscar usuario
           var Usuario = await _context.Users.FirstOrDefaultAsync(u=>u.Nombre == reparacionCreate.Nombre && u.Apellidos == reparacionCreate.Apellidos);
            if (Usuario == null) ModelState.AddModelError(nameof(reparacionCreate.Nombre), $"El Usuario {reparacionCreate.Nombre} {reparacionCreate.Apellidos} no existe.");

            // Si hay errores, retornar
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Hacer una sola llamada a la BBDD para traer todas las herramientas
            var herramientaNombres = reparacionCreate.ReparacionesItems.Select(i => i.HerramientaId).Distinct().ToList();
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaNombres.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id);



            var nuevaReparacion = new Reparación
            {
               
                FechaEntrega= reparacionCreate.FechaEntrega,
               FechaRecogida = reparacionCreate.FechaRecogida,
                PrecioTotal = reparacionCreate.PrecioTotal,
               ReparaciónItems= new List<ReparaciónItem>(),
                MétodoPago = metodoPago!, // Sabemos que no es null por la validación anterior
                Usuario = Usuario

            };
















            // Validar que todas las herramientas existen
            foreach (var itemDTO in reparacionCreate.ReparacionesItems)
            {
                if (!herramientasEnDB.TryGetValue(itemDTO.HerramientaId, out var herramienta))
                {
                    // La herramienta no se encontró en nuestra consulta
                    ModelState.AddModelError(nameof(reparacionCreate.ReparacionesItems), $"La HerramientaId {itemDTO.HerramientaId} no existe.");
                }
                else
                {
                   

                    var nuevoItem = new ReparaciónItem
                    {
                        Herramienta = herramienta,
                        Descripción = itemDTO.HerramientaDescripcion, 
                        cantidad = itemDTO.HerramientaCantidad, 

                                       
                    };

                    nuevaReparacion.ReparaciónItems.Add(nuevoItem);

                    // Calcular precio total 
                    nuevaReparacion.PrecioTotal += herramienta.Precio * itemDTO.HerramientaCantidad;
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));



            _context.Reparaciones.Add(nuevaReparacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la nueva oferta en la base de datos.");
                return Conflict($"Ocurrió un error al guardar la oferta: {ex.Message}");
            }

            // Construir DTO de respuesta con la información de los objetos Herramienta
            var reparacionDetalle = new ReparacionesDTO(
                nuevaReparacion.Usuario.Nombre,
                nuevaReparacion.Usuario.Apellidos,
                nuevaReparacion.FechaEntrega,
                nuevaReparacion.FechaRecogida,
                nuevaReparacion.PrecioTotal,
                nuevaReparacion.ReparaciónItems.Select(ri =>
                {
                  
                    return new ReparacionesItemDTO(
                        ri.Herramienta.Nombre,   
                        ri.Descripción, 
                        ri.cantidad,   
                        ri.Herramienta.Precio         
                    );
                }).ToList()
            );

            // Devolver el DTO simulado
            return CreatedAtAction(
     nameof(GetDetalleHerramientasParaReparación), // Nombre del método GET
     new { id = nuevaReparacion.Id }, // Parámetro de ruta para el método GET  
     reparacionDetalle); // El cuerpo de la respuesta (tu DTO)
        }

    }
}


