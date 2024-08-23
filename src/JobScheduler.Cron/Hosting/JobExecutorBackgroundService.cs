using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class AllJobsExecutorBackgroundService : BackgroundService
{
    private readonly IJob job;

    public AllJobsExecutorBackgroundService(IJob job) => this.job = job;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => job.Execute(stoppingToken);
}
