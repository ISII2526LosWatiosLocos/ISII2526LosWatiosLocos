namespace AppForSEII2526.API.DTOs
{
    public class OfertasParaDetalleDTO
    {
        public DateOnly FechaFinal { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaOferta { get; set; }
        public string TipoDirigida { get; set; }
        public string MetodoPago { get; set; }

        public string nombreUsuario { get; set; }

        //Lista de items
        public IList<OfertaItemsDTO> Items { get; set; }

        public OfertasParaDetalleDTO(DateOnly fechaFinal, DateOnly fechaInicio, DateOnly fechaOferta, string tipoDirigida, string metodoPago, IList<OfertaItemsDTO> items, string nombreUsuario)

        {
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            FechaOferta = fechaOferta;
            TipoDirigida = tipoDirigida;
            MetodoPago = metodoPago;
            Items = items;
            this.nombreUsuario = nombreUsuario;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not OfertasParaDetalleDTO dto)
                return false;

            // Comparación de las propiedades simples
            bool basicasIguales =
                FechaFinal == dto.FechaFinal &&
                FechaInicio == dto.FechaInicio &&
                FechaOferta == dto.FechaOferta &&
                TipoDirigida == dto.TipoDirigida &&
                nombreUsuario == dto.nombreUsuario &&
                MetodoPago == dto.MetodoPago;

            // Si alguna de las listas es null
            if (Items == null && dto.Items == null)
                return basicasIguales;

            if (Items == null || dto.Items == null)
                return false;

            // Comparar el contenido (no solo la referencia)
            bool listasIguales = Items.SequenceEqual(dto.Items);

            return basicasIguales && listasIguales;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaFinal, FechaInicio, FechaOferta, TipoDirigida, MetodoPago, Items, nombreUsuario);
        }
    }
}

