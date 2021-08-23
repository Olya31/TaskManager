using BL.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.MemoryDatabase
{
    public sealed class TaskCollection : ITaskCollection
    {
        private readonly ConcurrentDictionary<int, JobModel> _jobsToRun =
            new ConcurrentDictionary<int, JobModel>();

        public ValueTask<IEnumerable<JobModel>> GetJobsAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<IEnumerable<JobModel>>(_jobsToRun.Values.ToArray());
        }

        public ValueTask<JobModel> GetJobAsync(JobModel job, CancellationToken cancellationToken)
        {
            if (_jobsToRun.TryGetValue(job.Id, out JobModel result))
            {
                return new ValueTask<JobModel>(result);
            }

            return new ValueTask<JobModel>(result);
        }

        public ValueTask<bool> ContainsJobAsync(JobModel job, CancellationToken cancellationToken)
        {
            return new ValueTask<bool>(_jobsToRun.ContainsKey(job.Id));
        }

        public ValueTask<bool> TryAddAsync(JobModel job, CancellationToken cancellationToken)
        {
            return new ValueTask<bool>(_jobsToRun.TryAdd(job.Id, job));
        }

        public ValueTask<bool> TryRemoveAsync(JobModel job, CancellationToken cancellationToken)
        {
            return new ValueTask<bool>(_jobsToRun.TryRemove(job.Id, out _));
        }
    }
}
