using BL.Managers.Interfaces;
using BL.Models;
using BL.Scheduler.Managers.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers
{
    public sealed class SchedulerManager
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private JobModel _job;

        public Task JobTask => _task;

        public SchedulerManager(JobModel job)
        {
            if (job.Cron.Minutes < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(job.Cron.Minutes));
            }

            _job = job;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start(
            ISenderManager senderManager,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken = default)
        {
            if (_task != null)
            {
                throw new InvalidOperationException("Already started.");
            }

            var mergedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);

            _task = Task.Factory.StartNew(
                        () => JobEntryPoint(senderManager, semaphoreSlim, mergedTokenSource.Token),
                        mergedTokenSource.Token,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);

            Thread.MemoryBarrier();
        }

        public Task Stop()
        {
            if (_task == null)
            {
                throw new InvalidOperationException("Not started.");
            }

            _cancellationTokenSource.Cancel();
            return _task;
        }

        private async Task JobEntryPoint(
            ISenderManager senderManager,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await senderManager.SendAsync(_job.Url, _job.Email, cancellationToken);
                    semaphoreSlim.Release();
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception)
                {
                }

                await Task.Delay(_job.Cron.Minutes, cancellationToken);
            }
        }
    }
}


