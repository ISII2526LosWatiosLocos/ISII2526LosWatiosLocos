using System;

namespace LogViewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== VISOR DE LOGS RABBITMQ ===");
                Console.WriteLine("Seleccione el nivel de logs que desea escuchar:");
                Console.WriteLine("1. Información (Info + Error + Warning...)"); // En realidad filtra por nivel 'information'
                Console.WriteLine("2. Solo Errores");
                Console.WriteLine("3. Ver TODO (Cualquier nivel)");
                Console.WriteLine("4. Salir");
                Console.Write("\nOpción: ");

                string? input = Console.ReadLine();
                string subscriptionTopic = "";

                switch (input)
                {
                    case "1":
                        subscriptionTopic = "information.#";
                        break;
                    case "2":

                        subscriptionTopic = "error.#";
                        break;
                    case "3":
                        subscriptionTopic = "#";
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Se usará '#' por defecto (ver todo).");
                        subscriptionTopic = "#";
                        break;
                }

                Console.WriteLine($"\nIniciando suscripción con topic: '{subscriptionTopic}'...");

                // Creo el suscriptor
                var subscriber = new Subscriber();

                // Inicia la recepción (esto bloqueará la consola hasta que presiones Enter dentro del método)
                subscriber.StartReceiving(subscriptionTopic);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error crítico: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}