using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.AlquileresDTOs
{
    public class AlquilarItemsDTO
    {
        public int IdItem { get; set; }

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
        public AlquilarItemsDTO(float herramientaPrecio, int herramientaId, int herramientaCantidad)
        {
            PrecioItem = herramientaPrecio;
            IdItem = herramientaId;
            CantidadItem = herramientaCantidad;
        }
        public AlquilarItemsDTO(int herramientaId, int herramientaCantidad)
        {
            IdItem = herramientaId;
            CantidadItem = herramientaCantidad;
        }
        public AlquilarItemsDTO() { }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarItemsDTO dTO &&
                   IdItem == dTO.IdItem &&
                   NombreItem == dTO.NombreItem &&
                   MaterialItem == dTO.MaterialItem &&
                   PrecioItem == dTO.PrecioItem &&
                   CantidadItem == dTO.CantidadItem;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdItem, NombreItem, MaterialItem, PrecioItem, CantidadItem);
        }
    }
}
