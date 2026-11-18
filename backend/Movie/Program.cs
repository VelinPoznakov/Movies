using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Entities;
using Microsoft.AspNetCore.Identity;
using Movie.Infrastructure;

namespace Movie
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentException("Database connection string missing");
            
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseNpgsql(dbConnectionString));

            builder.Services
                .AddIdentityCore<AppUser>(options => options.AddIdentityConfiguration(builder.Configuration))
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<MovieDbContext>();
            
            builder
                .Services
                .AddAuthentication(options => options.AddShemaConfiguration())
                .AddJwtBearer(options => options.AddJWTBearerOptions(builder.Configuration));

            builder.Services.AddServices();
            builder.Services.StartupSeeders();
            builder.Services.AddRepositories();
            builder.Services.AddBackgroundServices();
            
            builder.Services.AddAzureBlobStorage(builder.Configuration);
            
            string[] cors = builder
                .Configuration
                .GetSection("Cors")
                .ToString()?
                .Split(", ") ?? throw new ArgumentException("Cors configuration missing");

            builder.Services.AddCors(options => options.AddCorsOptions(cors));
            
            builder.Services.AddExternalSystemServices(builder.Configuration);
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => options.AddSwaggerGenOptions(builder.Configuration));

            WebApplication app = builder.Build();

            await app.EnsureBlobContainersExistsAsync(app.Configuration);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options => options.AddSwaggerUIOptions(app.Environment, app.Configuration));
            }

            app.RoleSeeder(app.Configuration);
            
            app.UseCors(CorsPolicies.CORSPOLICYNAME);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
