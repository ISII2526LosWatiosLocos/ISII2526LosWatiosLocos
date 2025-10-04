namespace AppForSEII2526.API.Models
{
    public class OfertaItem
    {
        [Key]
        public int IdOferta { get; set; }

        [Required]
        public int IdHerramienta { get; set; }

        [Required(ErrorMessage = "El porcentaje es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
        public int Porcentaje { get; set; }

        [Required]
        public float PrecioFinal { get; set; }

        //Relaciones

        public Oferta Oferta { get; set; }
        public Herramienta Herramienta { get; set; }
    }
}
