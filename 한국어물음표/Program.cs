using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json.Linq;

using KoreanQuestionMark.BotConfig;
using KoreanQuestionMark.Logging;

namespace KoreanQuestionMark
{
    class Program
    {
        Config config;
        DiscordSocketClient _client;
        
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            try
            {
                config = Config.Load("BotConfig.json");
            }
            catch
            {
                config = Config.Make();
            }
            _client = new DiscordSocketClient();
            _client.Log += Logging.Logging.Log;
            
        }
    }
}