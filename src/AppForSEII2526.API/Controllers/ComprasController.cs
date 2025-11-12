using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

/*
Sistema 1. Ferretería: Caso de uso 1. Comprar herramientas

Flujo Básico:
    1. El cliente selecciona Comprar Herramientas en el menú principal.
    2. El Sistema muestra la lista de herramientas disponibles para comprar, indicando su NOMBRE, MATERIAL, FABRICANTE y PRECIO.    [GET]
    3. El cliente selecciona las herramientas que desea comprar, y estas se añaden al carrito de compras, actualizando el precio 
    total de acuerdo con el precio de compra de las herramientas seleccionadas.
    4. El cliente selecciona Comprar Herramientas.
    5. El sistema muestra la lista de herramientas seleccionadas incluyendo su NOMBRE, MATERIAL y PRECIO, y pide al cliente que     [POST]
    introduzca su NOMBRE, APELLIDOS, DIRECCIÓN DE ENVÍO y MÉTODO DE PAGO (tarjeta de crédito, PayPal o metálico), siendo todos
    ellos campos obligatorios, y de manera opcional un NÚMERO DE TELÉFONO y CORREO ELECTRÓNICO. De forma obligatoria, para cada
    herramienta seleccionada se pedirá la CANTIDAD a comprar y una breve DESCRIPCIÓN.
    6. El cliente rellena los datos y elige la opción Guardar.
    7. El sistema muestra la compra realizada, indicando los datos del cliente (NOMBRE y APELLIDOS), DIRECCIÓN DE ENVÍO, su         [DETAIL]
    PRECIO TOTAL, FECHA DE COMPRA y las herramientas compradas (NOMBRE, MATERIAL, PRECIO, DESCRIPCIÓN y CANTIDAD).

Flujo Alternativo 0 - al Paso 2:
    Si el sistema detecta que no hay herramientas disponibles para comprar se lo notificará al usuario.

Flujo Alternativo 1 - al Paso 2:
    1.1 El sistema ofrece al cliente la posibilidad de filtrar las herramientas por material y/o precio.
    1.2 El cliente fija los filtros que le interesan.
    1.3 El sistema muestra sólo las herramientas que cumplen los criterios de los filtros.

Flujo Alternativo 2 - al Paso 5:
    El cliente elige modificar el carrito de compras para borrar aquellas herramientas que no le interesan. Automáticamente,
    el sistema actualiza el precio total del contenido del carrito de acuerdo con el precio de compra de las herramientas seleccionadas.

Flujo Alternativo 3 - al Paso 4:
    Si el sistema detecta que no hay en el carrito ninguna herramienta para comprar, la opción para continuar el proceso no estará activa.

Flujo Alternativo 4 - al Paso 6:
    Si el sistema detecta que algún dato obligatorio no se ha rellenado, notificará al usuario y volverá al paso 5.

Flujo Alternativo 5 - al Paso 6:
    Si el sistema detecta que la cantidad que el usuario desea comprar de cualquier herramienta es 0, la opción de continuar el proceso no estará activa.

Flujo Alternativo 6 - al Paso 7:
    El sistema detecta que no hay cantidad suficiente de herramientas, informa al usuario del problema y muestra otra vez la vista de
    selección de herramientas volviendo al paso 4.

Precondición:
    El usuario debe estar conectado como Cliente para iniciar el caso de uso.
 */

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
        [Route("DetalleCompra")]
        // El tipo de respuesta es una lista de ComprasParaDetalleDTO
        [ProducesResponseType(typeof(IList<ComprasParaDetalleDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetalleHerramientasParaCompra(int id)
        {
            if (_context.Compras == null)
            {
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
                _logger.LogInformation("No se encontró la compra con id {Id}", id);
                return NotFound();
            }

            var compraDto = new ComprasParaDetalleDTO(
                compra.Usuario?.Nombre ?? string.Empty, // las interrogaciones y el string.Empty son por si el usuario es NULL
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
        [Route("CrearCompra")]
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

            // --- 2. VALIDAR ENTIDADES RELACIONADAS (Patrón del ejemplo) ---

            // a. Buscar Método de Pago
            var metodoPago = await _context.MetodosPagos.FindAsync(CrearCompraDTO.MetodoPagoId);
            if (metodoPago == null)
                ModelState.AddModelError(nameof(CrearCompraDTO.MetodoPagoId), $"El MetodoPagoId {CrearCompraDTO.MetodoPagoId} no existe.");

            // b. Buscar Usuario
            var Usuario = await _context.Users.FirstOrDefaultAsync(u=>u.Nombre == CrearCompraDTO.Nombre && u.Apellidos == CrearCompraDTO.Apellidos);
            if (Usuario == null) ModelState.AddModelError(nameof(CrearCompraDTO.Nombre), $"El usuario no existe.");

            // c. Validar items del dto (que no tengan valores imposibles)
            // Solo comprobación estructural mínima aquí: existencia de la lista y validación básica del IdHerramienta.
            // Las validaciones dependientes del nombre de la herramienta (descripción / cantidad) se hacen
            // dentro del bucle principal donde ya tenemos la herramienta cargada y su nombre.
            if (CrearCompraDTO.Items == null || !CrearCompraDTO.Items.Any())
                ModelState.AddModelError(nameof(CrearCompraDTO.Items), "La compra debe incluir al menos una herramienta.");

            // Validación básica de IdHerramienta (sigue interesando para ciertos tests)
            foreach (var itemDto in CrearCompraDTO.Items)
            {
                if (itemDto.IdHerramienta <= 0)
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"IdHerramienta inválido: {itemDto.IdHerramienta}.");
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
            // Validamos cada item con la información completa (herramienta cargada) para generar mensajes claros
            // y evitar excepciones en SaveChanges.
            foreach (var itemDTO in CrearCompraDTO.Items)
            {
                // Primero: la herramienta debe existir (comprobación en el diccionario que cargamos)
                if (!herramientasEnDB.TryGetValue(itemDTO.IdHerramienta, out var herramienta))
                {
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La HerramientaId {itemDTO.IdHerramienta} no existe.");
                    continue;
                }

                // Validaciones dependientes del nombre real de la herramienta (para mensajes legibles en tests)
                bool itemTieneError = false;

                // 1) descripción no nula -> tests esperan mensaje: "La herramienta Nombre - Herramienta3 no tiene descipción."
                if (string.IsNullOrWhiteSpace(itemDTO.DescripcionHerramienta))
                {
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La herramienta {herramienta.Nombre} no tiene descipción.");
                    itemTieneError = true;
                }

                // 2) cantidad: cero o negativa (mensajes distintos)
                if (itemDTO.CantidadHerramienta == 0)
                {
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La herramienta {herramienta.Nombre} tiene cantidad cero.");
                    itemTieneError = true;
                }
                else if (itemDTO.CantidadHerramienta < 0)
                {
                    ModelState.AddModelError(nameof(CrearCompraDTO.Items), $"La herramienta {herramienta.Nombre} tiene cantidad negativa.");
                    itemTieneError = true;
                }

                // Si hubo errores para este item, no lo añadimos a la compra (se devolverán todos al final).
                if (itemTieneError)
                    continue;

                // Si llegamos aquí, el item es válido: lo añadimos a la nueva compra
                var nuevoItem = new CompraItem
                {
                    Herramienta = herramienta,
                    Compra = nuevaCompra,
                    Cantidad = itemDTO.CantidadHerramienta,
                    Descripcion = itemDTO.DescripcionHerramienta,
                };
                nuevaCompra.CompraItems.Add(nuevoItem);
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

                // Mapeamos los items desde los objetos en memoria incluyendo datos de la herramienta
                nuevaCompra.CompraItems.Select(oi => new CompraItemsDTO(
                    oi.Herramienta.Id,         // IdHerramienta
                    oi.Herramienta.Nombre,     // NombreHerramienta
                    oi.Herramienta.Material,   // MaterialHerramienta
                    oi.Herramienta.Precio,     // PrecioHerramienta (float)
                    oi.Descripcion,            // DescripcionHerramienta
                    oi.Cantidad                // CantidadHerramienta
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
