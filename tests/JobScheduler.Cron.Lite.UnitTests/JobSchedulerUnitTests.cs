using JobScheduler.Cron.Lite.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobScheduler.Cron.Lite.UnitTests;

public class HostedJobSchedulerUnitTests
{
    [Fact]
    public Task HostedJobScheduler_Should_Call_Job_Execute_As_Per_Cron() => ExecuteTest(Job.AllowedTimeMargin);

    // Timeline:
    // |-------1-------|-------2-------|-------3-------|-------4-------|-------5-------|-------6-------|
    // |               |               |               |               |               |               |
    // | Execution #1  | End Exec #1   | Execution #2  | End Exec #2   | Execution #3  | End Exec #3   |
    // |<---------->   |<-->           |<---------->   |<-->           |<---------->   |<-->           |        
    [Fact]
    public Task HostedJobScheduler_Should_Skip_Execution_When_Previous_Job_Is_Still_Running() => ExecuteTest(
        Job.CronTimeBetweenExecutions + Job.AllowedTimeMargin,
        TimeSpan.FromTicks(Job.CronTimeBetweenExecutions.Ticks * 2),
        Job.CountExpectedExecutions / 2);

    private static async Task ExecuteTest(TimeSpan jobExecutionTime, TimeSpan? timeBetweenExecutions = default,
        int countExpectedExecutions = Job.CountExpectedExecutions)
    {
        Job.ExecutionTime = jobExecutionTime;
        Job job;
        using (IHost host = CreateHost())
        {
            await host.StartAsync();
            await Task.Delay(TimeSpan.FromTicks(Job.CronTimeBetweenExecutions.Ticks * Job.CountExpectedExecutions));
            job = host.Services.GetRequiredService<Job>();
        }
        AssertJob(job, timeBetweenExecutions, countExpectedExecutions);
    }

    private static IHost CreateHost() => Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) => services
            .AddJobScheduler<Job>(new()
            {
                Cron = Job.Cron
            })
            .AddHostedJobScheduler()
            .AddSingleton<Job>()
        )
        .Build();

    private static void AssertJob(Job Job, TimeSpan? timeBetweenExecutions, int countExpectedExecutions)
    {
        TimeSpan expectedTimeBetweenExecutions = timeBetweenExecutions ?? Job.CronTimeBetweenExecutions;

        Assert.InRange(Job.TimeStartExecution.Count, countExpectedExecutions - 1, countExpectedExecutions + 1);
        for (int i = 1; i < Job.TimeStartExecution.Count; i++)
        {
            TimeSpan timeDifference = Job.TimeStartExecution[i] - Job.TimeStartExecution[i - 1];
            Assert.InRange(timeDifference, expectedTimeBetweenExecutions - Job.AllowedTimeMargin,
                expectedTimeBetweenExecutions + Job.AllowedTimeMargin);
        }
        Assert.True(Job.ExecuteDisposeAsync);
    }
}