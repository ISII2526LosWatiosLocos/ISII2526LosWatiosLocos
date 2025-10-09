namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(AlquilerId), nameof(HerramientaId))]
    public class AlquilarItem
    {
        public int AlquilerId { get; set; }

        public int HerramientaId { get; set; }
        [Required]
        public float Precio { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }
        // Relaciones
        public Alquiler Alquiler { get; set; }
        public Herramienta Herramienta { get; set; }
    }
}
