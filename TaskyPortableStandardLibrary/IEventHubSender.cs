using System.Threading.Tasks;

namespace Tasky.PortableStandardLibrary
{
    public interface IEventHubSender
    {
        void Start();

        Task Stop();

        Task Send(string message);
    }
}
