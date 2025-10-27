namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraId), nameof(HerramientaId))]
    public class CompraItem
    {

        public int CompraId { get; set; }
        public int HerramientaId { get; set; }

        [Required]
        public int Cantidad { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public float Precio { get; set; }

        // Relaciones

        public Herramienta Herramienta { get; set; }
        public Compra Compra { get; set; }

        // Constructor completo
        public CompraItem(int compraId, int herramientaId, int cantidad, string descripcion, float precio, Herramienta herramienta, Compra compra)
        {
            CompraId = compraId;
            HerramientaId = herramientaId;
            Cantidad = cantidad;
            Descripcion = descripcion;
            Precio = precio;
            Herramienta = herramienta;
            Compra = compra;
        }
        // Constructor sin el ID para las pruebas
        public CompraItem(int herramientaId, int cantidad, string descripcion, float precio, Herramienta herramienta, Compra compra)
        {
            HerramientaId = herramientaId;
            Cantidad = cantidad;
            Descripcion = descripcion;
            Precio = precio;
            Herramienta = herramienta;
            Compra = compra;
        }
    }
}
