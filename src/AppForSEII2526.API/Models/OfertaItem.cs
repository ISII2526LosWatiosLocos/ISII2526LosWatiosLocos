namespace AppForSEII2526.API.Models
{
    public class OfertaItem
    {
        [Key]
        public int IdOferta { get; set; }

        [Required]
        public int IdHerramienta { get; set; }

        public int Porcentaje { get; set; }

        public float PrecioFinal { get; set; }

        //Relaciones

        public Oferta Oferta { get; set; }
        public Herramienta Herramienta { get; set; }
    }
}
