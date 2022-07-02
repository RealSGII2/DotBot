namespace DotBot.Shared.Models
{
    public record UserExperienceData
    {
        public uint TotalExperience { get; init; }

        public uint ExperienceToNextLevel
        {
            get => 100;
        }

        public uint CurrentExperience
        {
            get
                => TotalExperience % ExperienceToNextLevel;
        }

        public uint Level
        {
            get
                => (uint)Math.Round((decimal)(TotalExperience / ExperienceToNextLevel));
        }
    }
}
