using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers.Interfaces
{
    public interface IJobPooling
    {
        Task StartAsync(CancellationToken token);
    }
}
