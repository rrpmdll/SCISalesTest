using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Domain.Options;
using SCISalesTest.Domain.Repositories;
using SCISalesTest.Domain.Resources;
using SCISalesTest.Infrastructure.Context;
using SCISalesTest.Infrastructure.Repositories;
using SCISalesTest.Infrastructure.Resources;
using SCISalesTest.Infrastructure.Services;

namespace SCISalesTest.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        AddOptions(services, configuration);
        AddDatabaseContext(services);
        AddRepositories(services);
        AddResourceProviders(services);
        AddApplicationServices(services);
        AddExternalServices(services);
        AddHttpClients(services, configuration);
        AddAutoMapper(services);
        AddMediatR(services);

        return services;
    }

    private static void AddOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Key));
        services.Configure<ExchangeRateOptions>(configuration.GetSection(ExchangeRateOptions.Key));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Key));
    }

    private static void AddDatabaseContext(IServiceCollection services)
    {
        services.AddSingleton<DapperContext>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<ProductService>();
    }

    private static void AddResourceProviders(IServiceCollection services)
    {
        services.AddTransient<IMessagesProvider, MessagesProvider>();
    }

    private static void AddExternalServices(IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
    }

    private static void AddHttpClients(IServiceCollection services, IConfiguration configuration)
    {
        var exchangeRateOptions = configuration.GetSection(ExchangeRateOptions.Key).Get<ExchangeRateOptions>();

        services.AddHttpClient("ExchangeRate", client =>
        {
            client.BaseAddress = new Uri(exchangeRateOptions?.BaseAddress ?? "https://open.er-api.com");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Application.Feature.Products.ProductProfile).Assembly);
    }

    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.Feature.Products.ProductProfile).Assembly));
    }
}
