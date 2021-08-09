﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessageService.Models;
using MessageService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageService
{
    public class ConsumeRabbitMQService : BackgroundService
    {
        private readonly EmailService _emailService;
        private readonly string ExchangeName;
        private readonly string QueueName;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private ConnectionFactory _connectionFactory;

        public ConsumeRabbitMQService(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;

            QueueName = _configuration["RabbitMQ:QueueName"];
            ExchangeName = _configuration["RabbitMQ:ExchangeName"];
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Первоначальная настройка
            InitializeRabbitMQ();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body;
                var messageJson = Encoding.UTF8.GetString(body.ToArray());
                var rabbitMessage = JsonConvert.DeserializeObject<RabbitMessage>(messageJson);

                Console.WriteLine($"============================================ Received message: {rabbitMessage.Message}");

                await _emailService.SendEmailAsync(rabbitMessage);
            };

            _channel.BasicQos(0, 1, false);
            _channel.BasicConsume(QueueName, true, consumer);

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
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
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:UserPassword"]
            };

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, QueueName);
            _channel.BasicQos(0, 1, false);
        }
    }
}