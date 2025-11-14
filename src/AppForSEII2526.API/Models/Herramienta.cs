namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        [Key]
        public int Id { get; set; }

        // Mi enunciado para la modificación propuesta:
        // Cada herramienta tiene una fecha de fabricación, como cliente quiero poder usarla para filtrar en el get, también quiero que se muestre en el post y el details
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date), Display(Name = "Fecha de fabricación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaFabricacion { get; set; }

        [Required, StringLength(100, ErrorMessage = "No puede contener más de 100 caracteres")]
        public string Nombre { get; set; }

        [Required, StringLength(50, ErrorMessage = "No puede contener más de 50 caracteres")]
        public string Material { get; set; }

        [Required]
        public float Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public int TiempoReparacion { get; set; }

        public List<CompraItem> CompraItems { get; set; }
        public List<AlquilarItem> AlquilarItems { get; set; }
        public List<OfertaItem> OfertaItems { get; set; }
        public List<ReparaciónItem> ReparaciónItems { get; set; }
        public Fabricante Fabricante { get; set; }

        // Constructor completo
        public Herramienta(int id, DateOnly fechafabricacion, string nombre, string material, float precio, int stock, int tiemporeparacion, List<CompraItem> compraItems, List<AlquilarItem> alquilarItems, List<OfertaItem> ofertaitems, List<ReparaciónItem> reparacionItems, Fabricante fabricante)
        {
            Id = id;
            FechaFabricacion = fechafabricacion;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            Stock = stock;
            TiempoReparacion = tiemporeparacion;
            CompraItems = compraItems;
            AlquilarItems = alquilarItems;
            OfertaItems = ofertaitems;
            ReparaciónItems = reparacionItems;
            Fabricante = fabricante;
        }

        // Constructor sin el ID para las pruebas
        public Herramienta(DateOnly fechafabricacion, string nombre, string material, float precio, int stock, int tiemporeparacion, List<CompraItem> compraItems, List<AlquilarItem> alquilarItems, List<OfertaItem> ofertaitems, List<ReparaciónItem> reparacionItems, Fabricante fabricante)
        {
            FechaFabricacion = fechafabricacion;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            Stock = stock;
            TiempoReparacion = tiemporeparacion;
            CompraItems = compraItems;
            AlquilarItems = alquilarItems;
            OfertaItems = ofertaitems;
            ReparaciónItems = reparacionItems;
            Fabricante = fabricante;
        }

        // Constructor sin el ID ni las listas para las pruebas del CU de Ofertas
        public Herramienta(DateOnly fechafabricacion, string nombre, string material, float precio, int stock, int tiemporeparacion, Fabricante fabricante)
        {
            FechaFabricacion = fechafabricacion;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            Stock = stock;
            TiempoReparacion = tiemporeparacion;
            Fabricante = fabricante;
        }
        // Constructor vacío
        public Herramienta()
        {
        }

    }
}
