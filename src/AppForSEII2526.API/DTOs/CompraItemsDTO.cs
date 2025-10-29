



namespace AppForSEII2526.API.DTOs
{
    public class CompraItemsDTO
    {
        public int IdHerramienta { get; set; }
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public string DescripcionHerramienta { get; set; }
        public int CantidadHerramienta { get; set; }

        public CompraItemsDTO(int idHerramienta, string nombreHerramienta, string materialHerramienta, float precioHerramienta, string descripcionHerramienta, int cantidadHerramienta)
        {
            IdHerramienta = idHerramienta;
            NombreHerramienta = nombreHerramienta;
            MaterialHerramienta = materialHerramienta;
            PrecioHerramienta = precioHerramienta;
            DescripcionHerramienta = descripcionHerramienta;
            CantidadHerramienta = cantidadHerramienta;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemsDTO dTO &&
                   IdHerramienta == dTO.IdHerramienta &&
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
