using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace DotBot.Bot.Components.Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services, InteractionService interactionService, IDiscordClient client)
        {
            _interactionService = interactionService;
            _services = services;
            _client = (DiscordSocketClient)client;
        }

        public async Task HookCommandsAsync()
        {

            await _interactionService.AddModulesAsync(
                assembly: Assembly.GetAssembly(typeof(Client)),
                services: _services
            );

            //_interactionService.InteractionExecuted += HandleCommand;
            _client.InteractionCreated += HandleInteraction;


            // Discord takes time to propagate global commands;
            // if debugging, we'll just send the commands to our
            // test server as it'll propagate instantly.
#if DEBUG
            await _interactionService.RegisterCommandsToGuildAsync(992111983484735559);
#else
            await _interactionService.AddCommandsGloballyAsync();
#endif
        }

        public async Task HandleInteraction(SocketInteraction interaction)
        {
            await _interactionService.ExecuteCommandAsync(new SocketInteractionContext(_client, interaction), _services);
        }
    }
}
