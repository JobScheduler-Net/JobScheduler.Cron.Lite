using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Hosting;

internal class AllJobsExecutorBackgroundService(IJob job) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await job.Execute(stoppingToken);
        }
        catch (TaskCanceledException)
        {
            // Expected exception
        }
    }
}
