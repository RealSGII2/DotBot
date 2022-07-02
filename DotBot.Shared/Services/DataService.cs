using DotBot.Shared.Models.Database;
using LiteDB;

namespace DotBot.Shared.Services
{
    public class DataService
    {
        private readonly string _databasePath;

        public DataService(string databasePath = @".\Data.db")
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets a guild from the database. If it doesn't exist, an entry will be created.
        /// </summary>
        /// <param name="guildId">The guild's ID</param>
        /// <returns>The guild's database entry</returns>
        public DatabaseGuild GetGuild(ulong guildId)
        {
            using LiteDatabase database = new(_databasePath);
            var column = database.GetCollection<DatabaseGuild>("guilds");
            column.EnsureIndex(g => g.Id);

            var result = column.FindOne(g => g.Id == guildId);

            if (result is null)
            {
                result = new DatabaseGuild()
                {
                    ExperienceGainCooldown = 120,
                    Id = guildId,
                    MaxExperienceGain = 25,
                    MinExperienceGain = 15,
                    UserExperience = new Dictionary<ulong, uint?>()
                };

                column.Insert(result);
            }

            result._dataService = this;

            return result;
        }

        public void SaveGuild(DatabaseGuild guild)
        {
            using LiteDatabase database = new(_databasePath);
            var column = database.GetCollection<DatabaseGuild>("guilds");
            Console.WriteLine(column.Update(guild));
        }
    }
}
