using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogViewer
{
    public class Subscriber
    {
        private readonly string _exchangeName = "logs_topic";

        public void StartReceiving(string subscriptionTopic)
        {
            var factory = new ConnectionFactory() { HostName = "10.154.14.250" };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Topic,
                durable: true);

            var queueName = channel.QueueDeclare(
                queue: "",
                durable: false,
                exclusive: true,
                autoDelete: true,
                arguments: null).QueueName;

            channel.QueueBind(
                queue: queueName,
                exchange: _exchangeName,
                routingKey: subscriptionTopic);

            Console.WriteLine($"[*] Suscrito a '{_exchangeName}'. Cola: {queueName}. Esperando logs...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[LOG RECIBIDO]: {message}");
                Console.ResetColor();
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Presiona [Enter] para salir.");
            Console.ReadLine();

            // (Opcional) Limpiar al salir
            channel.Close();
            connection.Close();
        }
    }
}
