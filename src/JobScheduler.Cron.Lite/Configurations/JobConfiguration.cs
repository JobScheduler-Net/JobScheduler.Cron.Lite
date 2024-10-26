﻿using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Cron.Lite.Configurations;

/// <summary>
/// Represents the configuration settings for a scheduled job.
/// </summary>
public class JobConfiguration
{
    /// <summary>
    /// Cron expression that defines the schedule for the job.
    /// The cron expression must follow the standard format with seconds and 
    /// be compatible with NCrontab (https://github.com/atifaziz/NCrontab).
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// Function to get the reference date and time for running the job.
    /// By default, this function uses the <see cref="TimeProvider.GetUtcNow().DateTime"/>.
    /// </summary>
    public Func<IServiceProvider, DateTime> GetTimeReference { get; set; } = static serviceProvider =>
        serviceProvider.GetRequiredService<TimeProvider>().GetUtcNow().DateTime;

    /// <summary>
    /// The type of the job to be executed.
    /// This property is used internally to identify the specific job implementation that will be run
    /// according to the provided configuration. 
    /// </summary>
    internal Type JobType { get; set; }
}