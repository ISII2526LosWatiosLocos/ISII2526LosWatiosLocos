namespace AppForSEII2526.API.DTOs.AlquileresDTOs
{
    public class AlquilarItemsDTO
    {
        public string NombreItem { get; set; }
        public string MaterialItem { get; set; }
        public float PrecioItem { get; set; }
        public int CantidadItem { get; set; }
        public AlquilarItemsDTO(string nombreItem, string materialItem, float precioItem, int cantidadItem)
        {
            NombreItem = nombreItem;
            MaterialItem = materialItem;
            PrecioItem = precioItem;
            CantidadItem = cantidadItem;
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
