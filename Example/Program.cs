using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example;

class Program
{
    static async Task Main(string[] args)
    {
        // Create service collection and configure our services
        var services = ConfigureServices();

        // Generate a provider
        var serviceProvider = services.BuildServiceProvider();

        // Kick off our actual code
        await serviceProvider.GetService<Application>().RunAsync();
    }

    private static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        var appConfigSecretName = "data-development";

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddSecretsManager(appConfigSecretName)
            .Build();

        // External services
        services.Configure<DataOptions>(options => config.GetSection("Data").Bind(options));

        // Internal services
        services.AddTransient<Application>();

        return services;
    }
}
