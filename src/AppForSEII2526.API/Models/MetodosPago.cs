namespace AppForSEII2526.API.Models
{
    public abstract class MetodosPago
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; protected set; }
    }
}
