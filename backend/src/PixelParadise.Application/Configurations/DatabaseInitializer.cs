using Npgsql;
using PixelParadise.Infrastructure;
using ILogger = Serilog.ILogger;

/// <summary>
///     Handles the initialization of the database, ensuring it is ready for application use.
///     This includes creating or deleting the database depending on the environment configuration.
/// </summary>
public class DatabaseInitializer
{
    private readonly PixelParadiseContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    public DatabaseInitializer(PixelParadiseContext context, ILogger logger, IWebHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    ///     Asynchronously initializes the database.
    ///     In development mode, the existing database is deleted before ensuring it is recreated.
    ///     Handles retry logic in case of connection failures.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NpgsqlException">Thrown when database operations encounter an error.</exception>
    public async Task InitializeDatabaseAsync()
    {
        const int maxRetryCount = 5;
        const int delaySeconds = 2;
        var retryCount = 0;
        var dbConnectionSuccessful = false;

        while (retryCount < maxRetryCount && !dbConnectionSuccessful)
            try
            {
                if (_environment.IsDevelopment())
                {
                    _logger.Information("Development mode: Deleting existing database.");
                    await _context.Database.EnsureDeletedAsync();
                }

                _logger.Information("Ensuring database is created.");
                await _context.Database.EnsureCreatedAsync();

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
                    _logger.Fatal(
                        "Could not establish a connection to the database after several attempts. APPLICATION SHUTDOWN!!!");
                    Environment.Exit(1);
                }

                await Task.Delay(delayTime);
            }
    }
}