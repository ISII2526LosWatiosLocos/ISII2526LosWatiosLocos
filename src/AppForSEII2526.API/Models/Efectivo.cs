namespace AppForSEII2526.API.Models
{
    public class Efectivo : MetodosPago
    {

        public override string ToString()
        {
            return Nombre; //devuelve "Efectivo" en lugar del nombre completo del tipo
        }

    }
}
