using BL.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.MemoryDatabase
{
    public interface ITaskCollection
    {
        ValueTask<IEnumerable<JobModel>> GetJobsAsync(CancellationToken cancellationToken);

        ValueTask<JobModel> GetJobAsync(JobModel task, CancellationToken cancellationToken);

        ValueTask<bool> ContainsJobAsync(JobModel task, CancellationToken cancellationToken);

        ValueTask<bool> TryAddAsync(JobModel task, CancellationToken cancellationToken);

        ValueTask<bool> TryRemoveAsync(JobModel task, CancellationToken cancellationToken);
    }
}
