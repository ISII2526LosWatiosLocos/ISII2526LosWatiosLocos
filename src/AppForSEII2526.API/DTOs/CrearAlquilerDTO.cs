
namespace AppForSEII2526.API.DTOs
{
    public class CrearAlquilerDTO
    {
        public CrearAlquilerDTO(float precioTotal, DateTime fechaAlquiler, DateTime fechaInicio, DateTime fechaFinal, string nombre, string apellidos, int metodoPagoId, string direccion, string telefono, string? correo, List<CrearAlquilerItemDTO> items)
        {
            PrecioTotal = precioTotal;
            FechaAlquiler = fechaAlquiler;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            Nombre = nombre;
            Apellidos = apellidos;
            MetodoPagoId = metodoPagoId;
            Direccion = direccion;
            this.telefono = telefono;
            this.correo = correo;
            Items = items;
        }

        public float PrecioTotal { get; set; }
        public DateTime FechaAlquiler {  get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal {  get; set; }
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
                   FechaInicio == dTO.FechaInicio &&
                   FechaFinal == dTO.FechaFinal &&
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
            HashCode hash = new HashCode();
            hash.Add(FechaInicio);
            hash.Add(FechaFinal);
            hash.Add(Nombre);
            hash.Add(Apellidos);
            hash.Add(MetodoPagoId);
            hash.Add(Direccion);
            hash.Add(telefono);
            hash.Add(correo);
            hash.Add(Items);
            return hash.ToHashCode();
        }
    }
}
