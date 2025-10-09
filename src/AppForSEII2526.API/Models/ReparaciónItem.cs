namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ReparaciónId), nameof(HerramientaId))]
    public class ReparaciónItem
    {

        public int ReparaciónId { get; set; }
        public int HerramientaId { get; set; }
  
        [Required]
        public int cantidad { get; set; }

        // descripción es el único atributo no obligatorio de ets a clase
        public String? Descripción { get; set; }


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
