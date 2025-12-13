namespace AppForSEII2526.API.DTOs.AlquileresDTOs
{
    public class CrearAlquilerDTO 
    {
        public CrearAlquilerDTO(string nombre, string apellidos, DateOnly fechaInicio, DateOnly fechaFinal, int metodoPagoId, string direccion, string telefono, string? correo, List<AlquilarItemsDTO> items)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            MetodoPagoId = metodoPagoId;
            Direccion = direccion;
            this.telefono = telefono;
            this.correo = correo;
            Items = items;
        }

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinal { get; set; }
        public int MetodoPagoId { get; set; }
        public string Direccion {  get; set; }
        public string? telefono { get; set; }
        public string? correo { get; set; }
        public List<AlquilarItemsDTO> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CrearAlquilerDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   FechaInicio.Equals(dTO.FechaInicio) &&
                   FechaFinal.Equals(dTO.FechaFinal) &&
                   MetodoPagoId == dTO.MetodoPagoId &&
                   Direccion == dTO.Direccion &&
                   telefono == dTO.telefono &&
                   correo == dTO.correo &&
                   Items.SequenceEqual(dTO.Items);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Nombre);
            hash.Add(Apellidos);
            hash.Add(FechaInicio);
            hash.Add(FechaFinal);
            hash.Add(MetodoPagoId);
            hash.Add(Direccion);
            hash.Add(telefono);
            hash.Add(correo);
            hash.Add(Items);
            return hash.ToHashCode();
        }
    }
}
