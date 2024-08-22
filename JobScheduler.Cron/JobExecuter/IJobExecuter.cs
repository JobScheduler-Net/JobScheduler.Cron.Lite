namespace JobScheduler.Cron.JobExecuter;

public interface IJobExecuter
{
    Task Execute(CancellationToken cancellationToken);
}
