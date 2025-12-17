using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// para ejecutarlo en la carpeta LogViewer hay que hacer:
// docker build -t nombresubscriber .
// docker run -it nombresubscriber

namespace LogViewer
{
    public class Subscriber
    {
        private readonly string _exchangeName = "logs_topic";

        public void StartReceiving(string subscriptionTopic)
        {
            //Crear la conexión
            var factory = new ConnectionFactory() { HostName = "10.69.79.250" }; // CAMBIAR LA IP ANTES DE BUILDEAR
            
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //Creas el exchangue
            channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Topic,
                durable: true);
            //Crear la cola efímera.Con durable=true, será persistente
            var queueName = channel.QueueDeclare(
                queue: "",
                durable: false,
                exclusive: true,
                autoDelete: true,
                arguments: null).QueueName;
            //Bindear la cola al exchangue
            channel.QueueBind(
                queue: queueName,
                exchange: _exchangeName,
                routingKey: subscriptionTopic);

            Console.WriteLine($"[*] Suscrito a '{_exchangeName}'. Cola: {queueName}. Esperando logs...");
            //Crear el consumidor
            var consumer = new EventingBasicConsumer(channel);
            //Configurar callback al recibir un mensaje
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[LOG RECIBIDO]: {message}");
                Console.ResetColor();
            };
            //Iniciar consumo de mensajes
            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Presiona [Enter] para salir.");
            Console.ReadLine();

            //Limpiar al salir
            channel.Close();
            connection.Close();
        }
    }
}
