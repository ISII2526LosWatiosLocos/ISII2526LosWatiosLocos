namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(OfertaId), nameof(HerramientaId))]
    public class OfertaItem
    {
        public int OfertaId { get; set; }

        public int HerramientaId { get; set; }

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
