namespace AppForSEII2526.API.Models
{
    public class Herramienta
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

        public List<CompraItem> CompraItems { get; set; }
        public List<AlquilarItem> AlquilarItems { get; set; }
        public List<OfertaItem> OfertaItems { get; set; }
        public List<ReparaciónItem> ReparaciónItems { get; set; }
        public Fabricante Fabricante { get; set; }


        public Herramienta(int id, string nombre, string material, float precio, int tiempoReparacion, List<CompraItem> compraItems, List<AlquilarItem> alquilarItems, List<OfertaItem> ofertaItems, List<ReparaciónItem> reparaciónItems, Fabricante fabricante)
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

        public Herramienta()
        {
        }

    }
}
