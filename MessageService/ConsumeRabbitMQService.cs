using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageService
{
    public class ConsumeRabbitMQService : BackgroundService
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "EmailQueue";
        private const string ExchangeName = "EmailExchange";

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Первоначальная настройка
            Console.WriteLine("StartAsync");
            InitializeRabbitMQ();
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("ExecuteAsync");
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"============================================ Received message: {message}");
            };

            _channel.BasicQos(0, 1, false);
            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("StopAsync");
                await base.StopAsync(cancellationToken);
            }
            finally
            {
                _connection.Dispose();
                _channel.Dispose();
            }
        }
        private void InitializeRabbitMQ()
        {
            var rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");

            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitHostName ?? "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();

            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: QueueName);
            _channel.BasicQos(0, 1, false);

        }
    }
}