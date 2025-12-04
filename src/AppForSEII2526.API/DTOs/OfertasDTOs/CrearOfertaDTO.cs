namespace AppForSEII2526.API.DTOs.OfertasDTOs
{
    public class CrearOfertaDTO
    {
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinal { get; set; }
        public string? TipoDirigida { get; set; }
        public int MetodoPagoId { get; set; }

        public string nombreUsuario { get; set; }   
        public List<OfertaItemsDTO> Items { get; set; }

        public CrearOfertaDTO()
        {
            Items = new List<OfertaItemsDTO>();
        }

        public override bool Equals(object? obj)
        {
            return obj is CrearOfertaDTO dTO &&
                   FechaInicio.Equals(dTO.FechaInicio) &&
                   FechaFinal.Equals(dTO.FechaFinal) &&
                   TipoDirigida == dTO.TipoDirigida &&
                   MetodoPagoId == dTO.MetodoPagoId &&
                   nombreUsuario == dTO.nombreUsuario &&
                   EqualityComparer<List<OfertaItemsDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaInicio, FechaFinal, TipoDirigida, MetodoPagoId, nombreUsuario, Items);
        }
    }
}
