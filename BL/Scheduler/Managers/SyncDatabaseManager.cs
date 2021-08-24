using BL.Managers.Interfaces;
using BL.Models;
using BL.Scheduler.Managers.Interfaces;
using BL.Scheduler.MemoryDatabase;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Managers
{
    public sealed class SyncDatabaseManager : ISyncDatabaseManager
    {
        private readonly ITaskManager _taskManager;
        private readonly ITaskCollection _taskCollection;

        public SyncDatabaseManager(
            ITaskCollection taskCollection,
            ITaskManager taskManager)
        {
            _taskCollection = taskCollection;
            _taskManager = taskManager;
        }

        public async Task SyncSchedulerTasksAsync(CancellationToken cancellationToken)
        {
            var tasks = await _taskManager.GetAllTaskAsync(cancellationToken);

            await AddTasksAsync(tasks, cancellationToken);
            await RemoveTasksAsync(tasks, cancellationToken);
        }

        private async Task RemoveTasksAsync(
            IEnumerable<TaskModel> tasks,
            CancellationToken cancellationToken)
        {
            var jobs = await _taskCollection.GetJobsAsync(cancellationToken);

            foreach (var job in jobs)
            {
                if (!tasks.Any(c => job.Id == c.Id))
                {
                    await _taskCollection.TryRemoveAsync(job, cancellationToken);
                }
            }
        }

        private async Task AddTasksAsync(
            IEnumerable<TaskModel> tasks,
            CancellationToken cancellationToken)
        {
            foreach (var task in tasks)
            {
                var job = new JobModel(task);
                if (!(await _taskCollection.ContainsJobAsync(job, cancellationToken)))
                {
                    await _taskCollection.TryAddAsync(job, cancellationToken);
                }

                var scheduleTask = await _taskCollection.GetJobAsync(job, cancellationToken);

                if (scheduleTask != null && !tasks.Equals(scheduleTask))
                {
                    await _taskCollection.TryRemoveAsync(scheduleTask, cancellationToken);
                    await _taskCollection.TryAddAsync(scheduleTask, cancellationToken);
                }
            }
        }
    }
}
