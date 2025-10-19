using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesDTO
    {


        public int Id { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }
        public List<HerramientasDTO> Items { get; set; }
        public MetodosPago MétodoPago { get; set; }
        public List<ReparaciónItem> ReparaciónItems { get; set; }
        public ApplicationUser Usuario { get; set; }


        private string nombre;
        private string apellidos;


    
        public ReparacionesDTO(string nombre, string apellidos, DateTime FechaEntrega, DateTime FechaRecogida, float PrecioTotal, List<HerramientasDTO> Items)
        {
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.FechaEntrega = FechaEntrega;
            this.FechaRecogida = FechaRecogida;
            this.PrecioTotal = PrecioTotal;
            
            this.Items = Items;
        }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesDTO dTO &&
                   Id == dTO.Id &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   EqualityComparer<MetodosPago>.Default.Equals(MétodoPago, dTO.MétodoPago) &&
                   EqualityComparer<List<HerramientasDTO>>.Default.Equals(Items, dTO.Items) &&
                   EqualityComparer<List<ReparaciónItem>>.Default.Equals(ReparaciónItems, dTO.ReparaciónItems) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Usuario, dTO.Usuario);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaEntrega, FechaRecogida, PrecioTotal, MétodoPago, Items, ReparaciónItems, Usuario);
        }
    }
}
