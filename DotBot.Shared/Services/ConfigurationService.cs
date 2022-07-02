using Microsoft.Extensions.Configuration;

namespace DotBot.Shared.Services
{
    public class ConfigurationService
    {
        public IConfigurationRoot Configuration { get; }

        public ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("props.yml");

            Configuration = builder.Build();
        }
    }
}
