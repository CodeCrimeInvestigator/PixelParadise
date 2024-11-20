using PixelParadise.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var initLogger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration, sectionName: "Serilog:Initialisation")
    .CreateLogger();

initLogger.Information("Starting the initialisation of Pixel Paradise... Registering Startup services...");
var startup = new Startup(builder.Configuration, initLogger);
startup.ConfigureServices(builder.Services, builder.Environment);
startup.ConfigureHost(builder.Host);

var app = builder.Build();
await startup.Configure(app, builder.Environment);
initLogger.Information("Ended the initialisation of Pixel Paradise... Starting the Application");
app.Run();