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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }
        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MetodoPago { get; set; }

        public int ?Teléfono { get; set; }

        public string ?CorreoElectrónico { get; set; }
    }
}
