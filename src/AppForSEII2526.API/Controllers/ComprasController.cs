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
        public async Task<IActionResult> GetDetalleHerramientasParaCompra()
        {
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
                o.CompraItems.Select(oi => oi.Herramienta.Nombre).FirstOrDefault()!,
                o.CompraItems.Select(oi => oi.Herramienta.Material).FirstOrDefault()!,
                o.CompraItems.Select(oi => oi.Herramienta.Precio).FirstOrDefault()!,
                o.CompraItems.Select(oi => oi.Descripción).FirstOrDefault()!,
                o.CompraItems.Select(oi => oi.Cantidad).FirstOrDefault()!

            )).ToList();

            return Ok(comprasParaDetalleDTO);
        }
    }
}