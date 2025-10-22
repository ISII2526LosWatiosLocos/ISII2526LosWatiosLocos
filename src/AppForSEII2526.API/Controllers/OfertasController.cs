using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 

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
        [ProducesResponseType(typeof(IList<OfertasParaDetalleDTO>), (int)HttpStatusCode.OK)]
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
                )).ToList()
            )).ToList();

            if (ofertas == null)
            {
                _logger.LogError("Error: No se encontraron ofertas.");
                return NotFound();
            }

            return Ok(ofertasDTO);
        }

        [HttpPost]
        [Route("Crear-Oferta")]
        [ProducesResponseType(typeof(OfertasParaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CrearOferta([FromBody] CrearOfertaDTO crearOfertaDTO)
        {
            // --- 1. VALIDACIONES DE LÓGICA (Patrón del ejemplo) ---

            if (_context.Ofertas == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                // Este es un error 500
                _logger.LogError("Error: Faltan DbSets (Ofertas, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            if (crearOfertaDTO.FechaInicio <= DateTime.Today)
                ModelState.AddModelError(nameof(crearOfertaDTO.FechaInicio), "La fecha de inicio debe ser posterior a hoy.");

            if (crearOfertaDTO.FechaInicio >= crearOfertaDTO.FechaFinal)
                ModelState.AddModelError(nameof(crearOfertaDTO.FechaFinal), "La fecha final debe ser posterior a la fecha de inicio.");

            if (crearOfertaDTO.Items == null || !crearOfertaDTO.Items.Any())
                ModelState.AddModelError(nameof(crearOfertaDTO.Items), "La oferta debe incluir al menos una herramienta.");

            // --- 2. VALIDAR ENTIDADES RELACIONADAS (Patrón del ejemplo) ---

            // a. Buscar Método de Pago
            var metodoPago = await _context.MetodosPagos.FindAsync(crearOfertaDTO.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(crearOfertaDTO.MetodoPagoId), $"El MetodoPagoId {crearOfertaDTO.MetodoPagoId} no existe.");

            // b. Validar Enum de TipoDirigida (si se proporcionó)
            tipoDirigidaOferta tipoDirigido = tipoDirigidaOferta.Cliente; // Valor por defecto (asumiendo que Clientes es tu "todo el mundo")
            if (!string.IsNullOrEmpty(crearOfertaDTO.TipoDirigida))
            {
                if (!Enum.TryParse<tipoDirigidaOferta>(crearOfertaDTO.TipoDirigida, true, out tipoDirigido))
                    ModelState.AddModelError(nameof(crearOfertaDTO.TipoDirigida), $"El valor '{crearOfertaDTO.TipoDirigida}' no es válido. Use 'Socio' o 'Cliente'.");
            }

            // c. Si hay *cualquier* error de los anteriores, parar y devolverlos todos
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- 3. CONSULTA ÚNICA (Patrón del ejemplo) ---

            // a. Coger todos los IDs del DTO
            var herramientaIds = crearOfertaDTO.Items.Select(i => i.HerramientaId).Distinct().ToList();

            // b. Hacer UNA sola llamada a la BBDD para traer todas las herramientas
            //    e incluir su Fabricante (para construir el DTO de respuesta después)
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id); // Convertir a Diccionario para búsquedas rápidas en memoria

            // --- 4. CONSTRUCCIÓN EN MEMORIA (Patrón del ejemplo) ---

            var nuevaOferta = new Oferta
            {
                FechaInicio = crearOfertaDTO.FechaInicio,
                FechaFinal = crearOfertaDTO.FechaFinal,
                FechaOferta = DateTime.UtcNow,
                MetodosPago = metodoPago!, // Sabemos que no es null por la validación anterior
                TipoDirigida = tipoDirigido,
                Items = new List<OfertaItem>()
            };

            // --- 5. BUCLE EN MEMORIA (Patrón del ejemplo) ---
            foreach (var itemDTO in crearOfertaDTO.Items)
            {
                // Buscar la herramienta en la lista local (el Diccionario)
                if (!herramientasEnDB.TryGetValue(itemDTO.HerramientaId, out var herramienta))
                {
                    // La herramienta no se encontró en nuestra consulta
                    ModelState.AddModelError(nameof(crearOfertaDTO.Items), $"La HerramientaId {itemDTO.HerramientaId} no existe.");
                }
                else if (itemDTO.PorcentajeDescuento <= 0 || itemDTO.PorcentajeDescuento > 90) // Lógica de negocio
                {
                    ModelState.AddModelError(nameof(crearOfertaDTO.Items), $"El porcentaje {itemDTO.PorcentajeDescuento}% para '{herramienta.Nombre}' no es válido. Debe estar entre 1 y 90.");
                }
                else
                {
                    // Todo correcto para este item
                    float precioFinal = herramienta.Precio * (1 - (itemDTO.PorcentajeDescuento / 100.0f));

                    var nuevoItem = new OfertaItem
                    {
                        Herramienta = herramienta,
                        Oferta = nuevaOferta,
                        Porcentaje = itemDTO.PorcentajeDescuento,
                        PrecioFinal = precioFinal
                    };
                    nuevaOferta.Items.Add(nuevoItem);
                }
            }

            // --- 6. VALIDACIÓN FINAL (Patrón del ejemplo) ---
            // Comprobar si se añadieron errores *dentro* del bucle
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- 7. GUARDADO ÚNICO (Patrón del ejemplo) ---
            _context.Ofertas.Add(nuevaOferta);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la nueva oferta en la base de datos.");
                return Conflict($"Ocurrió un error al guardar la oferta: {ex.Message}");
            }

            // --- 8. RESPUESTA SIN RECARGAR (Patrón del ejemplo) ---
            // Construimos el DTO de detalle con los objetos que ya tenemos

            var ofertaDTORespuesta = new OfertasParaDetalleDTO(
                nuevaOferta.FechaFinal,
                nuevaOferta.FechaInicio,
                nuevaOferta.FechaOferta,
                nuevaOferta.TipoDirigida.ToString(),
                // El tipo de MetodoPago (ej. "PayPal", "TarjetaCredito")
                nuevaOferta.MetodosPago.GetType().Name,

                // Mapeamos los items desde los objetos en memoria
                nuevaOferta.Items.Select(oi => new OfertaItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Fabricante.Nombre, // Esto funciona gracias al .Include() que hicimos
                    oi.Herramienta.Precio,
                    oi.PrecioFinal
                )).ToList()
            );

            // Devolvemos el DTO de detalle
            return CreatedAtAction(
                nameof(GetDetalleHerramientasParaOferta), // Nombre del método GET
                new { id = nuevaOferta.Id }, // Parámetro de ruta para el método GET
                ofertaDTORespuesta); // El cuerpo de la respuesta
        }
    }
}
