
namespace AppForSEII2526.API.Models
{

    public class Compra
    {
        [Key]
        public int Id { get; set; }

        // Campos obligatorios

        [Required]
        public string DireccionEnvio { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }

        // Relaciones

        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MetodoPago { get; set; }
        public ApplicationUser Usuario { get; set; }
    }
}
