namespace AppForSEII2526.API.Models
{
    public class AlquilarItem
    {
        [Key]
        public int IdAlquiler { get; set; }

        [Required]
        public int IdHerramienta { get; set; }
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
