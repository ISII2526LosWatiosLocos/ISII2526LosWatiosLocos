

namespace AppForSEII2526.API.DTOs
{
    public class CompraItemsDTO
    {
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public string DescripcionHerramienta { get; set; }
        public int CantidadHerramienta { get; set; }

        public CompraItemsDTO(string NombreHerramienta, string MaterialHerramienta, float PrecioHerramienta, string DescripcionHerramienta, int CantidadHerramienta)
        {
            this.NombreHerramienta = NombreHerramienta;
            this.MaterialHerramienta = MaterialHerramienta;
            this.PrecioHerramienta = PrecioHerramienta;
            this.DescripcionHerramienta = DescripcionHerramienta;
            this.CantidadHerramienta = CantidadHerramienta;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemsDTO dTO &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   DescripcionHerramienta == dTO.DescripcionHerramienta &&
                   CantidadHerramienta == dTO.CantidadHerramienta;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreHerramienta, MaterialHerramienta, PrecioHerramienta, DescripcionHerramienta, CantidadHerramienta);
        }
    }
}
