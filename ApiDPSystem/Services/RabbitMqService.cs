using System.Text;
using RabbitMQ.Client;

namespace ApiDPSystem.Services
{
    public class RabbitMqService
    {
        private const string QueueName = "EmailQueue";
        private const string ExchangeName = "EmailExchange";
        private readonly IModel _channel;

        public RabbitMqService(IConnection connection)
        {
            _channel = connection.CreateModel();
            _channel.ConfirmSelect();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, QueueName);
        }

        public void Publish(string payload, string exchange = ExchangeName)
        {
            var body = Encoding.UTF8.GetBytes(payload);
            _channel.BasicPublish(exchange, QueueName, null, body);
        }
    }
}