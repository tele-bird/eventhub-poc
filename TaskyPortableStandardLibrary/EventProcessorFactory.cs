using Microsoft.Azure.EventHubs.Processor;
using System;

namespace Tasky.PortableStandardLibrary
{
    public class EventProcessorFactory : IEventProcessorFactory
    {
        private EventHandler<string> _messageReceivedHandler;

        public EventProcessorFactory(EventHandler<string> messageReceivedHandler)
        {
            _messageReceivedHandler = messageReceivedHandler;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new EventHubEventProcessor(_messageReceivedHandler);
        }
    }
}
