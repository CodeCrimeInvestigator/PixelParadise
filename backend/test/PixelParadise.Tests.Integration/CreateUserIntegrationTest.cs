using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PixelParadise.Application.Contracts.Requests;
using PixelParadise.Infrastructure;

namespace PixelParadise.Tests.Integration;

public class CreateUserIntegrationTest
{
    [Fact]
    public async Task Get_ShouldReturn200OK_WhenDataIsSeeded()
    {
        var application = new CustomWebApplicationFactory<Program>();
        var client = application.CreateClient( new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
        
        using var scope = application.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PixelParadiseContext>();
            
        // Ensure seeding is awaited
        await Seeding.InitialiseTestDB(dbContext);

        // Act: Make a GET request to your endpoint
        var response = await client.GetAsync($"/api/users/eedca410-4304-4020-a175-ccea232fd8ca");  // Adjust this URL as needed

        // Assert: Check that the response status is OK
        response.EnsureSuccessStatusCode();

        // Check that the users are in the response content
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("usr1", content);
    }
    
    [Fact]
    public async Task Post_ShouldReturn201Created_WhenUserIsValid()
    {
        var application = new CustomWebApplicationFactory<Program>();
        var client = application.CreateClient();

        // Create a new user object for testing
        var newUser = new CreateUserRequest
        {
            UserName = "usr4",
            NickName = "nick4",
            Email = "user4@gmail.com",
            Age = 25
        };

        // Act: Send a POST request to create a new user
        var response = await client.PostAsJsonAsync("/api/users", newUser);

        // Assert: Check that the response status is 201 Created
        response.EnsureSuccessStatusCode();

        // Optionally, check the response content
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("usr4", content);
    }
}