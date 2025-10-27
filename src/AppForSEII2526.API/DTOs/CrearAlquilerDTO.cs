
namespace AppForSEII2526.API.DTOs
{
    public class CrearAlquilerDTO
    {
        public CrearAlquilerDTO(string nombre, string apellidos, int metodoPagoId, string direccion, string telefono, string? correo, List<CrearAlquilerItemDTO> items)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            MetodoPagoId = metodoPagoId;
            Direccion = direccion;
            this.telefono = telefono;
            this.correo = correo;
            Items = items;
        }

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int MetodoPagoId { get; set; }
        public string Direccion {  get; set; }
        public string telefono { get; set; }
        public string? correo { get; set; }
        public List<CrearAlquilerItemDTO> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CrearAlquilerDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   MetodoPagoId == dTO.MetodoPagoId &&
                   Direccion == dTO.Direccion &&
                   telefono == dTO.telefono &&
                   correo == dTO.correo &&
                   EqualityComparer<List<CrearAlquilerItemDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Apellidos, MetodoPagoId, Direccion, telefono, correo, Items);
        }
    }
}
