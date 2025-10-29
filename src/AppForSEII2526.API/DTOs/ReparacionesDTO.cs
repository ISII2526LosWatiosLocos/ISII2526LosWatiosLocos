using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesDTO
    {


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

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesDTO dTO &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   nombre == dTO.nombre &&
                   apellidos == dTO.apellidos &&
                   EqualityComparer<IList<ReparacionesItemDTO>>.Default.Equals(ReparacionesItems, dTO.ReparacionesItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaEntrega, FechaRecogida, PrecioTotal, nombre, apellidos, ReparacionesItems);
        }
    }
}
