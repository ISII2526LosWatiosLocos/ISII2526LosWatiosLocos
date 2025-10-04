namespace AppForSEII2526.API.Models
{
    public class ReparaciónItem
    { 
        [Key]
        public int IdReparación { get; set; }

        [Required]
        public int cantidad { get; set; }

        // descripciión es el único atributo no obligatorio de ets a clase
        public String? Descripción { get; set; }

        [Required]
        public int IdHerramienta { get; set; }

        [Required]
        public float Precio { get; set; }

        //Relaciones

        public Reparación Reparación { get; set; }

        public Herramienta Herramienta
        {
            get; set;

        }
    }
}
