using DotBot.Shared.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DotBot.Shared.Utility
{
    public class GraphicsFonts
    {
        public readonly FontFamily Outfit;

        public GraphicsFonts()
        {
            FontCollection collection = new();
            Outfit = collection.Add(@"C:\Users\willi\source\repos\DotBot\DotBot.Shared\Fonts\Outfit-Regular.ttf");
        }
    }

    public static class GraphicsUtility
    {
        public static readonly GraphicsFonts Font = new();

        public static readonly int RankCardWidth = 600;
        public static readonly int RankCardHeight = 200;
        public static readonly int RankCardPadding = 30;

        private static readonly PngEncoder pngEncoder = new();
        private static readonly GifEncoder gifEncoder = new();

        public static async Task<MemoryStream> DrawRankCard(
            UserExperienceData experienceData,
            int rank,
            string avatarUrl,
            string username,
            int discrim,
            bool isAnimated
        )
        {
            float avatarSize = (RankCardHeight - RankCardPadding * 2) / 2;

            HttpClient avatarFetchClient = new();
            var avatarImage = Image.Load(await avatarFetchClient.GetStreamAsync(avatarUrl));
            avatarImage.Mutate(i => i.Resize((int)avatarSize * 2, (int)avatarSize * 2));

            int frameCount = isAnimated ? avatarImage.Frames.Count : 1;

            using Image<Rgba32> baseImage = new(RankCardWidth, RankCardHeight);
            using Image<Rgba32> finalImage = new(RankCardWidth, RankCardHeight);

            baseImage.Mutate(i => {
                // Add a background
                i.Fill(Color.FromRgb(22, 27, 34));

                // Draw the username
                i.DrawText(username, Font.Outfit.CreateFont(30), Color.White, new PointF(200f, 41f));

                // Draw the level bar.
                LinearLineSegment levelBar = new(new PointF(210f, 94f), new PointF(560f, 94f));
                Pen levelBarPen = new(Color.FromRgb(52, 57, 66), 20);
                levelBarPen.EndCapStyle = EndCapStyle.Round;
                i.Draw(levelBarPen, new SixLabors.ImageSharp.Drawing.Path(levelBar));

                // Draw the level bar's progress.
                var progressX = 210f + 350f * (Convert.ToSingle(experienceData.CurrentExperience) / Convert.ToSingle(experienceData.ExperienceToNextLevel));

                if (progressX is not 210f)
                {
                    LinearLineSegment levelProgressBar = new(
                    new PointF(210f, 94f),
                    new PointF(
                        progressX,
                        94f
                    )
                );
                    Pen levelProgressBarPen = new(Color.FromRgb(31, 111, 235), 20);
                    levelProgressBarPen.EndCapStyle = EndCapStyle.Round;
                    i.Draw(levelProgressBarPen, new SixLabors.ImageSharp.Drawing.Path(levelProgressBar));
                }

                var size24Font = Font.Outfit.CreateFont(24);
                var size16Font = Font.Outfit.CreateFont(16);

                var lightGray = Color.FromRgb(139, 148, 158);
                var dimGray = Color.FromRgb(110, 118, 127);

                // Draw the rank number.
                i.DrawText($"Rank #{rank}", size24Font, lightGray, new PointF(200, 109));

                // Draw the level.
                i.DrawText($"Level {experienceData.Level}", size16Font, dimGray, new PointF(200, 139));

                // Draw the current xp and the xp to the next level.
                i.DrawText(
                    new TextOptions(size24Font)
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Origin = new PointF(570f, 109f)
                    },
                    $"{experienceData.CurrentExperience} / {experienceData.ExperienceToNextLevel} xp",
                    lightGray
                );

                // Draw the total xp.
                i.DrawText(
                    new TextOptions(size16Font)
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Origin = new PointF(570f, 139f)
                    },
                    $"{experienceData.TotalExperience} xp total",
                    dimGray
                );
            });

            EllipsePolygon avatar = new(RankCardHeight / 2, RankCardHeight / 2, (RankCardHeight - RankCardPadding * 2) / 2);

            for (int i = 0; i < frameCount; i++)
            {
                using Image<Rgba32> frame = baseImage.Clone();
                
                ImageBrush avatarBrush = new(avatarImage.Frames.CloneFrame(i));
                frame.Mutate(i => i.Fill(avatarBrush, avatar));

                frame.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay
                    = avatarImage.Frames[i].Metadata.GetGifMetadata().FrameDelay;

                finalImage.Frames.AddFrame(frame.Frames.RootFrame);
            }

            // The first frame is empty since we didn't add anything to it.
            finalImage.Frames.RemoveFrame(0);

            // Also, we set the loop count to 0 since normally the gif only plays once.
            finalImage.Metadata.GetGifMetadata().RepeatCount = 0;

            IImageEncoder encoder = isAnimated ? gifEncoder : pngEncoder;
            var stream = new MemoryStream();

            await encoder.EncodeAsync(finalImage, stream, new CancellationTokenSource().Token);

            return stream;
        }
    }
}
