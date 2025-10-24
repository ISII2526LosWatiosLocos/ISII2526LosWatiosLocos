namespace AppForSEII2526.API.DTOs
{
    public class CrearReparacionDTO
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaFinal { get; set; }
        public float Precio { get; set; }
        
        public List<CrearReparacionItemDTO> ReparacionesItems { get; set; }

        public CrearReparacionDTO()
        {
            ReparacionesItems = new List<CrearReparacionItemDTO>();
        }
    }
}
