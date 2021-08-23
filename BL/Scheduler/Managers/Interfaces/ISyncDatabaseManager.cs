using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers.Interfaces
{
    public interface ISyncDatabaseManager
    {
        Task SyncSchedulerTasksAsync(CancellationToken cancellationToken);
    }
}
