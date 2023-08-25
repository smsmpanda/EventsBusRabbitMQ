// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
    private static void Main1(string[] args)
    {
        string consumerName = args[0];

        Console.WriteLine("Hello, RabbitMQ EventBus...");

        //1.创建客户端与服务端远程连接
        var facetory = new ConnectionFactory { HostName = "192.168.200.22" };
        using var connection = facetory.CreateConnection();

        //2.创建一个通信的频道
        using var channel = connection.CreateModel();

        //3.声明队列，并与当前频道关联
        const string Queue = "event-bus_durable";
        channel.QueueDeclare(queue: Queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

        //4.等待接收消息
        Console.WriteLine($" [*] 消费者{consumerName}等待接收消息.");
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] 消费者{consumerName}接收消息: {message}");
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        channel.BasicConsume(queue: Queue, autoAck: false, consumer: consumer);


        Console.ReadLine();
    }
}