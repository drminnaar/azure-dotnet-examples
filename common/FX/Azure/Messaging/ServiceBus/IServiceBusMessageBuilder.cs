namespace Azure.Messaging.ServiceBus
{
    public interface IServiceBusMessageBuilder
    {
        IServiceBusMessageBuilder AddApplicationProperty(string propertyName, object value);
        ServiceBusMessage Build();
        IServiceBusMessageBuilder New();
        IServiceBusMessageBuilder SetMessage<T>(T value) where T : class, new();
        IServiceBusMessageBuilder SetSubject(string subject);
    }
}