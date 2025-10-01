namespace AppForSEII2526.API.Models
{
    public class ReparaciónItem
    {
        [Key]
        public int IdReparación { get; set; }

        [Required]
        public int cantidad { get; set; }

        public String Descripción { get; set; }

        public int IdHerramienta { get; set; }

        public float Precio { get; set; }
    }
}
