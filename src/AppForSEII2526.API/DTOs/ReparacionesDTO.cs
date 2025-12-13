using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesDTO
    {
        [JsonPropertyName("Id")]

        public int Id { get; set; }

        public DateOnly FechaEntrega { get; set; }
        public DateOnly FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }

      



        public string nombre { get; set; }
        public string apellidos { get; set; }


        public IList<ReparacionesItemDTO> ReparacionesItems { get; set; }
        public ReparacionesDTO(string nombre, string apellidos, DateOnly FechaEntrega, DateOnly FechaRecogida, float PrecioTotal, IList<ReparacionesItemDTO> ReparacionesItems)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.FechaEntrega = FechaEntrega;
            this.FechaRecogida = FechaRecogida;
            this.PrecioTotal = PrecioTotal;
            this.ReparacionesItems = ReparacionesItems;

        }

        public ReparacionesDTO(int id, DateOnly fechaEntrega, DateOnly fechaRecogida, float precioTotal, string nombre, string apellidos, IList<ReparacionesItemDTO> reparacionesItems)
        {
            Id = id;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            this.nombre = nombre;
            this.apellidos = apellidos;
            ReparacionesItems = reparacionesItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesDTO dTO &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   nombre == dTO.nombre &&
                   apellidos == dTO.apellidos &&
                    (ReparacionesItems?.SequenceEqual(dTO.ReparacionesItems ?? new List<ReparacionesItemDTO>()) ??
            dTO.ReparacionesItems == null);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaEntrega, FechaRecogida, PrecioTotal, nombre, apellidos, ReparacionesItems);
        }
    }
}
