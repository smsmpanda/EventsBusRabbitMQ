﻿using RabbitMQ.Client;
using System.Text;

namespace Event.Producer
{
    public class ExchangeTopic
    {
        static async Task Main4(string[] args)
        {
            string producerName = args[0];
            string routinueKey = args[1];

            //1.创建客户端与服务端远程连接
            var facetory = new ConnectionFactory { HostName = "192.168.200.22" };
            using var connection = facetory.CreateConnection();

            //2.创建一个通信的频道
            using var channel = connection.CreateModel();

            //3.声明队列，并与当前频道关联
            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            //4.开始发送消息
            CancellationTokenSource cts = new CancellationTokenSource(120 * 1000);
            cts.Token.Register(() => { Console.WriteLine("取消发送消息..."); });
            while (!cts.IsCancellationRequested)
            {
                string message = $"我是{producerName}成产的消息：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "topic_logs", routingKey: routinueKey, basicProperties: null, body: body);
                Console.WriteLine($"生产者（{producerName}）成功发送（{routinueKey}）消息...");
                await Task.Delay(2000);
            }
            Console.ReadLine();
        }
    }
}
