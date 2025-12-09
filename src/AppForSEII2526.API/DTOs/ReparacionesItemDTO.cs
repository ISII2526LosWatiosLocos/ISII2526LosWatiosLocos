namespace AppForSEII2526.API.DTOs
{
    public class ReparacionesItemDTO
    {

        public int HerramientaId { get; set; }
        public string  HerramientaNombre { get; set; }
        public string HerramientaDescripcion { get; set; }    
        public int HerramientaCantidad { get; set; }
        public float HerramientaPrecio { get; set; }

        public ReparacionesItemDTO() { }


        public ReparacionesItemDTO(string herramientaNombre, string herramientaDescripcion, int herramientaCantidad, float herramientaPrecio)
        {
            HerramientaNombre = herramientaNombre;
            HerramientaDescripcion = herramientaDescripcion;
            HerramientaCantidad = herramientaCantidad;
            HerramientaPrecio = herramientaPrecio;
        }

        public ReparacionesItemDTO(int herramientaId, string herramientaDescripcion, int herramientaCantidad)
        {
            HerramientaId = herramientaId; 
            HerramientaDescripcion = herramientaDescripcion;
            HerramientaCantidad = herramientaCantidad;
           
        }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionesItemDTO dTO &&
                   HerramientaNombre == dTO.HerramientaNombre &&
                   HerramientaDescripcion == dTO.HerramientaDescripcion &&
                   HerramientaCantidad == dTO.HerramientaCantidad &&
                   HerramientaPrecio == dTO.HerramientaPrecio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HerramientaNombre, HerramientaDescripcion, HerramientaCantidad, HerramientaPrecio);
        }
    }

}
