namespace AppForSEII2526.API.Models
{
    public class ReparaciónItem
    { 
        [Key]
        public int IdReparación { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        public String Descripción { get; set; }

        [Required]
        public int IdHerramienta { get; set; }

        [Required]
        public float Precio { get; set; }

        //Relaciones
        public Herramienta Herramienta
        {
            get; set;

        }
    }
}
