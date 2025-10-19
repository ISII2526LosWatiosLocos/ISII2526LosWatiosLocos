namespace AppForSEII2526.API.DTOs
{
    public class HerramientasParaAlquilarDTO
    {
        public string Nombre { get; set; }
        public string Material { get; set; }
        public string Fabricante { get; set; }
        public float Precio { get; set; }
        public HerramientasParaAlquilarDTO(string nombre, string material, string fabricante, float precio)
        {
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is HerramientasParaAlquilarDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Material, Fabricante, Precio);
        }
    }
}
