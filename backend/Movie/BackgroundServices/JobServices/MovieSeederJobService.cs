using Movie.BackgroundServices.Contracts;
using Movie.ExternalSystems.Contracts;

namespace Movie.BackgroundServices.JobServices;

public class MovieSeederJobService: IScheduleTask
{
    private readonly ITMDBHandler _requestHandler;
    private readonly ILogger<MovieSeederJobService> _logger;
    private readonly string _operationName;
        
    public MovieSeederJobService(
        ITMDBHandler requestHandler,
        ILogger<MovieSeederJobService> logger,
        HttpClient httpClient)
    {
        _requestHandler = requestHandler;
        _logger = logger;
        _operationName = nameof(MovieSeederJobService);
    }
    
    public Task ExecuteAsync(CancellationToken stoppingToken) => SeedMoviesAsync();

    private async Task SeedMoviesAsync()
    {
        _logger.LogInformation($"{nameof(MovieSeederJobService)} started");
        // make the service active in db
        // think about retry 
        // record error message
    }
}