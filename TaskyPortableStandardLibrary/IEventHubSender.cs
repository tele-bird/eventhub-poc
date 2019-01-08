using System.Threading.Tasks;

namespace TaskyPortableStandardLibrary
{
    public interface IEventHubSender
    {
        void Start();

        Task Stop();

        Task Send(string message);
    }
}
