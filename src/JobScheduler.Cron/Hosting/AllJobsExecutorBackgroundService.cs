using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class AllJobsExecutorBackgroundService(IJob job) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => job.Execute(stoppingToken);
}
