using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Carely.Services
{
    public class MqttService
    {
        private readonly IMqttClient _client;

        public MqttService(IConfiguration config)
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            string server = config["Mqtt:Server"] ?? "localhost";
            int port = int.TryParse(config["Mqtt:Port"], out var p) ? p : 8883;
            string user = config["Mqtt:Username"] ?? string.Empty;
            string password = config["Mqtt:Password"] ?? string.Empty;

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(server, port)
                .WithCredentials(user, password)
                .WithTls() 
                .Build();

            ConnectAsync(options).GetAwaiter().GetResult();
        }

        private async Task ConnectAsync(MqttClientOptions options)
        {
            try
            {
                await _client.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"MQTT connection failed: {ex.Message}", ex);
            }
        }

        public async Task PublishAsync(string topic, string payload)
        {
            if (!_client.IsConnected)
                throw new InvalidOperationException("MQTT client is not connected.");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await _client.PublishAsync(message);
        }
    }

}
