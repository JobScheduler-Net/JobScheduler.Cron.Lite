using JobScheduler.Cron.Configurations;
using JobScheduler.Cron.Hosting;
using JobScheduler.Cron.JobExecutor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JobScheduler.Cron.DependencyInjection;

/// <summary>
/// Extension methods for adding job scheduling services to the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionJobSchedulerServiceExtensions
{
    /// <summary>
    /// Adds the job scheduler services, including the hosted service, to the specified 
    /// <see cref="IServiceCollection"/>. This includes the <see cref="TimeProvider"/>, 
    /// <see cref="IJob"/>, and <see cref="Microsoft.Extensions.Hosting.IHostedService"/> 
    /// (implemented by <see cref="JobExecutorBackgroundService"/>) for executing scheduled jobs. 
    /// The method can be called multiple times to register different job configurations. 
    /// Note that the application will follow the patterns of <see cref="IHostedService"/>, and the 
    /// <see cref="CancellationToken"/> provided by the hosted service will be propagated to the jobs. 
    /// The job executor does not handle exceptions; it is the responsibility of the caller to ensure 
    /// that the job configuration is valid and that any necessary error handling is implemented within 
    /// the job itself.
    /// 
    /// This method internally injects an <see cref="IJob"/> implementation that is responsible for 
    /// executing all registered jobs. This internal <see cref="IJob"/> should not be overridden by 
    /// an external injection. If you need to manually execute jobs, you can resolve the 
    /// <see cref="IJob"/> instance from the service provider and invoke it as needed.
    /// </summary>
    /// <typeparam name="TJob">The implementation of <see cref="IJob"/></typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="jobConfiguration">The configuration for the scheduled job.</param>
    /// <returns>The original <see cref="IServiceCollection"/> with the job scheduler services added.</returns> 
    public static IServiceCollection AddHostedJobScheduler<TJob>(this IServiceCollection services,
        JobConfiguration jobConfiguration) where TJob : class, IJob => services
            .AddJobScheduler<TJob>(jobConfiguration)
            .AddHostedService<JobExecutorBackgroundService>();

    /// <summary>
    /// Adds the job scheduler services, excluding the hosted service, to the specified 
    /// <see cref="IServiceCollection"/>. This includes the <see cref="TimeProvider"/> and 
    /// <see cref="IJob"/>. The method can be called multiple times to register different 
    /// job configurations. Note that the job executor does not handle exceptions; it is the 
    /// responsibility of the caller to ensure that the job configuration is valid and that any 
    /// necessary error handling is implemented within the job itself.
    /// 
    /// This method internally injects an <see cref="IJob"/> implementation that is responsible for 
    /// executing all registered jobs. This internal <see cref="IJob"/> should not be overridden by 
    /// an external injection. If you need to manually execute jobs, you can resolve the 
    /// <see cref="IJob"/> instance from the service provider and invoke it as needed.
    /// </summary>
    /// <typeparam name="TJob">The implementation of <see cref="IJob"/></typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="jobConfiguration">The configuration for the scheduled job.</param>
    /// <returns>The original <see cref="IServiceCollection"/> with the job scheduler services added.</returns>
    public static IServiceCollection AddJobScheduler<TJob>(this IServiceCollection services,
        JobConfiguration jobConfiguration) where TJob : class, IJob
    {
        jobConfiguration.JobType = typeof(TJob);
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<IJob, AllJobsExecutor>();
        return services
            .AddSingleton(jobConfiguration)
            .AddTransient<TJob>();
    }
}