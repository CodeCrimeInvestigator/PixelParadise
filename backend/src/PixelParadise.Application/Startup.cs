using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PixelParadise.Application.Options;
using PixelParadise.Application.Services;
using PixelParadise.Infrastructure;
using PixelParadise.Infrastructure.Repositories;

namespace PixelParadise.Application;

/// <summary>
/// The Startup class is responsible for configuring services and the application pipeline.
/// It reads configuration settings and sets up dependency injection for the application.
/// </summary>
public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly StartupOptions _startupOptions;
    private readonly PostgreSqlOptions _postgreSqlOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
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

    /// <summary>
    /// Configures the services for the application.
    /// This method is called by the runtime to add services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="builderEnvironment">The hosting environment the application is running in.</param>
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment builderEnvironment)
    {
        services.AddDbContext<PixelParadiseContext>(options =>
            options.UseNpgsql(_postgreSqlOptions.GetConnectionString), contextLifetime: ServiceLifetime.Singleton);

        services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserService, UserService>();


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

    /// <summary>
    /// Configures the application request pipeline.
    /// This method is called by the runtime to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="builderEnvironment">The hosting environment the application is running in.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Configure(WebApplication app, IWebHostEnvironment builderEnvironment)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<PixelParadiseContext>();
            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                await context.Database.EnsureDeletedAsync();
            }

            await context.Database.EnsureCreatedAsync();
        }

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

        app.MapControllers();
    }
}