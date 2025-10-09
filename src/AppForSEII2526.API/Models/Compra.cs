namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraId))]
    public class Compra
    {

        public int CompraId { get; set; }

        [Required]
        public string DirecciónEnvío { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }

        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MetodoPago { get; set; }
    }
}
