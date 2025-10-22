namespace AppForSEII2526.API.DTOs
{
    public class OfertasParaDetalleDTO
    {
        public DateTime FechaFinal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaOferta { get; set; }
        public string TipoDirigida { get; set; }
        public string MetodoPago { get; set; }

        //Lista de items
        public List<OfertaItemsDTO> Items { get; set; }


        public OfertasParaDetalleDTO(DateTime fechaFinal, DateTime fechaInicio, DateTime fechaOferta, string tipoDirigida, string metodoPago, List<OfertaItemsDTO> items)
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
                   EqualityComparer<List<OfertaItemsDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaFinal, FechaInicio, FechaOferta, TipoDirigida, MetodoPago, Items);
        }
    }
}

