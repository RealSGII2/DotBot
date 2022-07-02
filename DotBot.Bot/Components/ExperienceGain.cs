using Discord.WebSocket;
using DotBot.Bot.Extensions;

namespace DotBot.Bot.Components
{
    public class ExperienceGain
    {
        private readonly DiscordSocketClient _client;
        private readonly Random _random = new();

        public ExperienceGain(DiscordSocketClient client)
        {
            Console.WriteLine("Exp Gain Init!");
            _client = client;
            _client.MessageReceived += OnMessage;
        }

        private Task OnMessage(SocketMessage message)
        {
            Console.WriteLine("Got the message event!");
            if (message.Channel is not SocketGuildChannel)
                return Task.CompletedTask;

            var channel = (SocketGuildChannel)message.Channel;
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
