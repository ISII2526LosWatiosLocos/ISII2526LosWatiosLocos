namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraId), nameof(HerramientaId))]
    public class CompraItem
    {

        public int CompraId { get; set; }
        public int HerramientaId { get; set; }

        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int Descripción { get; set; }
        [Required]
        public float Precio { get; set; }

        public Herramienta Herramienta { get; set; }
        public Compra Compra { get; set; }
    }
}
