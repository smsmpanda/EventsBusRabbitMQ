using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Event.Consumer
{
    public class ExchangeFanout
    {
        static void Main2(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "192.168.200.22" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //声明交换机
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "logs", routingKey: string.Empty);

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
