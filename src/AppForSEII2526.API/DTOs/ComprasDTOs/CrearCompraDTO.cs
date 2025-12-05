namespace AppForSEII2526.API.DTOs.ComprasDTOs
{
    public class CrearCompraDTO
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int MetodoPagoId { get; set; }
        public List<CompraItemsDTO> Items { get; set; }
        public string DireccionEnvio { get; set; }

        //Opcionales:
        public string? CorreoElectronico { get; set; }
        public string? NumeroTelefono { get; set; }

        public CrearCompraDTO()
        {
            Items = new List<CompraItemsDTO>();
        }


    }
}
