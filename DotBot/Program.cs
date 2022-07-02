using DotBot.Bot;
using DotBot.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotBot
{
    public class Program
    {
        private static ServiceCollection _services = new();

        // For now, we'd like our program to be all-async.
        // So, we'll just make this get the result of Program.MainAsync,
        // which is an async function.
        public static void Main(string[] args)
            => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            _services
                .AddSingleton<DataService>()
                .AddSingleton<ConfigurationService>();

            Client client = new();
            client.ConfigureClientServices(ref _services);

            var serviceProvider = _services.BuildServiceProvider();
            await client.StartAsync(serviceProvider);

            // Prevent the program from exiting.
            // We may remove this once the server is added.
            await Task.Delay(-1);
        }
    }
}