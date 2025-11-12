namespace AppForSEII2526.API.DTOs
{
    public class HerramientasParaComprarDTO
    {
        public DateOnly FechaFabricacion { get; set; }
        public string Nombre { get; set; }
        public string Material { get; set; }
        public string Fabricante { get; set; }
        public float Precio { get; set; }
        public HerramientasParaComprarDTO(DateOnly fechaFabricacion, string nombre, string material, string fabricante, float precio)
        {
            FechaFabricacion = fechaFabricacion;
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is HerramientasParaComprarDTO dTO &&
                   FechaFabricacion.Equals(dTO.FechaFabricacion) &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaFabricacion, Nombre, Material, Fabricante, Precio);
        }
    }
}
