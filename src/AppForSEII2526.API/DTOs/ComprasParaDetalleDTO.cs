namespace AppForSEII2526.API.DTOs
{
    public class ComprasParaDetalleDTO
    {
        private string Nombre { get; set; }
        private string Apellidos { get; set; }
        public string DireccionEnvio { get; set; }
        public float PrecioTotal { get; set; }
        public DateTime FechaCompra { get; set; }
        public IList<CompraItemsDTO> Items { get; set; } // Lista de Items

        public ComprasParaDetalleDTO(string Nombre, string Apellidos, string DireccionEnvio, float PrecioTotal, DateTime FechaCompra, IList<CompraItemsDTO> Items)
        {
            this.Nombre = Nombre;
            this.Apellidos = Apellidos;
            this.DireccionEnvio = DireccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.FechaCompra = FechaCompra;
            Items = Items;
            
        }
        public override bool Equals(object? obj)
        {
            return obj is ComprasParaDetalleDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   DireccionEnvio == dTO.DireccionEnvio &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaCompra.Equals(dTO.FechaCompra) &&
                   EqualityComparer<IList<CompraItemsDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Apellidos, DireccionEnvio, PrecioTotal, FechaCompra, Items);
        }
    }
}
