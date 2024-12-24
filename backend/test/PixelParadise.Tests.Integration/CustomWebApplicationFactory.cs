using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PixelParadise.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace PixelParadise.Tests.Integration;

internal class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PixelParadiseContext>));
            services.AddDbContext<PixelParadiseContext>(options =>
                options.UseInMemoryDatabase("InMemoryDbForTesting"), ServiceLifetime.Singleton);
        });
    }
}