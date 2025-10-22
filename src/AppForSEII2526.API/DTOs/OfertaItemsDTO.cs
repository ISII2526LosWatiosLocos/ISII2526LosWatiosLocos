
namespace AppForSEII2526.API.DTOs
{
    public class OfertaItemsDTO
    {
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public string FabricanteHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public float PrecioFinalOferta { get; set; }

        public OfertaItemsDTO(string nombreHerramienta, string materialHerramienta, string fabricanteHerramienta, float precioHerramienta, float precioFinalOferta)
        {
            NombreHerramienta = nombreHerramienta;
            MaterialHerramienta = materialHerramienta;
            FabricanteHerramienta = fabricanteHerramienta;
            PrecioHerramienta = precioHerramienta;
            PrecioFinalOferta = precioFinalOferta;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertaItemsDTO dTO &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   FabricanteHerramienta == dTO.FabricanteHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   PrecioFinalOferta == dTO.PrecioFinalOferta;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreHerramienta, MaterialHerramienta, FabricanteHerramienta, PrecioHerramienta, PrecioFinalOferta);
        }
    }
}
