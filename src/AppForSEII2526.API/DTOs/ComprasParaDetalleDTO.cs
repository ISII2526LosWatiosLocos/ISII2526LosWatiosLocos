namespace AppForSEII2526.API.DTOs
{
    public class ComprasParaDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DireccionEnvio { get; set; }
        public float PrecioTotal { get; set; }
        public DateOnly FechaCompra { get; set; }
        public IList<CompraItemsDTO> Items { get; set; } // Lista de Items

        // Constructor completo
        public ComprasParaDetalleDTO(int Id, string Nombre, string Apellidos, string DireccionEnvio, float PrecioTotal, DateOnly FechaCompra, IList<CompraItemsDTO> Items)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellidos = Apellidos;
            this.DireccionEnvio = DireccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.FechaCompra = FechaCompra;
            this.Items = Items;
            
        }

        // Constructor sin la Id para las pruebas
        public ComprasParaDetalleDTO(string Nombre, string Apellidos, string DireccionEnvio, float PrecioTotal, DateOnly FechaCompra, IList<CompraItemsDTO> Items)
        {
            this.Nombre = Nombre;
            this.Apellidos = Apellidos;
            this.DireccionEnvio = DireccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.FechaCompra = FechaCompra;
            this.Items = Items;

        }

        public override bool Equals(object? obj)
        {
            return obj is ComprasParaDetalleDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   DireccionEnvio == dTO.DireccionEnvio &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaCompra.Equals(dTO.FechaCompra) &&
                   // La anterior implementación comparaba la dirección de memoria, no el contenido
                   Items.OrderBy(i => i.IdHerramienta).SequenceEqual(dTO.Items.OrderBy(i => i.IdHerramienta));

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Apellidos, DireccionEnvio, PrecioTotal, FechaCompra, Items);
        }
    }
}
