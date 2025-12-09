namespace LogViewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Validar que se haya pasado un argumento (el topic de suscripción)
                if (args.Length == 0)
                {
                    //Diferentes tipos de logs a los que te puedes suscribir
                    Console.WriteLine("Uso: LogViewer <TOPIC_DE_SUSCRIPCION>");
                    Console.WriteLine("Ejemplo para suscribirse solo a errores: LogViewer error");
                    Console.WriteLine("Ejemplo para suscribirse a cualquier log: LogViewer #");
                    Console.WriteLine("Ejemplo para suscribirse a logs de API de tipo info: LogViewer info.api.*");
                    return;
                }

                string subscriptionTopic = args[0];


                //Creo el suscriptor
                var subscriber = new Subscriber();
                //Se suscribe a una cola
                subscriber.StartReceiving(subscriptionTopic);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}