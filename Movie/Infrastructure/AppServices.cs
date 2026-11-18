using Movie.BackgroundServices;
using Movie.BackgroundServices.Contracts;
using Movie.Data.Seed;
using Movie.ExternalSystems;
using Movie.ExternalSystems.Contracts;
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
    {
        // services.AddScoped<IDirectorRepository, DirectorRepository>();
        // services.AddScoped<IGenreRepository, GenreRepository>();
        // services.AddScoped<IMovieRepositiory, MovieRepositiory>();
        // services.AddScoped<ICommentsRepository, CommentsRepository>();
        // services.AddScoped<IRatingRepository, RatingRepository>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        
        // services.AddScoped<IDirectorService, DirectorService>();
        // services.AddScoped<IGenreService, GenreService>();
        // services.AddScoped<IMovieService, MovieService>();
        // services.AddScoped<IAuthService, AuthService>();
        // services.AddScoped<ICommentService, CommentsService>();
    }
    
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
}