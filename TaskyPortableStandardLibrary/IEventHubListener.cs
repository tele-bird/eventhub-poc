using System;
using System.Threading.Tasks;

namespace TaskyPortableStandardLibrary
{
    public interface IEventHubListener
    {
        event EventHandler<string> MessageReceived;

        Task Start();

        Task Stop();
    }
}
