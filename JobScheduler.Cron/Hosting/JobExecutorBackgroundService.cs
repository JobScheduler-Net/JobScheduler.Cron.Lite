using JobScheduler.Cron.JobExecutor;
using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class JobExecutorBackgroundService : BackgroundService
{
    private readonly IJobExecutor jobExecutor;

    public JobExecutorBackgroundService(IJobExecutor jobExecutor) => this.jobExecutor = jobExecutor;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => jobExecutor.Execute(stoppingToken);
}
