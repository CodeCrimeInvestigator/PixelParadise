using PixelParadise.Application;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();
await startup.Configure(app, builder.Environment);
app.Run();