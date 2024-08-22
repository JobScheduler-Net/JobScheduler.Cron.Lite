using JobScheduler.Cron.JobExecutor;
using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class JobExecutorBackgroundService : BackgroundService
{
    private readonly IJobExecutor jobExecuter;

    public JobExecutorBackgroundService(IJobExecutor jobExecuter) => this.jobExecuter = jobExecuter;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => jobExecuter.Execute(stoppingToken);
}
