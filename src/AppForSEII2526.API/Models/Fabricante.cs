namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(FabricanteId))]
    public class Fabricante
    {

        public int FabricanteId { get; set; }

        [Required, StringLength(100, ErrorMessage = "Numero de caracteres excedido de 100")]
        public string Nombre { get; set; }

        public List<Herramienta> Herramientas { get; set; }

    }
}
