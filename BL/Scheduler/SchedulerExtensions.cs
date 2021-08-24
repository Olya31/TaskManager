using ApiServices.ApiService;
using ApiServices.ApiService.Interfaces;
using ApiServices.EmailService;
using BL.Managers;
using BL.Managers.Interfaces;
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
            services.AddHostedService<SchedulerService>();

            services.AddTransient<ISyncDatabaseManager, SyncDatabaseManager>();
            services.AddTransient<ISenderManager, SenderManager>();
            services.AddTransient<IWeatherProcessor, WeatherProcessor>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<ITaskCollection, TaskCollection>();
            services.AddSingleton<IJobPooling, JobPooling>();
        }
    }
}
