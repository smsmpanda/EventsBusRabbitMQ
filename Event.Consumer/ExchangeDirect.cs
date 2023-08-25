using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Event.Consumer
{
    public class ExchangeDirect
    {
        static void Main(string[] args)
        {

            string routinueKey = "PriceUpdateIntergrationEvent";//args[0];

            var factory = new ConnectionFactory { HostName = "192.168.200.22" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //声明交换机
            channel.ExchangeDeclare(exchange: "eshop_event_bus", type: ExchangeType.Direct);

            var queueName = "Worker";//channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "eshop_event_bus", routingKey: routinueKey);

            Console.WriteLine("[*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {message}");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
