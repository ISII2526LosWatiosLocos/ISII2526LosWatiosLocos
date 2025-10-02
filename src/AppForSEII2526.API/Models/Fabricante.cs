namespace AppForSEII2526.API.Models
{
    public class Fabricante
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

    }
}
