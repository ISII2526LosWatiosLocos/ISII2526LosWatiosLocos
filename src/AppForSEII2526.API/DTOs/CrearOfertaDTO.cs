namespace AppForSEII2526.API.DTOs
{
    public class CrearOfertaDTO
    {
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinal { get; set; }
        public string? TipoDirigida { get; set; }
        public int MetodoPagoId { get; set; }
        public List<CrearOfertaItemDTO> Items { get; set; }

        public CrearOfertaDTO()
        {
            Items = new List<CrearOfertaItemDTO>();
        }


    }
}
