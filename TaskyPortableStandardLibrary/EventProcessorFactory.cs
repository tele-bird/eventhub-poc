using Microsoft.Azure.EventHubs.Processor;
using System;

namespace TaskyPortableStandardLibrary
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
