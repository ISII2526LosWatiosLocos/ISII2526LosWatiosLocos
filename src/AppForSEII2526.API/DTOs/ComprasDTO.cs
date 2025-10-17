namespace AppForSEII2526.API.DTOs
{
    public class ComprasDTO
    {
        [Key]
        public int Id { get; set; }

        // Campos obligatorios

        [Required]
        public string DirecciónEnvío { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }

        // Relaciones

        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MétodoPago { get; set; }
        public ApplicationUser Usuario { get; set; }

        public ComprasDTO(int Id, string DirecciónEnvío, DateOnly FechaCompra, float PrecioTotal, List<CompraItem> CompraItems, MetodosPago MétodoPago, ApplicationUser Usuario)
        {
            this.Id = Id;
            this.DirecciónEnvío = DirecciónEnvío;
            this.FechaCompra = FechaCompra;
            this.PrecioTotal = PrecioTotal;
            this.CompraItems = CompraItems;
            this.MétodoPago = MétodoPago;
            this.Usuario = Usuario;
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
