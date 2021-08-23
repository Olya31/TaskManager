using BL.Scheduler.Managers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Scheduler.Services
{
    public sealed class SchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TaskSyncService> _logger;

        public SchedulerService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<TaskSyncService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Startign {nameof(SchedulerService)}");

            while (!stoppingToken.IsCancellationRequested)
            {
                await LoopAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation($"Finishing {nameof(SchedulerService)}");
        }

        private async Task LoopAsync(CancellationToken stoppingToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var jobPooling = serviceScope.ServiceProvider.GetRequiredService<IJobPooling>();
            await jobPooling.StartAsync(stoppingToken);
        }
    }
}
