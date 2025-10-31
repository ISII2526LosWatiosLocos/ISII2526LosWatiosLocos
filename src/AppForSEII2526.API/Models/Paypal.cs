namespace AppForSEII2526.API.Models
{
    public class Paypal : MetodosPago
    {
        public override string ToString()
        {
            return Nombre; //devuelve "Paypal" en lugar del nombre completo del tipo
        }
    }
}
