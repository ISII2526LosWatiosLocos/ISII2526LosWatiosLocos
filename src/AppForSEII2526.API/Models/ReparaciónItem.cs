namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ReparacionId), nameof(HerramientaId))]
    public class ReparaciónItem
    {

        public int ReparacionId { get; set; }
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

        public ReparaciónItem(int reparacionId, int cantidad, string? descripción, float precio, Reparación reparación, Herramienta herramienta)
        {
            ReparacionId = reparacionId;
           
            this.cantidad = cantidad;
            Descripción = descripción;
            Precio = precio;
            Reparación = reparación;
            Herramienta = herramienta;
        }
        public ReparaciónItem( int cantidad, string? descripción, float precio, Reparación reparación, Herramienta herramienta)
        {
           
            this.cantidad = cantidad;
            Descripción = descripción;
            Precio = precio;
            Reparación = reparación;
            Herramienta = herramienta;
        }



        public ReparaciónItem()
        {
        }
    }
}
