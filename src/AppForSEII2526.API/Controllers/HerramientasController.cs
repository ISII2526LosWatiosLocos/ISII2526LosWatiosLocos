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
        [ProducesResponseType(typeof(IList<HerramientasDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaCompra()
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Select(h => new HerramientasDTO(
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
                    (filtroTiempoReparacion == null || h.TiempoReparación <= filtroTiempoReparacion))
                .Select(h => new HerramientasParaReparaciónDTO(
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio, h.TiempoReparación))
                .ToListAsync();
            return Ok(herramientas);

        }


        [HttpGet]
        [Route("Detalle-Oferta")]
        // Corregido: El tipo de respuesta es una lista de OfertasDTO
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
                o.Id,
                o.FechaFinal,
                o.FechaInicio,
                o.FechaOferta,
                o.TipoDirigida.ToString(), 
                
                o.MetodosPago.Nombre, 

                
                o.Items.Select(oi => new HerramientasDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Fabricante.Nombre,
                    oi.Herramienta.Precio, 
                    oi.PrecioFinal        
                )).ToList()

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


                o.CompraItems.Select(oi => new HerramientasDTO(
                    oi.Herramienta.Nombre,
                    oi.Herramienta.Material,
                    oi.Herramienta.Precio
                )).ToList()

            )).ToList();

            return Ok(comprasDTO);
        }

        [HttpGet]
        [Route("Detalle-Reparacion")]
        [ProducesResponseType(typeof(IList<ReparacionesDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDetalleHerramientasParaReparacion()
        {
            var reparaciones = await _context.Reparaciones
                .Include(r => r.Usuario)
                .Include(r => r.MétodoPago)
                .Include(r => r.ReparaciónItems)
                    .ThenInclude(ri => ri.Herramienta)
                        .ThenInclude(h => h.Fabricante)
                .ToListAsync();

            var reparacionesDTO = reparaciones.Select(r => new ReparacionesDTO(
                r.Usuario.Nombre,
                r.Usuario.Apellidos,
                r.FechaEntrega,
                r.FechaRecogida,
                r.PrecioTotal,
                r.ReparaciónItems.Select(ri => new HerramientasDTO(
                    ri.Herramienta.Nombre,
                    ri.Herramienta.Precio,
                    ri.Herramienta.Descripción,
                    ri.Herramienta.Cantidad
                )
              ).ToList()
            )).ToList();

            return Ok(reparacionesDTO);
        }

    }

}
