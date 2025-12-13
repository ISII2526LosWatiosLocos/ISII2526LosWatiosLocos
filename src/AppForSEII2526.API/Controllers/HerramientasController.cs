using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.AlquileresDTOs;
using AppForSEII2526.API.DTOs.ComprasDTOs;
using AppForSEII2526.API.DTOs.OfertasDTOs;
using AppForSEII2526.API.Models;
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
        [Route("ParaOferta")]
        [ProducesResponseType(typeof(IList<HerramientasParaOfertarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaOferta(string? filtroFabricante, float? filtroPrecio)
        {
            var herramientas = await _context.Herramientas
                .Include(h => h.Fabricante)
                .Where (h => (filtroFabricante == null || h.Fabricante.Nombre == filtroFabricante) &&
                            (filtroPrecio == null || h.Precio <= filtroPrecio))
                .Select(h => new HerramientasParaOfertarDTO(
                    h.Id, h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio))
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
                    h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio, h.Id))
                .ToListAsync();
            return Ok(herramientas);
        }
        /*
        Flujo Básico:
            1. El cliente selecciona Comprar Herramientas en el menú principal.
            2. El Sistema muestra la lista de herramientas disponibles para comprar, indicando su NOMBRE, MATERIAL, FABRICANTE y PRECIO.    [GET]
        */
        [HttpGet]
        [Route("Para-Compra")]
        [ProducesResponseType(typeof(IList<HerramientasParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaCompra(string? filtroMaterial, float? filtroPrecio) // Como están marcados por la ? (osea, son opcionales) si alguno de los filtros es null no pasa nada
        {
            var herramientas = await _context.Herramientas // Accede al conjunto de herramientas de la BBDD y carga sus datos
                .Include(h => h.Fabricante) // Carga también los datos relacionados al fabricante de cada herramienta
                .Where(h => (filtroMaterial == null || h.Material == filtroMaterial) &&
                            (filtroPrecio == null || h.Precio <= filtroPrecio)) // Filtra según los parámetros que le paso arriba, así cubro el flujo alternativo 1
                .Select(h => new HerramientasParaComprarDTO(
                    h.Id, h.Nombre, h.Material, h.Fabricante.Nombre, h.Precio)) // Creo un DTO para cada herramienta, así solo devuelvo los 5 campos que necesito y no todo el objeto.
                .ToListAsync(); // Consulto los datos de forma asíncrona
            if (!herramientas.Any())
                return NoContent(); // Lanzo error 204, así cubro el flujo alternativo 0 
            return Ok(herramientas); // Devuelvo la lista de HerramientasParaComprarDTO

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

    }

}
