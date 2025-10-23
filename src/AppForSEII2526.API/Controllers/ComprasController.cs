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
        private readonly ILogger<HerramientasController> _logger;

        public ComprasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
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
                .Include(o => o.MétodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.CompraItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var comprasParaDetalleDTO = comprasParaDetalle.Select(o => new ComprasParaDetalleDTO(
                o.Usuario.Nombre,
                o.Usuario.Apellidos,
                o.DirecciónEnvío,
                o.PrecioTotal,
                o.FechaCompra,
                o.CompraItems.Select(oi => new CompraItemsDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio,
                    oi.Descripción,
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
    }
}