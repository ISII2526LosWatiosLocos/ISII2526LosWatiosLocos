
namespace AppForSEII2526.API.DTOs
{
    public class HerramientasDTO
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        public string Nombre { get; set; }

        [Required, StringLength(50, ErrorMessage = "No puede contener más de 50 caracteres")]
        public string Material { get; set; }

        [Required]
        public float Precio { get; set; }

        public int TiempoReparacion { get; set; }

        public string NombreFabricante { get; set; }


        public List<CompraItem> CompraItems { get; set; }
        public List<AlquilarItem> AlquilarItems { get; set; }
        public List<OfertaItem> OfertaItems { get; set; }
        public List<ReparaciónItem> ReparaciónItems { get; set; }
        public Fabricante Fabricante { get; set; }

        public HerramientasDTO(int id, string nombre, string material, float precio, int tiempoReparacion, List<CompraItem> compraItems, List<AlquilarItem> alquilarItems, List<OfertaItem> ofertaItems, List<ReparaciónItem> reparaciónItems, Fabricante fabricante)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            TiempoReparacion = tiempoReparacion;
            CompraItems = compraItems;
            AlquilarItems = alquilarItems;
            OfertaItems = ofertaItems;
            ReparaciónItems = reparaciónItems;
            Fabricante = fabricante;
        }

        public HerramientasDTO(string nombre, string material, string nombrefabricante, float precio)
        {
            Nombre = nombre;
            Material = material;
            Precio = precio;
            NombreFabricante = nombrefabricante;
        }

        public override bool Equals(object? obj)
        {
            return obj is HerramientasDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Precio == dTO.Precio &&
                   TiempoReparacion == dTO.TiempoReparacion &&
                   EqualityComparer<List<CompraItem>>.Default.Equals(CompraItems, dTO.CompraItems) &&
                   EqualityComparer<List<AlquilarItem>>.Default.Equals(AlquilarItems, dTO.AlquilarItems) &&
                   EqualityComparer<List<OfertaItem>>.Default.Equals(OfertaItems, dTO.OfertaItems) &&
                   EqualityComparer<List<ReparaciónItem>>.Default.Equals(ReparaciónItems, dTO.ReparaciónItems) &&
                   EqualityComparer<Fabricante>.Default.Equals(Fabricante, dTO.Fabricante);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nombre);
            hash.Add(Material);
            hash.Add(Precio);
            hash.Add(TiempoReparacion);
            hash.Add(CompraItems);
            hash.Add(AlquilarItems);
            hash.Add(OfertaItems);
            hash.Add(ReparaciónItems);
            hash.Add(Fabricante);
            return hash.ToHashCode();
        }
    }
}
