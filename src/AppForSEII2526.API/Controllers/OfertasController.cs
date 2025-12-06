using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net; // Asegúrate de que este using esté presente

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OfertasController> _logger;

        public OfertasController(ApplicationDbContext context, ILogger<OfertasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Oferta")]
        [ProducesResponseType(typeof(IList<OfertasParaDetalleDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaOferta(int id)
        {
            // --- INYECCIÓN DE LOG DE ADVERTENCIA Y CRÍTICO ---
            if (id == 0)
            {
                // Advertencia: Posible error de cliente, pero no detiene la ejecución.
                _logger.LogWarning("Se recibió una solicitud GET para Detalle-Oferta con ID 0. Esto podría indicar un error de llamada.");
                // Opcional: Podríamos detener la ejecución aquí si ID 0 no es válido, pero continuamos para probar el NotFound.
            }

            if (_context.Ofertas == null)
            {
                // Un error grave si un DbSet está nulo
                _logger.LogCritical("CRITICAL ERROR: El DbSet Ofertas es nulo. Verifique la configuración del DbContext.");
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }
            // --- FIN INYECCIÓN ---

            var ofertas = await _context.Ofertas
                .Include(o => o.MetodosPago)
                .Include(o => o.Usuario)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .Where(o => o.Id == id)
                .ToListAsync();


            var ofertasDTO = ofertas.Select(o => new OfertasParaDetalleDTO(
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
                    oi.Herramienta.Precio * (100f - oi.Porcentaje) / 100
                )).ToList(),
                o.Usuario.Nombre
            )).FirstOrDefault();

            if (ofertasDTO == null)
            {
                _logger.LogError("Error: No se encontraron ofertas para el ID {OfertaId}.", id);
                return NotFound();
            }

            // --- INYECCIÓN DE LOG DE INFORMACIÓN ---
            _logger.LogInformation("Oferta obtenida correctamente. ID: {OfertaId}", id);
            return Ok(ofertasDTO);
        }

        [HttpPost]
        [Route("Crear-Oferta")]
        [ProducesResponseType(typeof(OfertasParaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CrearOferta([FromBody] CrearOfertaDTO crearOfertaDTO)
        {
            // --- 1. VALIDACIONES DE LÓGICA ---

            if (_context.Ofertas == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                // Log Crítico: Error de configuración interno
                _logger.LogCritical("CRITICAL ERROR: Faltan DbSets (Ofertas, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            if (crearOfertaDTO.FechaInicio <= DateOnly.FromDateTime(DateTime.Today))
                ModelState.AddModelError(nameof(crearOfertaDTO.FechaInicio), "La fecha de inicio debe ser posterior a hoy.");

            if (crearOfertaDTO.FechaInicio >= crearOfertaDTO.FechaFinal)
                ModelState.AddModelError(nameof(crearOfertaDTO.FechaFinal), "La fecha final debe ser posterior a la fecha de inicio.");

            if (crearOfertaDTO.Items == null || !crearOfertaDTO.Items.Any())
                ModelState.AddModelError(nameof(crearOfertaDTO.Items), "La oferta debe incluir al menos una herramienta.");

            // --- 2. VALIDAR ENTIDADES RELACIONADAS ---

            // a. Buscar Método de Pago
            var metodoPago = await _context.MetodosPagos.FindAsync(crearOfertaDTO.MetodoPagoId);
            if (metodoPago == null)
            {
                // Log Error: Entidad requerida no encontrada
                _logger.LogError("MetodoPagoId {MetodoPagoId} no existe.", crearOfertaDTO.MetodoPagoId);
                ModelState.AddModelError(nameof(crearOfertaDTO.MetodoPagoId), $"El MetodoPagoId {crearOfertaDTO.MetodoPagoId} no existe.");
            }

            // b. Validar Enum de TipoDirigida (si se proporcionó)
            tipoDirigidaOferta tipoDirigido = tipoDirigidaOferta.Cliente;
            if (!string.IsNullOrEmpty(crearOfertaDTO.TipoDirigida))
            {
                if (!Enum.TryParse<tipoDirigidaOferta>(crearOfertaDTO.TipoDirigida, true, out tipoDirigido))
                    ModelState.AddModelError(nameof(crearOfertaDTO.TipoDirigida), $"El valor '{crearOfertaDTO.TipoDirigida}' no es válido. Use 'Socio' o 'Cliente'.");
            }

            //c. Validar Usuario
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Nombre == crearOfertaDTO.nombreUsuario);
            if (usuario == null)
            {
                // Log Error: Entidad requerida no encontrada
                _logger.LogError("Usuario con nombre '{NombreUsuario}' no encontrado.", crearOfertaDTO.nombreUsuario);
                ModelState.AddModelError(nameof(crearOfertaDTO.nombreUsuario), "El usuario no existe.");
            }

            // d. Si hay *cualquier* error de los anteriores, parar y devolverlos todos
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- 3. CONSULTA ÚNICA ---

            var herramientaIds = crearOfertaDTO.Items.Select(i => i.HerramientaId).Distinct().ToList();
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id);

            // --- 4. CONSTRUCCIÓN EN MEMORIA ---
            var nuevaOferta = new Oferta(
                crearOfertaDTO.FechaFinal,
                crearOfertaDTO.FechaInicio,
                DateOnly.FromDateTime(DateTime.UtcNow),
                tipoDirigido,
                new List<OfertaItem>(),
                metodoPago!,
                usuario
            );

            // --- 5. BUCLE EN MEMORIA (Validaciones Item por Item) ---
            foreach (var itemDTO in crearOfertaDTO.Items)
            {
                if (!herramientasEnDB.TryGetValue(itemDTO.HerramientaId, out var herramienta))
                {
                    // Log Error: Problema en la integridad del DTO
                    _logger.LogError("HerramientaId {HerramientaId} no existe. Fallo de integridad en la creación de la oferta.", itemDTO.HerramientaId);
                    ModelState.AddModelError(nameof(crearOfertaDTO.Items), $"La HerramientaId {itemDTO.HerramientaId} no existe.");
                }
                else if (itemDTO.PorcentajeDescuento <= 0 || itemDTO.PorcentajeDescuento > 90)
                {
                    // Log Advertencia: Descuento demasiado alto (lógica de negocio cuestionable)
                    if (itemDTO.PorcentajeDescuento > 50)
                    {
                        _logger.LogWarning("Aplicando un descuento muy alto ({Descuento}%) a la herramienta '{HerramientaNombre}'.", itemDTO.PorcentajeDescuento, herramienta.Nombre);
                    }
                    ModelState.AddModelError(nameof(crearOfertaDTO.Items), $"El porcentaje {itemDTO.PorcentajeDescuento}% para '{herramienta.Nombre}' no es válido. Debe estar entre 1 y 90.");
                }
                else
                {
                    float precioFinal = herramienta.Precio * (1 - (itemDTO.PorcentajeDescuento / 100.0f));

                    var nuevoItem = new OfertaItem(
                        itemDTO.PorcentajeDescuento,
                        precioFinal,
                        nuevaOferta,
                        herramienta
                    );
                    nuevaOferta.Items.Add(nuevoItem);
                }
            }

            // --- 6. VALIDACIÓN FINAL ---
            if (ModelState.ErrorCount > 0)
            {
                // Log Advertencia: Petición mal formada devuelta al cliente
                _logger.LogWarning("Se recibió una solicitud de CrearOferta mal formada. {ErrorCount} errores de validación.", ModelState.ErrorCount);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // --- 7. GUARDADO ÚNICO ---
            _context.Ofertas.Add(nuevaOferta);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log Crítico/Error: Error fatal al guardar en la DB
                _logger.LogCritical(ex, "CRITICAL: No se pudo guardar la nueva oferta. Fallo de conexión o restricción de DB.");
                return Conflict($"Ocurrió un error al guardar la oferta: {ex.Message}");
            }

            // --- 8. RESPUESTA SIN RECARGAR ---
            var ofertaDTORespuesta = new OfertasParaDetalleDTO(
                nuevaOferta.FechaFinal,
                nuevaOferta.FechaInicio,
                nuevaOferta.FechaOferta,
                nuevaOferta.TipoDirigida.ToString(),
                // El tipo de MetodoPago (ej. "PayPal", "TarjetaCredito")
                metodoPago!.GetType().Name, // Usamos el objeto metodoPago que ya incluimos arriba

                // Mapeamos los items desde los objetos en memoria
                nuevaOferta.Items.Select(oi => new OfertaItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    herramientasEnDB[oi.Herramienta.Id].Fabricante.Nombre, // Usamos el diccionario para el fabricante
                    oi.Herramienta.Precio,
                    oi.PrecioFinal
                )).ToList(),
                usuario!.Nombre // Usamos el objeto usuario que ya incluimos arriba
            );

            // Log Información: Éxito
            _logger.LogInformation("Oferta creada con éxito. ID: {OfertaId}", nuevaOferta.Id);

            return CreatedAtAction(
                nameof(GetDetalleHerramientasParaOferta),
                new { id = nuevaOferta.Id },
                ofertaDTORespuesta);
        }
    }
}