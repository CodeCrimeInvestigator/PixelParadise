using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PixelParadise.Application.Options;
using PixelParadise.Infrastructure;

namespace PixelParadise.Application;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly StartupOptions _startupOptions;
    private readonly PostgreSqlOptions _postgreSqlOptions;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _postgreSqlOptions = _configuration.GetSection("PostgreSqlOptions").Get<PostgreSqlOptions>() ??
                             throw new InvalidConstraintException(
                                 "The `PostgreSQLSettings` section is not defined in the configuration.");
        _startupOptions = _configuration.GetSection("StartupOptions").Get<StartupOptions>() ??
                          throw new InvalidConstraintException(
                              "The `Startup` section is not defined in the configuration.");
    }

    
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment builderEnvironment)
    {
        services.AddDbContext<PixelParadiseContext>(options =>
            options.UseNpgsql(_postgreSqlOptions.GetConnectionString));
        
        if (_startupOptions.EnableSwagger)
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PixelParadise API", Version = "v1" });

                var filePath = Path.Combine(AppContext.BaseDirectory, "PixelParadise.xml");
                c.IncludeXmlComments(filePath);
            });
        
        services.AddControllers();
    }
    
    public async Task Configure(WebApplication app, IWebHostEnvironment builderEnvironment)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PixelParadiseContext>();
        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
            await context.Database.EnsureDeletedAsync();
        }
        await context.Database.EnsureCreatedAsync();
        
        app.UsePathBase("/api");
        
        if (_startupOptions.EnableSwagger)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "PixelParadise API V1");
                c.RoutePrefix = "swagger";
            });
        }

    }
}