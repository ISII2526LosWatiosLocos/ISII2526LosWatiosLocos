
namespace AppForSEII2526.API.DTOs
{
    public class HerramientasParaOfertarDTO 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Material { get; set; }
        public string Fabricante { get; set; }
        public float Precio { get; set; }
        public HerramientasParaOfertarDTO(string nombre, string material, string fabricante, float precio)
        {
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
        }

        public HerramientasParaOfertarDTO(int id, string nombre, string material, string fabricante, float precio)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
        }


        public override bool Equals(object? obj)
        {
            return obj is HerramientasParaOfertarDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio;
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Material, Fabricante, Precio);
        }
    }
}
