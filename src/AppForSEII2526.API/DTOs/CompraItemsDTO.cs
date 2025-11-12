



namespace AppForSEII2526.API.DTOs
{
    public class CompraItemsDTO
    {
        public int IdHerramienta { get; set; }
        public DateOnly FechaFabricacionHerramienta { get; set; }
        public string NombreHerramienta { get; set; }
        public string MaterialHerramienta { get; set; }
        public float PrecioHerramienta { get; set; }
        public string DescripcionHerramienta { get; set; }
        public int CantidadHerramienta { get; set; }

        // Constructor completo
        public CompraItemsDTO(int idHerramienta, DateOnly fechaFabricacionHerramienta, string nombreHerramienta, string materialHerramienta, float precioHerramienta, string descripcionHerramienta, int cantidadHerramienta)
        {
            IdHerramienta = idHerramienta;
            FechaFabricacionHerramienta = fechaFabricacionHerramienta;
            NombreHerramienta = nombreHerramienta;
            MaterialHerramienta = materialHerramienta;
            PrecioHerramienta = precioHerramienta;
            DescripcionHerramienta = descripcionHerramienta;
            CantidadHerramienta = cantidadHerramienta;
        }

        // Constructor vacío para que el Json lo use por defecto al serializar (sino, al haber más de uno no sabe cual usar y peta)
        public CompraItemsDTO() { }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemsDTO dTO &&
                   IdHerramienta == dTO.IdHerramienta &&
                   FechaFabricacionHerramienta.Equals(dTO.FechaFabricacionHerramienta) &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   DescripcionHerramienta == dTO.DescripcionHerramienta &&
                   CantidadHerramienta == dTO.CantidadHerramienta;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaFabricacionHerramienta, NombreHerramienta, MaterialHerramienta, PrecioHerramienta, DescripcionHerramienta, CantidadHerramienta);
        }
    }
}
