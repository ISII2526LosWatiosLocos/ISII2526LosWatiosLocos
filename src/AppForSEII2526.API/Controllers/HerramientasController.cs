using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HerramientasController : ControllerBase
    {

        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;

        //used to log any information when your system is running
        private readonly ILogger<HerramientasController> _logger;

        public HerramientasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("Para-Oferta")]
        [ProducesResponseType(typeof(IList<HerramientasParaOfertarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaOferta(string? filtroFabricante, float? filtroPrecio)
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where (h => (filtroFabricante == null || h.Fabricante.Nombre == filtroFabricante) &&
                            (filtroPrecio == null || h.Precio <= filtroPrecio))
                .Select(h => new HerramientasParaOfertarDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio))
                .ToListAsync();
            return Ok(herramientas);

        }
        
        [HttpGet]
        [Route("Para-Alquiler")]
        [ProducesResponseType(typeof(IList<HerramientasParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaAlquiler(String? filtroNombre, String? filtroMaterial)
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => (filtroNombre == null || filtroNombre == h.Nombre) &&
                            (filtroMaterial == null || filtroMaterial == h.Material))
                .Select(h => new HerramientasParaAlquilarDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio))
                .ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("Para-Compra")]
        [ProducesResponseType(typeof(IList<HerramientasParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaCompra(string? filtroMaterial, float? filtroPrecio)
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => (filtroMaterial == null || h.Material == filtroMaterial) &&
                            (filtroPrecio == null || h.Precio <= filtroPrecio))
                .Select(h => new HerramientasParaComprarDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio))
                .ToListAsync();
            return Ok(herramientas);
        }

        [HttpGet]
        [Route("Para-Reparación")]
        [ProducesResponseType(typeof(IList<HerramientasParaReparaciónDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaReparación(string? filtroNombre, int? filtroTiempoReparacion)
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where(h => (filtroNombre == null || h.Nombre == filtroNombre) &&
                    (filtroTiempoReparacion == null || h.TiempoReparacion <= filtroTiempoReparacion))
                .Select(h => new HerramientasParaReparaciónDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio, h.TiempoReparacion))
                .ToListAsync();
            return Ok(herramientas);

        }


        [HttpGet]
        [Route("Detalle-Oferta")]
        [ProducesResponseType(typeof(IList<OfertasDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaOferta()
        {
            var ofertas = await _context.Ofertas
                .Include(o => o.MetodosPago)
                .Include(o => o.Items) 
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();

           
            var ofertasDTO = ofertas.Select(o => new OfertasDTO(
                o.FechaFinal,
                o.FechaInicio,
                o.FechaOferta,
                o.TipoDirigida.ToString(),
                o.MetodosPago.Nombre,
                o.Items.Select(oi => oi.Herramienta.Nombre).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Material).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Fabricante.Nombre).FirstOrDefault()!,
                o.Items.Select(oi => oi.Herramienta.Precio).FirstOrDefault(),
                o.Items.Select(oi => oi.PrecioFinal).FirstOrDefault()

            )).ToList();

            return Ok(ofertasDTO);
        }

        [HttpGet]
        [Route("Detalle-Compra")]
        // El tipo de respuesta es una lista de ComprasDTO
        [ProducesResponseType(typeof(IList<ComprasDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaCompra()
        {
            var compras = await _context.Compras
                .Include(o => o.MétodoPago)
                .Include(o => o.Usuario)
                .Include(o => o.CompraItems)
                    .ThenInclude(oi => oi.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();


            var comprasDTO = compras.Select(o => new ComprasDTO(
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

            return Ok(comprasDTO);
        }
    }

}
