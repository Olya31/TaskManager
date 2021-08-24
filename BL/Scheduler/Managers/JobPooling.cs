using BL.Managers.Interfaces;
using BL.Models;
using BL.Scheduler.Managers.Interfaces;
using BL.Scheduler.MemoryDatabase;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers
{
    public sealed class JobPooling : IJobPooling
    {
        private readonly ITaskCollection _taskCollection;
        private readonly ISenderManager _senderManager;

        private readonly ConcurrentDictionary<JobModel, SchedulerManager> _jobs =
            new ConcurrentDictionary<JobModel, SchedulerManager>();

        public JobPooling(
            ITaskCollection taskCollection,
            ISenderManager senderManager)
        {
            _taskCollection = taskCollection;
            _senderManager = senderManager;
        }

        public async Task StartAsync(CancellationToken token)
        {
            await SyncJobsAsyc(token);
            StartJobs(token);
            await StopJobsAsync(token);
        }

        private void StartJobs(CancellationToken token)
        {
            var ss = new SemaphoreSlim(3); // thread count

            foreach (var job in _jobs)
            {
                if (!job.Value.IsRunning)
                {
                    job.Value.Start(_senderManager, ss, token);
                }
            }
        }

        private async Task SyncJobsAsyc(CancellationToken token)
        {
            var jobs = await _taskCollection.GetJobsAsync(token);

            foreach (var job in jobs)
            {
                if (!_jobs.TryGetValue(job, out var _))
                {
                    var scheduler = new SchedulerManager(job);
                    _jobs.TryAdd(job, scheduler);
                }
            }
        }

        private async Task StopJobsAsync(CancellationToken token)
        {
            foreach (var job in _jobs) 
            {
                if (!(await _taskCollection.ContainsJobAsync(job.Key, token)))
                {
                    _ = job.Value.Stop();
                    _jobs.TryRemove(job.Key, out var _);
                }
            }
        }
    }
}
