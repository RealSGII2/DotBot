using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DotBot.Bot.Extensions;
using DotBot.Shared.Utility;

namespace DotBot.Bot.Modules
{
    public class ExperienceModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("rank", "Get your or another user's rank")]
        [UserCommand("Rank")]
        public async Task Rank(
            [Summary(description: "The user to get information from. Uses the user invoking this command if left empty")]
            SocketGuildUser? user = null
        )
        {
            // Animated avatars may take a tad bit to load, so we'll defer the reply
            // so we're not limited to 3 seconds.
            await DeferAsync(ephemeral: true);

            bool userFieldBlank = user is null;

            if (userFieldBlank)
                user = Context.Guild.GetUser(Context.User.Id);

            var experience = user.GetExperienceData();

            bool isAvatarAnimated = user.GetDisplayAvatarUrl().Contains(".gif");
            string fileFormat = isAvatarAnimated ? "gif" : "png";

            var card = await GraphicsUtility.DrawRankCard(
                    experience,
                    19,
                    user.GetDisplayAvatarUrl(),
                    user.DisplayName,
                    user.DiscriminatorValue,
                    isAvatarAnimated
                );

            string userRef = userFieldBlank ? "your" : $"{user.DisplayName}'s"; 

            await ModifyOriginalResponseAsync(
                m =>
                {
                    m.Attachments = new(
                        new List<FileAttachment>()
                        {
                            new FileAttachment(card, $"card.{fileFormat}")
                        }
                    );
                    m.Content = $"Here's {userRef} rank!";
                }
            );
        }

        [SlashCommand("data", "Get guild data.")]
        public async Task GetGuildData()
        {
            var data = Context.Guild.GetData();

            await RespondAsync($"**Guild Data**\nId: {data.Id}\nMaxXP: {data.MaxExperienceGain}\nMinXP: {data.MinExperienceGain}");
        }
    }
}

