namespace AppForSEII2526.API.Models
{
    public class TarjetaCredito : MetodosPago
    {
        public override string ToString()
        {
            return Nombre; //devuelve "TarjetaCredito" en lugar del nombre completo del tipo
        }
    }
}
