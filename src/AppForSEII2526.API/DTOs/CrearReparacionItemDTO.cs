
namespace AppForSEII2526.API.DTOs
{
    public class CrearReparacionItemDTO
    {
        public int HerramientaId { get; set; }
        public string ? HerramientaDescripcion { get; set; }

      
        public int HerramientaCantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CrearReparacionItemDTO dTO &&
                   HerramientaId == dTO.HerramientaId &&
                   HerramientaDescripcion == dTO.HerramientaDescripcion &&
                   HerramientaCantidad == dTO.HerramientaCantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HerramientaId, HerramientaDescripcion, HerramientaCantidad);
        }
    }

}
