﻿using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Messaging
{
    internal class RabbitMqMessagePublisher : IMessagePublisher
    {
        public const string EXCHANGE_NAME = "Kwetter";
        private readonly string _queueName;
        private readonly RabbitMqConnection _connection;

        public RabbitMqMessagePublisher(QueueName queueName, RabbitMqConnection connection)
        {
            _queueName = queueName.Name;
            _connection = connection;
        }

        public Task PublishMessageAsync<T>(string messageType, T value)
        {
            using var channel = _connection.CreateChannel();
            channel.ExchangeDeclare(EXCHANGE_NAME, "fanout", durable: true);
            channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(_queueName, EXCHANGE_NAME, routingKey: string.Empty);

            var message = channel.CreateBasicProperties();
            message.ContentType = "application/json";
            message.DeliveryMode = 2;
            // Add a MessageType header, this part is crucial for our solution because it is our way of distinguishing messages
            message.Headers = new Dictionary<string, object> { ["MessageType"] = messageType };
            var body = JsonSerializer.SerializeToUtf8Bytes(value);

            // Publish this without a routing key to the rabbitmq broker
            channel.BasicPublish(EXCHANGE_NAME, _queueName, message, body);
            return Task.CompletedTask;
        }
    }
}
