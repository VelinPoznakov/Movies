using System.Net.Http.Headers;
using Movie.ExternalSystems;
using Movie.ExternalSystems.Contracts;

namespace Movie.Infrastructure;

public static class HttpClientsConfigurations
{
    public static void AddExternalSystemServices(this IServiceCollection services, IConfiguration config)
    {
        IConfigurationSection section = config.GetSection("TMDBSettings");

        services.AddHttpClient<ITMDBHandler, TMDBHandler>("TMDB", client =>
        {
            client.BaseAddress =
                new Uri($"{section.GetValue<string>("Host")}{section.GetValue<string>("HostVersion")}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", section.GetValue<string>("AccessToken"));

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        services.AddHttpClient("TMDBImages", client =>
        {
            client.BaseAddress = new Uri(section.GetValue<string>("ImageHost")!); 
        });
    }
}