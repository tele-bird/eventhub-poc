using System;
using System.Threading.Tasks;

namespace Tasky.PortableStandardLibrary
{
    public interface IEventHubListener
    {
        event EventHandler<string> MessageReceived;

        Task Start();

        Task Stop();
    }
}
