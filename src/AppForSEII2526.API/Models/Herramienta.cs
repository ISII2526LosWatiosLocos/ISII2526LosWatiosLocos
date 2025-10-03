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

        //Faltan las siguientes clases:
        //alquilar item
        //itemsreparacion

        public List<CompraItem> CompraItems { get; set; }

        public List<OfertaItem> OfertaItems { get; set; }

        public Fabricante Fabricante { get; set; }

    }
}
