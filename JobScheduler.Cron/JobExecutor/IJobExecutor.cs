namespace JobScheduler.Cron.JobExecutor;

public interface IJobExecutor
{
    Task Execute(CancellationToken cancellationToken);
}
