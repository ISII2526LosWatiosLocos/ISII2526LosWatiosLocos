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

        public Reparación Reparacion { get; set; }

        public Herramienta Herramienta
        {
            get; set;

        }

        public ReparaciónItem(int reparacionId, int herramientaId, int cantidad, string? descripción, float precio, Reparación reparacion, Herramienta herramienta)
        {
            ReparacionId = reparacionId;
            HerramientaId = herramientaId;
            this.cantidad = cantidad;
            Descripción = descripción;
            Precio = precio;
            Reparacion = reparacion;
            Herramienta = herramienta;
        }

        public ReparaciónItem( int cantidad, string? descripción, float precio, Reparación reparación, Herramienta herramienta)
        {
           
            this.cantidad = cantidad;
            Descripción = descripción;
            Precio = precio;
            Reparacion = reparación;
            Herramienta = herramienta;
        }



        public ReparaciónItem()
        {
        }
    }
}
