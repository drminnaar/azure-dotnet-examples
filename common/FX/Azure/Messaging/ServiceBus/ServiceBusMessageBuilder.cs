using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Azure.Messaging.ServiceBus
{
    public sealed class ServiceBusMessageBuilder : IServiceBusMessageBuilder
    {
        public ServiceBusMessageBuilder()
        {
        }

        private readonly IDictionary<string, object> _applicationProperties = new Dictionary<string, object>();
        private string Message { get; set; } = string.Empty;
        private string Subject { get; set; } = string.Empty;

        public IServiceBusMessageBuilder AddApplicationProperty(string propertyName, object value)
        {
            _applicationProperties.Add(propertyName.ToLowerInvariant(), value);
            return this;
        }

        public IServiceBusMessageBuilder New()
        {
            Message = string.Empty;
            Subject = string.Empty;
            _applicationProperties.Clear();
            return this;
        }

        public IServiceBusMessageBuilder SetMessage<T>(T value) where T : class, new()
        {
            Message = JsonSerializer.Serialize(
                value,
                value.GetType(),
                new JsonSerializerOptions { WriteIndented = true });

            return this;
        }

        public IServiceBusMessageBuilder SetSubject(string subject)
        {
            Subject = subject.ToLowerInvariant();
            return this;
        }

        public ServiceBusMessage Build()
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(Message))
            {
                Subject = Subject
            };

            foreach (var appProperty in _applicationProperties)
                message.ApplicationProperties.Add(appProperty.Key, appProperty.Value);

            return message;
        }
    }
}
