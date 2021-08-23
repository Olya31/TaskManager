using BL.Scheduler.Managers;
using BL.Scheduler.Managers.Interfaces;
using BL.Scheduler.MemoryDatabase;
using BL.Scheduler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Scheduler
{
    public static class SchedulerExtensions
    {
        public static void AddSchedulerDependencies(this IServiceCollection services)
        {
            services.AddHostedService<TaskSyncService>();

            services.AddScoped<ISyncDatabaseManager, SyncDatabaseManager>();

            services.AddSingleton<ITaskCollection, TaskCollection>();
            //services.AddSingleton<IJobPooling, JobPooling>();
        }
    }
}
