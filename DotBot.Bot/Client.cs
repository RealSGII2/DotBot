using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotBot.Bot.Components;
using DotBot.Bot.Components.Commands;
using DotBot.Bot.Extensions;
using DotBot.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotBot.Bot
{
    public class Client
    {
        private DiscordSocketClient _client;
        private InteractionService _interactionService;
        private Optional<ServiceProvider> _servicesValue = new();

        private ServiceProvider _services
        {
            get => _servicesValue.Value;
        }

        public Client()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
            });
            _client.Log += Log;
            _client.Ready += OnReady;

            _interactionService = new InteractionService(_client.Rest, new InteractionServiceConfig()
            {
                ThrowOnError = true,
                DefaultRunMode = RunMode.Sync,
                LogLevel = LogSeverity.Debug,
            });
            _interactionService.Log += Log;
        }

        public void ConfigureClientServices(ref ServiceCollection services)
        {
            services
                .AddSingleton(_client)
                .AddSingleton(_interactionService)
                .AddSingleton<CommandHandler>()
                .AddSingleton<ExperienceGain>();
        }

        public async Task StartAsync(ServiceProvider services)
        {
            _servicesValue = new(services);
            SocketGuildExtensions.DataService = _services.GetRequiredService<DataService>();
            var configuration = _services.GetRequiredService<ConfigurationService>();

            _services.GetRequiredService<ExperienceGain>();

            if (string.IsNullOrWhiteSpace(configuration.Configuration["tokens:discord"]))
                throw new ApplicationException("props.yml is missing tokens:discord. Please add your discord bot token there to start the bot.");

            // TODO: Move token and other properties to a config.yml file.
            await _client.LoginAsync(TokenType.Bot, configuration.Configuration["tokens:discord"]);
            await _client.StartAsync();
        }

        public async Task OnReady()
        {
            var commandHandler = _services.GetRequiredService<CommandHandler>();
            await commandHandler.HookCommandsAsync();
        }

        private static Task Log(LogMessage message)
        {
            // TODO: Write a proper logging sevice and use that here.
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
