using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Cron.Configurations;

/// <summary>
/// Represents the configuration settings for a scheduled job.
/// </summary>
public class JobConfiguration
{
    /// <summary>
    /// Cron expression that defines the schedule for the job.
    /// The cron expression must be compatible with the NCrontab (https://github.com/atifaziz/NCrontab).
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// Function to get reference date and time to run job.
    /// By default, this function uses the <see cref="TimeProvider.GetUtcNow"/>.
    /// </summary>
    public Func<IServiceProvider, DateTime> GetNow { get; set; } = static serviceProvider =>
        serviceProvider.GetRequiredService<TimeProvider>().GetUtcNow().DateTime;

    /// <summary>
    /// The type of the job to be executed.
    /// This property is used internally to identify the specific job implementation that will be run
    /// according to the provided configuration. 
    /// </summary>
    internal Type JobType { get; set; }
}
