using DotBot.Shared.Services;
using LiteDB;

namespace DotBot.Shared.Models.Database
{
    public class DatabaseGuild
    {
        [BsonIgnore]
        public DataService _dataService;

        public ulong Id { get; set; }

        public uint MinExperienceGain { get; set; }

        public uint MaxExperienceGain { get; set; }

        /// <summary>
        /// The amount in seconds in which you can't gain experience after sending a message.
        /// </summary>
        public uint ExperienceGainCooldown { get; set; }

        /// <summary>
        /// The amount of experience members have. <userId, userExperience>
        /// </summary>
        public Dictionary<ulong, uint?> UserExperience { get; set; } = new();

        /// <summary>
        /// DO NOT USE THIS. This remains here for database (de)serialisation,
        /// </summary>
        public DatabaseGuild() { }

        public void Save()
            => _dataService.SaveGuild(this);
        
    }
}
