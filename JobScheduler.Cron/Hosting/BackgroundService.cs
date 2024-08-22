using JobScheduler.Cron.JobExecutor;
using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class JobExecuterBackgroundService : BackgroundService
{
    private readonly IJobExecutor jobExecuter;

    public JobExecuterBackgroundService(IJobExecutor jobExecuter) => this.jobExecuter = jobExecuter;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => jobExecuter.Execute(stoppingToken);
}
