namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreCliente { get; set; }
        [Required]
        public string ApellidoCliente { get; set; }
        [Required]
        public string DirecciónEnvío { get; set; }
        [Required]
        public DateOnly FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }
        [Required]
        public List<CompraItem> CompraItems { get; set; }
        [Required]
        public MetodosPago MetodoPago { get; set; }

        public int Teléfono { get; set; }

        public string CorreoElectrónico { get; set; }
    }
}
