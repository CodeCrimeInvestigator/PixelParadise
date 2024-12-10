using System.Data;
using Npgsql;
using PixelParadise.Application.Logging;
using PixelParadise.Application.Mapping;
using PixelParadise.Application.Options;
using PixelParadise.Infrastructure;
using ILogger = Serilog.ILogger;

namespace PixelParadise.Application;

/// <summary>
///     The Startup class is responsible for configuring services and the application pipeline.
///     It reads configuration settings and sets up dependency injection for the application.
/// </summary>
public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly PostgreSqlOptions _postgreSqlOptions;
    private readonly StartupOptions _startupOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Startup" /> class.
    /// </summary>
    public Startup(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _postgreSqlOptions = _configuration.GetSection("PostgreSqlOptions").Get<PostgreSqlOptions>() ??
                             throw new InvalidConstraintException(
                                 "The `PostgreSQLSettings` section is not defined in the configuration.");
        _startupOptions = _configuration.GetSection("StartupOptions").Get<StartupOptions>() ??
                          throw new InvalidConstraintException(
                              "The `Startup` section is not defined in the configuration.");
        _logger = logger;
    }

    /// <summary>
    ///     Configures the services for the application.
    ///     This method is called by the runtime to add services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="builderEnvironment">The hosting environment the application is running in.</param>
    public void ConfigureServices(IServiceCollection services, IWebHostEnvironment builderEnvironment)
    {
        services.Configure<StorageOptions>(_configuration.GetSection("StorageOptions"));
        services.AddDbContext(_postgreSqlOptions);
        services.AddRepositories();
        services.AddServices();
        services.AddValidators();
        services.AddLogging(_configuration);
        services.AddSwagger(_startupOptions);
        services.AddControllers();
    }

    /// <summary>
    ///     Configures the application request pipeline.
    ///     This method is called by the runtime to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="builderEnvironment">The hosting environment the application is running in.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Configure(WebApplication app, IWebHostEnvironment builderEnvironment)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PixelParadiseContext>();

        var retryCount = 0;
        const int maxRetryCount = 3;
        const int delaySeconds = 2;
        var dbConnectionSuccessful = false;

        //TODO: extract logic to method
        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
            await context.Database.EnsureDeletedAsync();
        }

        while (retryCount < maxRetryCount && !dbConnectionSuccessful)
            //TODO: extract this functionality to a separate class
            try
            {
                await context.Database.EnsureCreatedAsync();
                _logger.Information("Database connection successful.");
                dbConnectionSuccessful = true;
            }
            catch (NpgsqlException ex)
            {
                retryCount++;
                var delayTime = TimeSpan.FromSeconds(Math.Pow(delaySeconds, retryCount));
                _logger.Error(
                    $"Database connection failed. Attempt {retryCount} of {maxRetryCount}. Retrying in {delayTime.TotalSeconds} seconds.");
                _logger.Verbose($"Exception details: {ex.Message}");

                if (retryCount >= maxRetryCount)
                {
                    _logger.Fatal("Could not establish a connection to the database after several attempts.");
                    Environment.Exit(1);
                }

                await Task.Delay(delayTime);
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

        app.UseMiddleware<ValidationMappingMiddleware>();
        app.MapControllers();
    }
}