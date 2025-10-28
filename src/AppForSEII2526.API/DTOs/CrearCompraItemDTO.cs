namespace AppForSEII2526.API.DTOs
{
    public class CrearCompraItemDTO
    {
        // El ID de la Herramienta a la que se aplica la compra
        public int CompraId { get; set; }
        public int HerramientaId { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
    }
}
