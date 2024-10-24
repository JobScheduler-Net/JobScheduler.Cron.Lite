namespace JobScheduler.Cron;

public interface IJob 
{
    Task Execute(CancellationToken cancellationToken);
}