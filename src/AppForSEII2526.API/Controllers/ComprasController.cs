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
        public async Task<IActionResult> GetDetalleHerramientasParaCompra()
        {
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla no existe.");
                return NotFound();
            }
            var comprasParaDetalle = await _context.Compras
                .Include(o => o.MetodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.CompraItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var comprasParaDetalleDTO = comprasParaDetalle.Select(o => new ComprasParaDetalleDTO(
                o.Usuario.Nombre,
                o.Usuario.Apellidos,
                o.DireccionEnvio,
                o.PrecioTotal,
                o.FechaCompra,
                o.CompraItems.Select(oi => new CompraItemsDTO(
                    oi.Herramienta.Id,
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Descripcion,
                    oi.Cantidad
                )).ToList()

            )).ToList();

            if (comprasParaDetalle == null)
            {
                _logger.LogError("Error: No se encontraron compras.");
                return NotFound();
            }

            return Ok(comprasParaDetalleDTO);
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
                _logger.LogError("Error: Faltan DbSets (Compras, Herramientas o MetodosPagos) en el DbContext.");
                return StatusCode(500, "Error interno del servidor al configurar la base de datos.");
            }

            if (CrearCompraDTO.Items == null || !CrearCompraDTO.Items.Any())
                ModelState.AddModelError(nameof(CrearCompraDTO.Items), "La compra debe incluir al menos una herramienta.");

            // --- 2. VALIDAR ENTIDADES RELACIONADAS (Patrón del ejemplo) ---

            // a. Buscar Método de Pago
            var metodoPago = await _context.MetodosPagos.FindAsync(CrearCompraDTO.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(CrearCompraDTO.MetodoPagoId), $"El MetodoPagoId {CrearCompraDTO.MetodoPagoId} no existe.");

            // b. Buscar Usuario
            var Usuario = await _context.Users.FirstOrDefaultAsync(u=>u.Nombre == CrearCompraDTO.Nombre && u.Apellidos == CrearCompraDTO.Apellidos);
            if (Usuario == null) ModelState.AddModelError(nameof(CrearCompraDTO.Nombre), $"El Usuario {CrearCompraDTO.Nombre} {CrearCompraDTO.Apellidos} no existe.");

            // c. Validar items del dto (que no tengan valores imposibles)
            if (CrearCompraDTO.Items == null || !CrearCompraDTO.Items.Any())
                ModelState.AddModelError(nameof(CrearCompraDTO.Items), "La compra debe incluir al menos una herramienta.");

            foreach (var itemDto in CrearCompraDTO.Items)
            {
                if (itemDto.IdHerramienta <= 0)
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"IdHerramienta inválido: {itemDto.IdHerramienta}.");

                if (itemDto.CantidadHerramienta <= 0)
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La cantidad para la IdHerramienta {itemDto.IdHerramienta} debe ser mayor que 0.");
            }

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
                    // La herramienta no se encontró en nuestra consulta
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La HerramientaId {itemDTO.IdHerramienta} no existe.");
                }
                else
                {
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
                return BadRequest(new ValidationProblemDetails(ModelState));

            // --- 8. GUARDADO ÚNICO (Patrón del ejemplo) ---
            _context.Compras.Add(nuevaCompra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la nueva compra en la base de datos.");
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

            // Devolvemos el DTO de detalle
            return CreatedAtAction(
                nameof(GetDetalleHerramientasParaCompra), // Nombre del método GET
                new { id = nuevaCompra.Id }, // Parámetro de ruta para el método GET
                compraDTORespuesta); // El cuerpo de la respuesta
        }
    }
}
