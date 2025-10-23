namespace AppForSEII2526.API.DTOs
{
    public class AlquileresParaDetalleDTO
    {
        public String Nombre {  get; set; }
        public String Apellidos { get; set; }
        public String Direccion {  get; set; }
        public DateTime FechaAlquiler { get; set; }
        public float PrecioTotal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal {  get; set; }
        public List<AlquilarItemsDTO> Items { get; set; }

        public AlquileresParaDetalleDTO(string nombre, string apellidos, string direccion, DateTime fechaAlquiler, float precioTotal, DateTime fechaInicio, DateTime fechaFinal, List<AlquilarItemsDTO> items)
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
                   FechaAlquiler == dTO.FechaAlquiler &&
                   PrecioTotal == dTO.PrecioTotal &&
                   FechaInicio == dTO.FechaInicio &&
                   FechaFinal == dTO.FechaFinal &&
                   EqualityComparer<List<AlquilarItemsDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Apellidos, Direccion, FechaAlquiler, PrecioTotal, FechaInicio, FechaFinal, Items);
        }
    }
}
