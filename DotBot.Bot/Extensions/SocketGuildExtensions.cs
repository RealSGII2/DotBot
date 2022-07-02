using Discord.WebSocket;
using DotBot.Shared.Models.Database;
using DotBot.Shared.Services;

namespace DotBot.Bot.Extensions
{
    public static class SocketGuildExtensions
    {
        public static DataService DataService;

        public static DatabaseGuild GetData(this SocketGuild guild)
            => DataService.GetGuild(guild.Id);
    }
}
