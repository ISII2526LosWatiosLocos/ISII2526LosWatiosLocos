
namespace AppForSEII2526.API.Models
{

    public class Compra
    {
        [Key]
        public int Id { get; set; }

        // Campos obligatorios

        [Required]
        public string DireccionEnvio { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly FechaCompra { get; set; }
        [Required]
        public float PrecioTotal { get; set; }

        // Relaciones

        public List<CompraItem> CompraItems { get; set; }
        public MetodosPago MetodoPago { get; set; }
        public ApplicationUser Usuario { get; set; }

        // Constructor completo
        public Compra(int id, string direccionEnvio, DateOnly fechaCompra, float precioTotal, List<CompraItem> compraItems, MetodosPago metodoPago, ApplicationUser usuario)
        {
            Id = id;
            DireccionEnvio = direccionEnvio;
            FechaCompra = fechaCompra;
            PrecioTotal = precioTotal;
            CompraItems = compraItems;
            MetodoPago = metodoPago;
            Usuario = usuario;
        }

        // Constructor sin el ID para las pruebas
        public Compra(string direccionEnvio, DateOnly fechaCompra, float precioTotal, List<CompraItem> compraItems, MetodosPago metodoPago, ApplicationUser usuario)
        {
            DireccionEnvio = direccionEnvio;
            FechaCompra = fechaCompra;
            PrecioTotal = precioTotal;
            CompraItems = compraItems;
            MetodoPago = metodoPago;
            Usuario = usuario;
        }
        // Constructor vacío
        public Compra() { }
    }
}
