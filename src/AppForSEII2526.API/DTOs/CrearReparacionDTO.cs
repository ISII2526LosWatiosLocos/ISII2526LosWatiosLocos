namespace AppForSEII2526.API.DTOs
{
    public class CrearReparacionDTO
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateOnly FechaEntrega { get; set; }
        public DateOnly FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }
        public int MetodoPagoId { get; set; }
        public String telefono { get; set; }
        


        public List<ReparacionesItemDTO> ReparacionesItems { get; set; }

        public CrearReparacionDTO()
        {
            ReparacionesItems = new List<ReparacionesItemDTO>();
        }

        public override bool Equals(object? obj)
        {
            return obj is CrearReparacionDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   FechaEntrega.Equals(dTO.FechaEntrega) &&
                   FechaRecogida.Equals(dTO.FechaRecogida) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   MetodoPagoId == dTO.MetodoPagoId &&
                   telefono == dTO.telefono &&
                   EqualityComparer<List<ReparacionesItemDTO>>.Default.Equals(ReparacionesItems, dTO.ReparacionesItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Apellidos, FechaEntrega, FechaRecogida, PrecioTotal, MetodoPagoId, telefono, ReparacionesItems);
        }
    }
}
