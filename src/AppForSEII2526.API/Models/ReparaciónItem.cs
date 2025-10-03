namespace AppForSEII2526.API.Models
{
    public class ReparaciónItem
    {
        public int Catidad { get; set; }

        public String Descripción { get; set; }
        public float Precio { get; set; }

        //Relaciones
        public Herramienta Herramienta
        {
            get; set;

        }
    }
}
