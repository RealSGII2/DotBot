using Discord.WebSocket;
using DotBot.Bot.Components;
using DotBot.Bot.Extensions;
using DotBot.Shared.Services;
using DotBot.Tests.Mocks;
using DotBot.Tests.Mocks.SocketGuild;

namespace DotBot.Tests
{
    public class ExperienceGainTests
    {
        private readonly DataService _data = new(@".\test.db");
        private readonly ClientMock _client = new();
        private readonly ExperienceGain _experienceGain = new();
        private readonly GuildMock _guild;
        private readonly GuildChannelMock _channel;

        public ExperienceGainTests()
        {
            _guild = new() { DataService = _data };
            _channel = new GuildChannelMock()
            {
                Guild = _guild,
            };

            SocketGuildExtensions.DataService = _data;
        }

        [Theory]
        [InlineData(1, 15, 25, 15, 25)]
        [InlineData(2, 15, 25, 30, 50)]
        [InlineData(1, 5, 15, 5, 15)]
        [InlineData(2, 5, 15, 10, 30)]
        public async Task ExperienceGainTheory(uint messageCount, uint minXP, uint maxXP, uint minExpectedXP, uint maxExpectedXP)
        {
            GuildUserMock user = new();

            var data = _data.GetGuild(_guild.Id);
            data.MinExperienceGain = minXP;
            data.MaxExperienceGain = maxXP;
            data.Save();

            for (var i = 0; i < messageCount; i++)
                await _experienceGain.OnMessage(new MessageMock { Author = user, Channel = _channel });

            var newData = _data.GetGuild(_guild.Id);

            Assert.NotNull(newData.UserExperience[user.Id]);
#pragma warning disable CS8629 // We literally check if it's not null above, thanks IntelliSense
            Assert.InRange((uint)newData.UserExperience[user.Id], minExpectedXP, maxExpectedXP);
#pragma warning restore CS8629 
        }
    }
}