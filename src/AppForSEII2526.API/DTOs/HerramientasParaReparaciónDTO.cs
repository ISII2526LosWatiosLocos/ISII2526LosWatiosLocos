
namespace AppForSEII2526.API.DTOs
{
    public class HerramientasParaReparaciónDTO
    {

        public string Nombre { get; set; }
        public string Material { get; set; }
        public string Fabricante { get; set; }
        public float Precio { get; set; }
        public int TiempoReparación { get; set; }
        public HerramientasParaReparaciónDTO(string nombre, string material, string fabricante, float precio, int tiempoReparación)
        {
            Nombre = nombre;
            Material = material;
            Fabricante = fabricante;
            Precio = precio;
            TiempoReparación = tiempoReparación;
        }

        public override bool Equals(object? obj)
        {
            return obj is HerramientasParaReparaciónDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio &&
                   TiempoReparación == dTO.TiempoReparación;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Material, Fabricante, Precio, TiempoReparación);
        }
    }
}
