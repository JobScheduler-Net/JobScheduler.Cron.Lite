namespace JobScheduler.Cron.Lite;

public interface IJob 
{
    Task Execute(CancellationToken cancellationToken);
}