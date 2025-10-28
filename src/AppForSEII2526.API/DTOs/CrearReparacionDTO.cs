namespace AppForSEII2526.API.DTOs
{
    public class CrearReparacionDTO
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }
        public int MetodoPagoId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public List<CrearReparacionItemDTO> ReparacionesItems { get; set; }

        public CrearReparacionDTO()
        {
            ReparacionesItems = new List<CrearReparacionItemDTO>();
        }
    }
}
