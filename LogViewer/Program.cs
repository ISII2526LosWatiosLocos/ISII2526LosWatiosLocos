namespace LogViewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. Crea una instancia de tu suscriptor
            var subscriber = new Subscriber();

            // 2. Llama al método que inicia la escucha
            subscriber.StartReceiving();
        }
    }
}