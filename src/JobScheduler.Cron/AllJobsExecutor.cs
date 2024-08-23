using JobScheduler.Cron.Configurations;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace JobScheduler.Cron;

internal class AllJobsExecutor(IServiceProvider serviceProvider, IEnumerable<JobConfiguration> jobsConfiguration) : IJob
{
    public Task Execute(CancellationToken cancellationToken)
    {
        IEnumerable<Task> jobSchedulers = jobsConfiguration.Select(x => CreateJobScheduler(x, cancellationToken));
        return Task.WhenAll(jobSchedulers);
    }

    private async Task CreateJobScheduler(JobConfiguration jobConfiguration, CancellationToken cancellationToken)
    {
        CrontabSchedule crontabSchedule = CrontabSchedule.Parse(jobConfiguration.Cron, new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = true,
        });

        while (!cancellationToken.IsCancellationRequested)
        {
            DateTime now = jobConfiguration.GetNow(serviceProvider);
            DateTime nextOcurrence = crontabSchedule.GetNextOccurrence(now);
            await Task.Delay(nextOcurrence - now, cancellationToken);

            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            var job = (IJob)scope.ServiceProvider.GetRequiredService(jobConfiguration.JobType);
            await job.Execute(cancellationToken);
        }
    }
}