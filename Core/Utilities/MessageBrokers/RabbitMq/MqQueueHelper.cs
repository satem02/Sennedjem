﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.MessageBrokers.RabbitMq
{
    public class MqQueueHelper : IMessageBrokerHelper
    {
        private readonly IConfiguration _configuration;
        private readonly MessageBrokerOptions _brokerOptions;
        public MqQueueHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _brokerOptions = _configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();
        }

        public void QueueMessage(string messageText)
        {
            var factory = new ConnectionFactory
            {
                HostName = _brokerOptions.HostName,
                UserName = _brokerOptions.UserName,
                Password = _brokerOptions.Password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                        queue: "Queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                var message = "This message came from other side of universe";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "Queue", basicProperties: null, body: body);
            }
        }


    }
}
