using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBus.PubSub.Subscriber
{
    public sealed class ReviewSubscriptionBuilder
    {
        private readonly ServiceBusAdministrationClient _administrationClient;

        public ReviewSubscriptionBuilder(ServiceBusAdministrationClient administrationClient)
        {
            _administrationClient = administrationClient
                ?? throw new ArgumentNullException(nameof(administrationClient));
        }

        public async Task<SubscriptionProperties> Build(string topicName, string subscriberName)
        {
            if (!await _administrationClient.TopicExistsAsync(topicName))
                await _administrationClient.CreateTopicAsync(new CreateTopicOptions(topicName));

            return await CreateReviewSubscription(topicName, subscriberName);
        }

        private async Task<SubscriptionProperties> CreateReviewSubscription(string topicName, string subscriberName)
        {
            var subscriptionName = $"{subscriberName}-reviews";

            if (!await SubscriptionExists(topicName, subscriptionName))
            {
                return await _administrationClient.CreateSubscriptionAsync(
                    new CreateSubscriptionOptions(topicName, subscriptionName));
            }

            return await _administrationClient.GetSubscriptionAsync(topicName, subscriptionName);
        }

        private async Task<bool> SubscriptionExists(string topicName, string subscriptionName) =>
            (await _administrationClient.SubscriptionExistsAsync(topicName, subscriptionName)).Value;
    }
}
