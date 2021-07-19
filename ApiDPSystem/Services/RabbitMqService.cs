using RabbitMQ.Client;
using System.Text;

namespace ApiDPSystem.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "EmailQueue";
        private const string ExchangeName = "EmailExchange";

        public RabbitMqService(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();

            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: QueueName);
        }

        public void Publish(string payload, string exchange = ExchangeName)
        {
            var body = Encoding.UTF8.GetBytes(payload);
            _channel.BasicPublish(exchange: exchange, routingKey: QueueName, basicProperties: null, body: body);
        }
    }
}