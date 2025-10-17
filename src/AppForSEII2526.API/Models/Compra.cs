using AppForSEII2526.API.DTOs;

namespace AppForSEII2526.API.Models
{

    public class Compra
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
    }
}
