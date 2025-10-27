namespace AppForSEII2526.API.Models
{
    public class Fabricante
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, ErrorMessage = "Numero de caracteres excedido de 100")]
        public string Nombre { get; set; }

        public List<Herramienta> Herramientas { get; set; }


        // Constructor completo
        public Fabricante(int id, string nombre, List<Herramienta> herramientas)
        {
            Id = id;
            Nombre = nombre;
            Herramientas = herramientas;
        }

        // Constructor sin el ID para las pruebas
        public Fabricante(string nombre, List<Herramienta> herramientas)
        {
            Nombre = nombre;
            Herramientas = herramientas;
        }

        // Constructor vacío
        public Fabricante() { }
    }
}
