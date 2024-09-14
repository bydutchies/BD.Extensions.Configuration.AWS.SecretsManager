using Microsoft.Extensions.Options;

namespace Example;

internal class Application
{
    private readonly DataOptions _options;

    public Application(IOptions<DataOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Application starting point 
    /// </summary>
    public async Task RunAsync()
    {
        Console.WriteLine(_options.SqlConnectionString);
        Console.WriteLine(_options.ServerName);
        Console.ReadKey();
    }
}
