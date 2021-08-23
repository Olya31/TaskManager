using BL.Managers.Interfaces;
using BL.Models;
using BL.Scheduler.Managers.Interfaces;
using BL.Scheduler.MemoryDatabase;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            await RunJobsAsync(token);
        }

        private async Task RunJobsAsync(CancellationToken token)
        {
            var ss = new SemaphoreSlim(3); // thread count

            foreach (var job in _jobs)
            {
                if (!job.Value.IsRunning)
                {
                    await ss.WaitAsync(token);
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

        public void Stop(CancellationToken token)
        {
            var tasks = new List<Task>();

            foreach (var job in _jobs)
            {
                tasks.Add(job.Value.Stop());
            }

            Task.WaitAll(tasks.ToArray(), token);
        }
    }
}
