using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Movie.Infrastructure;

public static class SwaggerConfiguration
{
    public static void AddSwaggerGenOptions(this SwaggerGenOptions options,
        IConfigurationManager configurationManager)
    {
        IConfigurationSection documentSection = configurationManager.GetSection("Swagger:Document");
        IConfigurationSection securitySection = configurationManager.GetSection("Swagger:Security");

        string documentName = documentSection.GetValue<string>("Name", "v1")!;

        options.SwaggerDoc(documentName, new OpenApiInfo
        {
            Title = documentSection.GetValue<string>("Title", "Movie API"),
            Version = documentSection.GetValue<string>("Version", "v1"),
            Description = documentSection.GetValue<string>("Description")
        });

        string schemeName = securitySection.GetValue<string>("SchemeName", "Bearer")!;

        options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Scheme = securitySection.GetValue<string>("Scheme", "bearer"),
            BearerFormat = securitySection.GetValue<string>("BearerFormat", "JWT"),
            Name = securitySection.GetValue<string>("HeaderName", "Authorization"),
            Description = securitySection.GetValue<string>("Description")
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(schemeName, document)] = []
        });
    }

    public static void AddSwaggerUIOptions(this SwaggerUIOptions options,
        IHostEnvironment environment, IConfiguration configuration)
    {
        IConfigurationSection documentSection = configuration.GetSection("Swagger:Document");
        IConfigurationSection uiSection = configuration.GetSection("Swagger:UI");

        string documentName = documentSection.GetValue<string>("Name", "v1")!;
        string title = documentSection.GetValue<string>("Title", "Movie API")!;

        options.SwaggerEndpoint($"/swagger/{documentName}/swagger.json", $"{title} {documentName}");

        options.DocumentTitle = uiSection.GetValue("DocumentTitle", title);
        options.RoutePrefix = uiSection.GetValue<string>("RoutePrefix", "swagger");

        if (uiSection.GetValue<bool>("PersistAuthorization"))
            options.EnablePersistAuthorization();

        if (uiSection.GetValue<bool>("DisplayRequestDuration"))
            options.DisplayRequestDuration();

        if (uiSection.GetValue<bool>("TryItOutByDefault"))
            options.EnableTryItOutByDefault();

        if (uiSection.GetValue<bool>("EnableFilter"))
            options.EnableFilter();

        options.DocExpansion(DocExpansion.None);
        options.DefaultModelsExpandDepth(uiSection.GetValue<int>("DefaultModelsExpandDepth"));
    }
}