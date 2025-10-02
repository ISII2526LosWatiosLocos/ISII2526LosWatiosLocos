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
        public int Cantidad { get; set; }
    }
}
