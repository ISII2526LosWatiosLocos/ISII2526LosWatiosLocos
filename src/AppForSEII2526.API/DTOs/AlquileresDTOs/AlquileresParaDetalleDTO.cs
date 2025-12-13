namespace AppForSEII2526.API.DTOs.AlquileresDTOs
{
    public class AlquileresParaDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre {  get; set; }
        public string Apellidos { get; set; }
        public string Direccion {  get; set; }
        public DateOnly FechaAlquiler { get; set; }
        public float PrecioTotal { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinal {  get; set; }
        public List<AlquilarItemsDTO> Items { get; set; }

        public AlquileresParaDetalleDTO(string nombre, string apellidos, string direccion, DateOnly fechaAlquiler, float precioTotal, DateOnly fechaInicio, DateOnly fechaFinal, List<AlquilarItemsDTO> items, int id)
        {
            Id = id;
            Nombre = nombre;
            Apellidos = apellidos;
            Direccion = direccion;
            FechaAlquiler = fechaAlquiler;
            PrecioTotal = precioTotal;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            Items = items;
        }

        public AlquileresParaDetalleDTO(string nombre, string apellidos, string direccion, DateOnly fechaAlquiler, float precioTotal, DateOnly fechaInicio, DateOnly fechaFinal, List<AlquilarItemsDTO> items)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            Direccion = direccion;
            FechaAlquiler = fechaAlquiler;
            PrecioTotal = precioTotal;
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            Items = items;
        }


        public override bool Equals(object? obj)
        {
            return obj is AlquileresParaDetalleDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Apellidos == dTO.Apellidos &&
                   Direccion == dTO.Direccion &&
                   FechaAlquiler.Equals(dTO.FechaAlquiler) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInicio.Equals(dTO.FechaInicio) &&
                   FechaFinal.Equals(dTO.FechaFinal) &&
                   Items.SequenceEqual(dTO.Items);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Nombre);
            hash.Add(Apellidos);
            hash.Add(Direccion);
            hash.Add(FechaAlquiler);
            hash.Add(PrecioTotal);
            hash.Add(FechaInicio);
            hash.Add(FechaFinal);
            hash.Add(Items);
            return hash.ToHashCode();
        }
    }
}
