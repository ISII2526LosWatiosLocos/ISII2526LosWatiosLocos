namespace AppForSEII2526.API.DTOs
{
    public class CrearOfertaItemDTO
    {
        // El ID de la Herramienta a la que se aplica la oferta
        public int HerramientaId { get; set; }

        // El porcentaje de descuento (p.ej., 20 para un 20%)
        public int PorcentajeDescuento { get; set; }
    }
}
