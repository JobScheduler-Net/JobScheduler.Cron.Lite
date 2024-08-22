using JobScheduler.Cron.Configurations;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace JobScheduler.Cron.JobExecuter;

public class JobExecuter : IJobExecuter
{
    private readonly IServiceProvider serviceProvider;
    private readonly IEnumerable<JobConfiguration> jobsConfiguration;

    public JobExecuter(IServiceProvider serviceProvider, IEnumerable<JobConfiguration> jobsConfiguration)
    {
        this.serviceProvider = serviceProvider;
        this.jobsConfiguration = jobsConfiguration;
    }

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
            await jobConfiguration.OnExecute(scope.ServiceProvider, cancellationToken);
        }
    }
}