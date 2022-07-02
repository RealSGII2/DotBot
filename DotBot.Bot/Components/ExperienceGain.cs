using Discord;
using Discord.WebSocket;
using DotBot.Bot.Extensions;
using DotBot.Shared.Services;

namespace DotBot.Bot.Components
{
    public class ExperienceGain
    {
        private readonly DiscordSocketClient? _client;
        private static readonly Random _random = new();

        public ExperienceGain() { }

        public ExperienceGain(IDiscordClient client)
        {
            _client = (DiscordSocketClient)client;
            _client.MessageReceived += OnMessage;
        }

        public Task OnMessage(IMessage message)
        {
            if (message.Channel is not IGuildChannel)
                return Task.CompletedTask;

            var channel = (IGuildChannel)message.Channel;
            var guildData = channel.Guild.GetData();

            var gain = _random.Next((int)guildData.MinExperienceGain, (int)guildData.MaxExperienceGain);

            if (guildData.UserExperience.ContainsKey(message.Author.Id))
                guildData.UserExperience[message.Author.Id] += (uint)gain;
            else
                guildData.UserExperience[message.Author.Id] = (uint)gain;

            guildData.Save();

            return Task.CompletedTask;
        }
    }
}
