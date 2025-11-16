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
        private readonly ILogger<AlquileresController> _logger;

        public AlquileresController(ApplicationDbContext context, ILogger<AlquileresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("DetalleAlquiler")]
        // El tipo de respuesta es un AlquileresParaDetalleDTO (detalle de un alquiler)
        [ProducesResponseType(typeof(AlquileresParaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetalleHerramientasParaAlquiler(int id)
        {
            if (_context.Alquileres == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }

            var alquiler = await _context.Alquileres
                .Include(o => o.MetodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.AlquilarItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (alquiler == null)
            {
                _logger.LogError("Error: No se encontró el alquiler con id {Id}.", id);
                return NotFound();
            }

            var alquilerParaDetalle = new AlquileresParaDetalleDTO(
                alquiler.Usuario.Nombre,
                alquiler.Usuario.Apellidos,
                alquiler.DireccionEnvio,
                alquiler.FechaAlquiler,
                alquiler.PrecioTotal,
                alquiler.FechaInicio,
                alquiler.FechaFin,
                alquiler.AlquilarItems.Select(oi => new AlquilarItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Cantidad
                )).ToList()
            );

            return Ok(alquilerParaDetalle);
        }

        [HttpPost]
        [Route("CrearAlquiler")]
        [ProducesResponseType(typeof(AlquileresParaDetalleDTO), 201)] // Created
        [ProducesResponseType(typeof(ValidationProblemDetails),400)] // Bad Request
        [ProducesResponseType(typeof(string), 409)] // Conflict
        public async Task<ActionResult> CreateAlquiler([FromBody] CrearAlquilerDTO crearAlquilerDTO)
        {
            // Validaciones de lógica
            if (_context.Alquileres == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                _logger.LogError("Error: Faltan DbSets (Alquileres, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }


            if (crearAlquilerDTO.Items == null || !crearAlquilerDTO.Items.Any())
                ModelState.AddModelError(nameof(crearAlquilerDTO.Items), "El alquiler debe incluir al menos una herramienta.");

            var metodoPago = await _context.MetodosPagos.FindAsync(crearAlquilerDTO.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(crearAlquilerDTO.MetodoPagoId), $"El MetodoPagoId {crearAlquilerDTO.MetodoPagoId} no existe.");

            var usuario = await _context.Users.FirstOrDefaultAsync(
                u => u.Nombre == crearAlquilerDTO.Nombre &&
                u.Apellidos == crearAlquilerDTO.Apellidos);
            if (usuario == null) ModelState.AddModelError(nameof(crearAlquilerDTO.Nombre), $"El Usuario {crearAlquilerDTO.Nombre} {crearAlquilerDTO.Apellidos} no existe.");

            // Alguna validación más ???

            // Si hay errores, retornar
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Consulta única

            // Hacer una sola llamada a la BBDD para traer todas las herramientas
            var herramientaIds = crearAlquilerDTO.Items.Select(i => i.HerramientaId).Distinct().ToList();
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id);

            //Construccion en memoria

            var nuevoAlquiler = new Alquiler
            {
                DireccionEnvio = crearAlquilerDTO.Direccion,
                MetodoPago = metodoPago,
                AlquilarItems = new List<AlquilarItem>(),
                Usuario = usuario,
            };



            // Validar que todas las herramientas existen
            foreach (var itemDTO in crearAlquilerDTO.Items)
            {
                // Buscar la herramienta en la lista local (el Diccionario)
                if (!herramientasEnDB.TryGetValue(itemDTO.HerramientaId, out var herramienta))
                {
                    // La herramienta no se encontró en nuestra consulta
                    ModelState.AddModelError(nameof(CrearAlquilerDTO.Items), $"La HerramientaId {itemDTO.HerramientaId} no existe.");
                }
                else
                {
                    var nuevoItem = new AlquilarItem(
                        herramienta.Precio,               // <-- usar precio unitario
                        itemDTO.HerramientaCantidad,
                        nuevoAlquiler,
                        herramienta
                    );

                    nuevoAlquiler.AlquilarItems.Add(nuevoItem);
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Calcular el precio total del alquiler
            nuevoAlquiler.PrecioTotal = nuevoAlquiler.AlquilarItems
                .Sum(i => i.Cantidad * i.Precio);

            _context.Alquileres.Add(nuevoAlquiler);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el nuevo alquiler en la base de datos.");
                return Conflict($"Ocurrió un error al guardar el alquiler: {ex.Message}");
            }

            // Construir DTO de respuesta
            var alquilerDTORespuesta = new AlquileresParaDetalleDTO(
                nuevoAlquiler.Usuario.Nombre,
                nuevoAlquiler.Usuario.Apellidos,
                nuevoAlquiler.DireccionEnvio,
                nuevoAlquiler.FechaAlquiler,
                nuevoAlquiler.PrecioTotal,
                nuevoAlquiler.FechaInicio,
                nuevoAlquiler.FechaFin,
                // Mapeamos los items desde los objetos en memoria
                nuevoAlquiler.AlquilarItems.Select(oi => new AlquilarItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Cantidad
                )).ToList()
            );

            // Devolvemos el DTO de detalle
            return CreatedAtAction(
                nameof(GetDetalleHerramientasParaAlquiler), // Nombre del método GET
                new { id = nuevoAlquiler.Id }, // Parámetro de ruta para el método GET
                alquilerDTORespuesta); // El cuerpo de la respuesta
        }
    }
}
