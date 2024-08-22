using JobScheduler.Cron.JobExecuter;
using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class JobExecuterBackgroundService : BackgroundService
{
    private readonly IJobExecuter jobExecuter;

    public JobExecuterBackgroundService(IJobExecuter jobExecuter) => this.jobExecuter = jobExecuter;

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => jobExecuter.Execute(stoppingToken);
}
