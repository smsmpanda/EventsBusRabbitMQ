using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Event.Producer
{
    public class ManuallylPublish
    {
        const string BROKER_NAME = "eshop_event_bus";
        const string ROUTINUE_KEY = "PriceUpdateIntergrationEvent";
        static void Main(string[] args) 
        {
            IModel? channel;
            try
            {
                IConnection connection = Connect();
                channel = CreateModel(connection);
                Console.WriteLine("[*] connection rabbitmq host successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine("[*] connection rabbitmq host failure and reconnect.");
                return;
            }

            while (true)
            {
                Console.WriteLine("[*] please input a message \n [message]:");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) 
                {
                    continue;
                }

                 PublishMessage(channel, ROUTINUE_KEY, line);
            }
        }


        private static IConnection Connect() 
        {
            var facetory = new ConnectionFactory { HostName = "192.168.200.22" };
            return facetory.CreateConnection();
        }

        private static IModel? CreateModel(IConnection connection) 
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);

            return channel;
        }

        private static void PublishMessage(IModel channel,string routinueKey,string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: BROKER_NAME, routingKey: routinueKey, basicProperties: null, body: body);
        }
    }
}
