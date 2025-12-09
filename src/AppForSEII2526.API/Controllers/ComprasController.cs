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
    public class ComprasController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<ComprasController> _logger;

        public ComprasController(ApplicationDbContext context, ILogger<ComprasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Detalle-Compra")]
        // El tipo de respuesta es una lista de ComprasParaDetalleDTO
        [ProducesResponseType(typeof(IList<ComprasParaDetalleDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetalleHerramientasParaCompra(int id)
        {
            // ---INYECCIÓN DE LOG DE ADVERTENCIA Y CRÍTICO ---
            if (id == 0)
            {
                // Advertencia: Posible error de cliente, pero no detiene la ejecución.
                _logger.LogWarning("Se recibió una solicitud GET para Detalle-Compra con ID 0. Esto podría indicar un error de llamada.");
                // Opcional: Podríamos detener la ejecución aquí si ID 0 no es válido, pero continuamos para probar el NotFound.
            }

            if (_context.Compras == null)
            {
                // Un error grave si un DbSet está nulo
                _logger.LogCritical("CRITICAL ERROR: El DbSet Compras es nulo. Verifique la configuración del DbContext.");
                _logger.LogError("Error: La tabla Compras no existe en el DbContext.");
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(o => o.MetodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.CompraItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (compra == null)
            {
                _logger.LogError("Error: No se encontró la compra con id {Id}", id);
                return NotFound();
            }
            // --- INYECCIÓN DE LOG DE INFORMACIÓN ---
            _logger.LogInformation("Compra obtenida correctamente. ID: {Id}", id);

            var compraDto = new ComprasParaDetalleDTO(
                compra.Usuario?.Nombre ?? string.Empty,
                compra.Usuario?.Apellidos ?? string.Empty,
                compra.DireccionEnvio,
                compra.PrecioTotal,
                compra.FechaCompra,
                compra.CompraItems.Select(oi => new CompraItemsDTO(
                    oi.Herramienta.Id,
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Descripcion,
                    oi.Cantidad
                )).ToList()
            );

            return Ok(compraDto);
        }

        [HttpPost]
        [Route("Crear-Compra")]
        [ProducesResponseType(typeof(ComprasParaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CrearCompra([FromBody] CrearCompraDTO CrearCompraDTO)
        {
            // --- 1. VALIDACIONES DE LÓGICA (Patrón del ejemplo) ---

            if (_context.Compras == null || _context.Herramientas == null || _context.MetodosPagos == null)
            {
                // Este es un error 500
                _logger.LogCritical("Error Crítico: Faltan DbSets (Compras, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            // --- 2. VALIDAR ENTIDADES RELACIONADAS (Patrón del ejemplo) ---

            // a. Buscar Método de Pago
            var metodoPago = await _context.MetodosPagos.FindAsync(CrearCompraDTO.MetodoPagoId);
            if (metodoPago == null)
            {
                // Log Error: Entidad requerida no encontrada
                _logger.LogError("MetodoPagoId {MetodoPagoId} no existe.", CrearCompraDTO.MetodoPagoId);
                ModelState.AddModelError(nameof(CrearCompraDTO.MetodoPagoId), $"El MetodoPagoId {CrearCompraDTO.MetodoPagoId} no existe.");
            }

            // b. Buscar Usuario
            var Usuario = await _context.Users.FirstOrDefaultAsync(u=>u.Nombre == CrearCompraDTO.Nombre && u.Apellidos == CrearCompraDTO.Apellidos);
            if (Usuario == null)
            {
                // Log Error: Entidad requerida no encontrada
                _logger.LogError("Usuario con nombre '{NombreUsuario}' no encontrado.", CrearCompraDTO.Nombre);
                ModelState.AddModelError(nameof(CrearCompraDTO.Nombre), $"El Usuario {CrearCompraDTO.Nombre} {CrearCompraDTO.Apellidos} no existe.");
            }
            // c. Validar que la compra tenga items
            if (CrearCompraDTO.Items == null || !CrearCompraDTO.Items.Any())
                ModelState.AddModelError(nameof(CrearCompraDTO.Items), "La compra debe incluir al menos una herramienta.");

            // d. Si hay *cualquier* error de los anteriores, parar y devolverlos todos
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- 3. CONSULTA ÚNICA (Patrón del ejemplo) ---

            // a. Coger todos los IDs del DTO
            var herramientaIds = CrearCompraDTO.Items.Select(i => i.IdHerramienta).Distinct().ToList();

            // b. Hacer UNA sola llamada a la BBDD para traer todas las herramientas
            //    e incluir su Fabricante (para construir el DTO de respuesta después)
            var herramientasEnDB = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => herramientaIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id); // Convertir a Diccionario para búsquedas rápidas en memoria

            // --- 4. CONSTRUCCIÓN EN MEMORIA (Patrón del ejemplo) ---

            var nuevaCompra = new Compra
            {
                DireccionEnvio = CrearCompraDTO.DireccionEnvio,
                FechaCompra = DateOnly.FromDateTime(DateTime.UtcNow),
                CompraItems = new List<CompraItem>(),
                MetodoPago = metodoPago!, // Sabemos que no es null por la validación anterior
                Usuario = Usuario
            };

            // --- 5. BUCLE EN MEMORIA (Patrón del ejemplo) ---
            foreach (var itemDTO in CrearCompraDTO.Items)
            {
                // Buscar la herramienta en la lista local (el Diccionario)
                if (!herramientasEnDB.TryGetValue(itemDTO.IdHerramienta, out var herramienta))
                {
                    // Log Error: Problema en la integridad del DTO
                    _logger.LogError("HerramientaId {HerramientaId} no existe. Fallo de integridad en la creación de la compra.", itemDTO.IdHerramienta);
                    // La herramienta no se encontró en nuestra consulta
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La HerramientaId {itemDTO.IdHerramienta} no existe.");
                }
                else
                {
                    // Validar items del dto (que no tengan valores imposibles)
                    if (itemDTO.IdHerramienta <= 0)
                    {
                        _logger.LogError("IdHerramienta {IdHerramienta} menor o igual que 0. Fallo de integridad en la creación de la compra.", itemDTO.IdHerramienta);
                        ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"IdHerramienta inválido: {itemDTO.IdHerramienta}.");
                    }

                    if (itemDTO.CantidadHerramienta <= 0)
                    {
                        _logger.LogError("CantidadHerramienta {CantidadHerramienta} menor o igual que 0. Fallo de integridad en la creación de la compra.", itemDTO.CantidadHerramienta);
                        ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La cantidad para la IdHerramienta {itemDTO.IdHerramienta} debe ser mayor que 0.");
                    }
                    
                    // Todo correcto para este item
                    var nuevoItem = new CompraItem
                    {
                        Herramienta = herramienta,
                        Compra = nuevaCompra,
                        Cantidad = itemDTO.CantidadHerramienta,
                        Descripcion = itemDTO.DescripcionHerramienta,
                    };
                    nuevaCompra.CompraItems.Add(nuevoItem);
                }
            }

            // --- 6. CALCULO PRECIOTOTAL ---
            // convertimos a float solo por compatibilidad con el modelo, aunque decimal sería más adecuado
            // dos decimales para moneda (2)
            // opción recomendada para cálculos financieros (MidpointRounding.AwayFromZero)
            nuevaCompra.PrecioTotal = (float)Math.Round(nuevaCompra.CompraItems.Sum(i => (decimal)i.Herramienta.Precio * i.Cantidad), 2, MidpointRounding.AwayFromZero);


            // --- 7. VALIDACIÓN FINAL (Patrón del ejemplo) ---
            // Comprobar si se añadieron errores *dentro* del bucle
            if (ModelState.ErrorCount > 0)
            {
                // Log Error: Petición mal formada devuelta al cliente
                _logger.LogError("Se recibió una solicitud de CrearCompra mal formada. {ErrorCount} errores de validación.", ModelState.ErrorCount);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // --- 8. GUARDADO ÚNICO (Patrón del ejemplo) ---
            _context.Compras.Add(nuevaCompra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log Crítico/Error: Error fatal al guardar en la DB
                _logger.LogCritical(ex, "CRITICAL: No se pudo guardar la nueva compra. Fallo de conexión o restricción de DB.");
                return Conflict($"Ocurrió un error al guardar la compra: {ex.Message}");
            }

            // --- 9. RESPUESTA SIN RECARGAR (Patrón del ejemplo) ---
            // Construimos el DTO de detalle con los objetos que ya tenemos

            var compraDTORespuesta = new ComprasParaDetalleDTO(
                nuevaCompra.Usuario.Nombre,
                nuevaCompra.Usuario.Apellidos,
                nuevaCompra.DireccionEnvio,
                nuevaCompra.PrecioTotal,
                nuevaCompra.FechaCompra,

                // Mapeamos los items desde los objetos en memoria
                nuevaCompra.CompraItems.Select(oi => new CompraItemsDTO(
                    oi.Descripcion,
                    oi.Cantidad
                )).ToList()
            );

            // Log Información: Éxito
            _logger.LogInformation("Compra creada con éxito. ID: {IdCompra}", nuevaCompra.Id);

            // Devolvemos el DTO de detalle
            return CreatedAtAction(
                nameof(GetDetalleHerramientasParaCompra), // Nombre del método GET
                new { id = nuevaCompra.Id }, // Parámetro de ruta para el método GET
                compraDTORespuesta); // El cuerpo de la respuesta
        }
    }
}
