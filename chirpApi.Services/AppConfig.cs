using Microsoft.Extensions.Configuration;

namespace chirpApi
{
    internal class AppConfig
    {
        public static IConfiguration Configuration { get; set; }

        static AppConfig()
        {
            if (Configuration == null)
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
            }
        }
        public static string? GetConnectionString() => Configuration.GetConnectionString("Default");
    }
}
