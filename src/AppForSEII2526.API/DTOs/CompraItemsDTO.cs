



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
        public int StockHerramienta { get; set; }

        // Constructor completo
        public CompraItemsDTO(int idHerramienta, string nombreHerramienta, string materialHerramienta, float precioHerramienta, string descripcionHerramienta, int cantidadHerramienta, int stockHerramienta)
        {
            IdHerramienta = idHerramienta;
            NombreHerramienta = nombreHerramienta;
            MaterialHerramienta = materialHerramienta;
            PrecioHerramienta = precioHerramienta;
            DescripcionHerramienta = descripcionHerramienta;
            CantidadHerramienta = cantidadHerramienta;
            StockHerramienta = stockHerramienta;
        }

        // Constructor vacío para que el Json lo use por defecto al serializar (sino, al haber más de uno no sabe cual usar y peta)
        public CompraItemsDTO() { }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemsDTO dTO &&
                   IdHerramienta == dTO.IdHerramienta &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   MaterialHerramienta == dTO.MaterialHerramienta &&
                   PrecioHerramienta == dTO.PrecioHerramienta &&
                   DescripcionHerramienta == dTO.DescripcionHerramienta &&
                   CantidadHerramienta == dTO.CantidadHerramienta &&
                   StockHerramienta == dTO.StockHerramienta;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreHerramienta, MaterialHerramienta, PrecioHerramienta, DescripcionHerramienta, CantidadHerramienta, StockHerramienta);
        }
    }
}
