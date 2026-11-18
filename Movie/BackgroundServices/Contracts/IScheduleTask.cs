namespace Movie.BackgroundServices.Contracts;

public interface IScheduleTask
{
    Task ExecuteAsync(CancellationToken stoppingToken);
}