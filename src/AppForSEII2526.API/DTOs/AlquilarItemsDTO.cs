namespace AppForSEII2526.API.DTOs
{
    public class AlquilarItemsDTO
    {
        public String NombreItem { get; set; }
        public String MaterialItem { get; set; }
        public float PrecioItem { get; set; }
        public int CantidadItem { get; set; }
        public AlquilarItemsDTO(String nombreItem, String materialItem, float precioItem, int cantidadItem)
        {
            this.NombreItem = nombreItem;
            this.MaterialItem = materialItem;
            this.PrecioItem = precioItem;
            this.CantidadItem = cantidadItem;
        }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarItemsDTO dTO &&
                   NombreItem == dTO.NombreItem &&
                   MaterialItem == dTO.MaterialItem &&
                   PrecioItem == dTO.PrecioItem &&
                   CantidadItem == dTO.CantidadItem;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreItem, MaterialItem, PrecioItem, CantidadItem);
        }
    }
}
