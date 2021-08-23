using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers.Interfaces
{
    public interface IJobPooling
    {
        void Stop(CancellationToken token);

        Task StartAsync(CancellationToken token);
    }
}
