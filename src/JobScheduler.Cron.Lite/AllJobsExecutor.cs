using JobScheduler.Cron.Lite.Configurations;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace JobScheduler.Cron.Lite;

internal sealed class AllJobsExecutor(IServiceProvider serviceProvider, IEnumerable<JobConfiguration> jobsConfiguration) : IJob
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
            DateTime timeReference = jobConfiguration.GetTimeReference(serviceProvider);
            DateTime nextOcurrence = crontabSchedule.GetNextOccurrence(timeReference);
            await Task.Delay(nextOcurrence - timeReference, cancellationToken);

            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            IJob job = (IJob)scope.ServiceProvider.GetRequiredService(jobConfiguration.JobType);
            await job.Execute(cancellationToken);
        }
    }
}