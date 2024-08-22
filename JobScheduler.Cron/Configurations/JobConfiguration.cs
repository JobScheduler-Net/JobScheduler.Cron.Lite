using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Cron.Configurations;

/// <summary>
/// Represents the configuration settings for a scheduled job.
/// </summary>
public class JobConfiguration
{
    /// <summary>
    /// Gets or sets the cron expression that defines the schedule for the job.
    /// The cron expression must be compatible with the NCrontab library 
    /// (https://github.com/atifaziz/NCrontab).
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// Gets or sets the function that will be executed when the job is triggered.
    /// The function takes an <see cref="IServiceProvider"/> and a <see cref="CancellationToken"/>
    /// as parameters and returns a <see cref="Task"/>.
    /// The <see cref="IServiceProvider"/> provided will be from a scope created 
    /// using the <see cref="ServiceProviderServiceExtensions.CreateAsyncScope"/> method.
    /// </summary>
    public Func<IServiceProvider, CancellationToken, Task> OnExecute { get; set; }

    /// <summary>
    /// Gets or sets the function that returns the current date and time.
    /// By default, this function uses the <see cref="TimeProvider"/> service to get the current UTC date and time.
    /// </summary>
    public Func<IServiceProvider, DateTime> GetNow { get; set; } = serviceProvider =>
        serviceProvider.GetRequiredService<TimeProvider>().GetUtcNow().DateTime;
}
