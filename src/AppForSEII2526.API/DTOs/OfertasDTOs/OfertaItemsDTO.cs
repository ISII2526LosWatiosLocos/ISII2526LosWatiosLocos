
namespace AppForSEII2526.API.DTOs.OfertasDTOs
{
    public class OfertaItemsDTO
    {
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public string FabricanteHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public float PrecioFinalOferta { get; set; }
        // El ID de la Herramienta a la que se aplica la oferta
        public int HerramientaId { get; set; }

        // El porcentaje de descuento (p.ej., 20 para un 20%)
        public int PorcentajeDescuento { get; set; }

        public OfertaItemsDTO(string nombreHerramienta, string materialHerramienta, string fabricanteHerramienta, float precioHerramienta, float precioFinalOferta)
        {
            NombreHerramienta = nombreHerramienta;
            MaterialHerramienta = materialHerramienta;
            FabricanteHerramienta = fabricanteHerramienta;
            PrecioHerramienta = precioHerramienta;
            PrecioFinalOferta = precioFinalOferta;
        }

        public OfertaItemsDTO() { } 

        public OfertaItemsDTO(int herramientaId, int porcentajeDescuento)
        {
            HerramientaId = herramientaId;
            PorcentajeDescuento = porcentajeDescuento;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertaItemsDTO dTO &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   FabricanteHerramienta == dTO.FabricanteHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   PrecioFinalOferta == dTO.PrecioFinalOferta &&
                   HerramientaId == dTO.HerramientaId &&
                   PorcentajeDescuento == dTO.PorcentajeDescuento;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreHerramienta, MaterialHerramienta, FabricanteHerramienta, PrecioHerramienta, PrecioFinalOferta, HerramientaId, PorcentajeDescuento);
        }
    }
}
