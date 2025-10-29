namespace AppForSEII2526.API.DTOs
{
    public class OfertasParaDetalleDTO
    {
        public DateOnly FechaFinal { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaOferta { get; set; }
        public string TipoDirigida { get; set; }
        public string MetodoPago { get; set; }

        //Lista de items
        public IList<OfertaItemsDTO> Items { get; set; }

        public OfertasParaDetalleDTO(DateOnly fechaFinal, DateOnly fechaInicio, DateOnly fechaOferta, string tipoDirigida, string metodoPago, IList<OfertaItemsDTO> items)

        {
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            FechaOferta = fechaOferta;
            TipoDirigida = tipoDirigida;
            MetodoPago = metodoPago;
            Items = items;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertasParaDetalleDTO dTO &&
                   FechaFinal == dTO.FechaFinal &&
                   FechaInicio == dTO.FechaInicio &&
                   FechaOferta == dTO.FechaOferta &&
                   TipoDirigida == dTO.TipoDirigida &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<IList<OfertaItemsDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaFinal, FechaInicio, FechaOferta, TipoDirigida, MetodoPago, Items);
        }
    }
}

