using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class JobExecutorBackgroundService : BackgroundService
{
    private readonly IJob job;

    public JobExecutorBackgroundService(IJob job) => this.job = job;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => job.Execute(stoppingToken);
}
