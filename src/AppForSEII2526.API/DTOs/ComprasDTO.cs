namespace AppForSEII2526.API.DTOs
{
    public class ComprasDTO
    {
        private string nombre;
        private string apellidos;
        
        public int Id { get; set; }
        public string DirecciónEnvío { get; set; }
        public DateOnly FechaCompra { get; set; }
        public float PrecioTotal { get; set; }
        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MétodoPago { get; set; }
        public ApplicationUser Usuario { get; set; }

        public ComprasDTO(int Id, string DirecciónEnvío, DateOnly FechaCompra, float PrecioTotal, List<CompraItem> CompraItems, MetodosPago MétodoPago, ApplicationUser Usuario)
        {
            this.Id = Id;
            this.DirecciónEnvío = DirecciónEnvío;
            this.FechaCompra = FechaCompra;
            this.PrecioTotal = PrecioTotal;
            this.CompraItems = CompraItems.ToList();
            this.MétodoPago = MétodoPago;
            this.Usuario = Usuario;
        }

        public ComprasDTO(string nombre, string apellidos, string direcciónEnvío, float precioTotal, DateOnly fechaCompra)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            DirecciónEnvío = direcciónEnvío;
            PrecioTotal = precioTotal;
            FechaCompra = fechaCompra;
        }

        public override bool Equals(object? obj)
        {
            return obj is ComprasDTO dTO &&
                   Id == dTO.Id &&
                   DirecciónEnvío == dTO.DirecciónEnvío &&
                   FechaCompra.Equals(dTO.FechaCompra) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<List<CompraItem>>.Default.Equals(CompraItems, dTO.CompraItems) &&
                   EqualityComparer<MetodosPago>.Default.Equals(MétodoPago, dTO.MétodoPago) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Usuario, dTO.Usuario);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, DirecciónEnvío, FechaCompra, PrecioTotal, CompraItems, MétodoPago, Usuario);
        }
    }
}
