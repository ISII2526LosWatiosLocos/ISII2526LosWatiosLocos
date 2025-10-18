namespace AppForSEII2526.API.DTOs
{
    public class OfertasDTO
    {
        public int Id { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaOferta { get; set; }
        public string TipoDirigida { get; set; }
        public string MetodoPago { get; set; }
        public List<HerramientasDTO> Items { get; set; }

        // El constructor que usaremos desde el controlador
        public OfertasDTO(
            int id,
            DateTime fechaFinal,
            DateTime fechaInicio,
            DateTime fechaOferta,
            string tipoDirigida,
            string metodoPago, 
            List<HerramientasDTO> items 
        )
        {
            Id = id;
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            FechaOferta = fechaOferta;
            TipoDirigida = tipoDirigida;
            MetodoPago = metodoPago;
            Items = items;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertasDTO dTO &&
                   Id == dTO.Id &&
                   FechaFinal == dTO.FechaFinal &&
                   FechaInicio == dTO.FechaInicio &&
                   FechaOferta == dTO.FechaOferta &&
                   TipoDirigida == dTO.TipoDirigida &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<List<HerramientasDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaFinal, FechaInicio, FechaOferta, TipoDirigida, MetodoPago, Items);
        }
    }
}

