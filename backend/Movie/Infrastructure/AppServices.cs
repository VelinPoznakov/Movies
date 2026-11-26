using Movie.BackgroundServices;
using Movie.BackgroundServices.Contracts;
using Movie.Data.Seed;
using Movie.Repositories;
using Movie.Repositories.Interfaces;
using Movie.Services;
using Movie.Services.Interfaces;

namespace Movie.Infrastructure;

public static class AppServices
{
    public static void StartupSeeders(this IServiceCollection services)
    {
        services.AddScoped<IIdentitySeeder, IdentitySeeder>();
    }
    
    public static void AddServices(this IServiceCollection services)
        => AddScoped(services, typeof(IMovieService), typeof(MovieService));

    public static void AddRepositories(this IServiceCollection services)
        => AddScoped(services, typeof(IBaseRepository<>), typeof(BaseRepository<>));
    
    public static void AddBackgroundServices(this IServiceCollection services)
    {
        IEnumerable<Type> jobTypes = typeof(IScheduleTask).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IScheduleTask)));

        foreach (Type jobType in jobTypes)
        {
            services.AddScoped(jobType);
            services.AddSingleton(typeof(IHostedService), typeof(ScheduleTask<>).MakeGenericType(jobType));
        }
    }

    private static void AddScoped(IServiceCollection services, Type interfaceType, Type implementationType)
    {
        string interfacesNamespace = interfaceType.Namespace!;
        string implementationNamespace = implementationType.Namespace!;

        Type[] implementationTypes = implementationType.Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.Namespace == implementationNamespace)
            .ToArray();

        IEnumerable<Type> interfaceTypes = interfaceType.Assembly.GetTypes()
            .Where(t => t.IsInterface && t.Namespace == interfacesNamespace);

        foreach (Type foundInterfaceType in interfaceTypes)
        {
            Type implementation = implementationTypes
                                      .FirstOrDefault(t => "I" + t.Name == interfaceType.Name && Implements(t, interfaceType))
                                  ?? throw new InvalidOperationException(
                                      $"Missing implementation for {interfaceType.FullName}");

            services.AddScoped(foundInterfaceType, implementation);
        }
    }
    
    private static bool Implements(Type impl, Type contract) =>
        contract.IsGenericTypeDefinition
            ? impl.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == contract)
            : contract.IsAssignableFrom(impl);
}