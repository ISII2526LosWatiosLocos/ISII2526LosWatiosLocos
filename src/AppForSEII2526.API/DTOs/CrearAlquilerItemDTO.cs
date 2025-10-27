
namespace AppForSEII2526.API.DTOs
{
    public class CrearAlquilerItemDTO
    {
        public CrearAlquilerItemDTO(int herramientaId, int herramientaCantidad)
        {
            HerramientaId = herramientaId;
            HerramientaCantidad = herramientaCantidad;
        }

        public int HerramientaId { get; set; }
        public int HerramientaCantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CrearAlquilerItemDTO dTO &&
                   HerramientaId == dTO.HerramientaId &&
                   HerramientaCantidad == dTO.HerramientaCantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HerramientaId, HerramientaCantidad);
        }
    }

}
