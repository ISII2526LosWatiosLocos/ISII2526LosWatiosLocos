namespace AppForSEII2526.API.DTOs
{
    public class CrearCompraDTO
    {
        public ApplicationUser Usuario { get; set; }
        public int MetodoPagoId { get; set; }
        public List<CrearCompraItemDTO> Items { get; set; }
        public string DireccionEnvio { get; set; }
        public float PrecioTotal { get; set; }

        public CrearCompraDTO()
        {
            Items = new List<CrearCompraItemDTO>();
        }


    }
}
