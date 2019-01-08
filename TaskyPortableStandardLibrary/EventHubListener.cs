using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace TaskyPortableStandardLibrary
{
    public class EventHubListener : IEventHubListener
    {
        private EventProcessorHost _eventProcessorHost;

        public event EventHandler<string> MessageReceived;

        public async Task Start()
        {
            _eventProcessorHost = new EventProcessorHost(
                EventHubConstants.EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConstants.EventHubConnectionString,
                EventHubConstants.StorageConnectionString,
                EventHubConstants.StorageContainerName);

            await _eventProcessorHost.RegisterEventProcessorFactoryAsync(new EventProcessorFactory(MessageReceivedHandler));
        }

        private void MessageReceivedHandler(object sender, string e)
        {
            MessageReceived?.Invoke(sender, e);
        }

        public async Task Stop()
        {
            await _eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
