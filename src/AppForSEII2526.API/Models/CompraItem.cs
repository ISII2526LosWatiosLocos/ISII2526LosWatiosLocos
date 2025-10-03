namespace AppForSEII2526.API.Models
{
    public class CompraItem
    {
        [Key]
        public int IdCompra { get; set; }

        [Required]
        public int IdHerramienta { get; set; }

        public int Cantidad { get; set; }

        public int Descripción { get; set; }

        public float Precio { get; set; }

        public Herramienta Herramienta { get; set; }

        public Compra Compra { get; set; }
    }
}
