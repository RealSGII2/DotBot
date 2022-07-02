using Discord.WebSocket;
using DotBot.Shared.Models;

namespace DotBot.Bot.Extensions
{
    public static class SocketGuildUserExtensions
    {
        public static UserExperienceData GetExperienceData(this SocketGuildUser user)
        {
            var data = user.Guild.GetData();
            data.UserExperience.TryGetValue(user.Id, out uint? experience);
            experience ??= 0;

            return new UserExperienceData()
            {
                TotalExperience = (uint)experience,
            };
        }
    }
}
