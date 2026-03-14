using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Domain.Repositories;

namespace SCISalesTest.WebApi.IntegrationTest.Setup;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IProductRepository> ProductRepositoryMock { get; } = new();
    public Mock<IExchangeRateService> ExchangeRateServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            RemoveService<IProductRepository>(services);
            RemoveService<IExchangeRateService>(services);

            services.AddScoped(_ => ProductRepositoryMock.Object);
            services.AddScoped(_ => ExchangeRateServiceMock.Object);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
        });
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null)
            services.Remove(descriptor);
    }
}
