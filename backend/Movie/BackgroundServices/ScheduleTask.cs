using Cronos;
using Movie.BackgroundServices.Contracts;

namespace Movie.BackgroundServices;

public class ScheduleTask<TJob>: BackgroundService where TJob: class, IScheduleTask
{
    private readonly ILogger<ScheduleTask<TJob>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _jobName;
    private readonly bool _isEnabled;
    private readonly string _cronPattern;
    private readonly bool _runOnStartup;
    private readonly string _timeZone = "Europe/Sofia";
    
    public ScheduleTask(
        ILogger<ScheduleTask<TJob>> logger,
        IConfiguration config,
        IServiceScopeFactory scopeFactory)
    {
        this._logger = logger;
        this._jobName = typeof(TJob).Name;
        this._isEnabled = config.GetValue<bool>($"JobServices:{this._jobName}:Enabled");
        this._cronPattern = config.GetValue<string>($"JobServices:{this._jobName}:CronPattern")
            ?? throw new NullReferenceException("Cron pattern is missing");
        this._runOnStartup = config.GetValue<bool>($"JobServices:{this._jobName}:RunOnStratUp");
        this._scopeFactory = scopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if(!this._isEnabled)
            return;

        CronExpression cron = CronExpression.Parse(this._cronPattern);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(this._timeZone);
        
        if(this._runOnStartup)
            await RunJobAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            DateTimeOffset? next = cron.GetNextOccurrence(DateTimeOffset.UtcNow, timeZoneInfo);

            if (next == null)
            {
                this._logger.LogWarning($"{this._jobName} cron pattern has no future occurrences");
                
                return;
            }
            
            try
            {
                await DelayUntilAsync(next.Value, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            await RunJobAsync(stoppingToken);
        }
    }

    private async Task RunJobAsync(CancellationToken ct)
    {
        try
        {
            using IServiceScope scope = this._scopeFactory.CreateScope();
            TJob job = scope.ServiceProvider.GetRequiredService<TJob>();
        
            this._logger.LogInformation($"Running job {this._jobName} at {DateTime.UtcNow.ToString("")}");

            await job.ExecuteAsync(ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            this._logger.LogInformation($"{this._jobName} cancelled because the app is stopping");
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, $"{this._jobName} failed");
        }
    }
    
    private static async Task DelayUntilAsync(DateTimeOffset target, CancellationToken ct)
    {
        while (true)
        {
            TimeSpan remaining = target - DateTimeOffset.UtcNow;
            if (remaining <= TimeSpan.Zero) return;

            await Task.Delay(remaining > TimeSpan.FromDays(1) ? TimeSpan.FromDays(1) : remaining, ct);
        }
    }
}