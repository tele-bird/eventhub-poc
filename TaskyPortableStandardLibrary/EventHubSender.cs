using Microsoft.Azure.EventHubs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace TaskyPortableStandardLibrary
{
    public class EventHubSender : IEventHubSender
    {
        private EventHubClient _eventHubClient;

        public void Start()
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConstants.EventHubConnectionString)
            {
                EntityPath = EventHubConstants.EventHubName
            };

            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public async Task Send(string message)
        {
            try
            {
                await _eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception exc)
            {
            }
        }

        public async Task Stop()
        {
            await _eventHubClient.CloseAsync();
            _eventHubClient = null;
        }
    }
}
