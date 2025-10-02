namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApellidoCliente { get; set; }

        public string CorreoElectrónico { get; set; }

        public string DirecciónEnvío { get; set; }

        public DateOnly FechaCompra { get; set; }

        public string NombreCliente { get; set; }

        public float PrecioTotal { get; set; }

        public int Teléfono { get; set; }
    }
}
