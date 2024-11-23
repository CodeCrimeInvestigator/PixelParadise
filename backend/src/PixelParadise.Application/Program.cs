using PixelParadise.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var initLogger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate:
        "INITIALISATION LOGGER: [{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
initLogger.Information("Starting the initialisation of Pixel Paradise... Registering Startup services...");
var startup = new Startup(builder.Configuration, initLogger);
startup.ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();
await startup.Configure(app, builder.Environment);
initLogger.Information("Ended the initialisation of Pixel Paradise... Starting the Application");
app.Run();