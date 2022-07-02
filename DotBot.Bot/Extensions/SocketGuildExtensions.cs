using Discord;
using DotBot.Shared.Models.Database;
using DotBot.Shared.Services;

namespace DotBot.Bot.Extensions
{
    public static class SocketGuildExtensions
    {
        public static DataService DataService;

        public static DatabaseGuild GetData(this IGuild guild)
            => DataService.GetGuild(guild.Id);
    }
}
