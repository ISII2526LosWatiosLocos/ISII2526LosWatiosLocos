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
                .Include(o => o.MetodoPago)
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

            // Flujos alternativos ???


            // Validar entidades
            var metodoPago = await _context.MetodosPagos.FindAsync(crearAlquilerDTO.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(crearAlquilerDTO.MetodoPagoId), $"El MetodoPagoId {crearAlquilerDTO.MetodoPagoId} no existe.");

            var usuario = await _context.Users.FirstOrDefaultAsync(
                u => u.Nombre == crearAlquilerDTO.Nombre &&
                u.Apellidos == crearAlquilerDTO.Apellidos &&
                u.CorreoElectronico == crearAlquilerDTO.correo &&
                u.NumeroTelefono == crearAlquilerDTO.telefono);
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
               // Aplicar logica de negociooo (flujos alterrnativos) !!!!!
                else
                {
                    var nuevoItem = new AlquilarItem(
                        nuevoAlquiler.PrecioTotal,
                        itemDTO.HerramientaCantidad,
                        nuevoAlquiler,
                        herramienta);
                    nuevoAlquiler.AlquilarItems.Add(nuevoItem);
                }
            }
            // Validación final
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Guardado único

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

            // Respuesta sin recargar
            // Construir DTO de respuesta con la información de los objetos Herramienta
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
