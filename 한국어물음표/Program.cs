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
            _client.SlashCommandExecuted += SlashCommandExecuted;
            _client.Log += Logging.Logging.Log;
            _client.Ready += Ready;

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        private async Task Ready()
        {
            await MakeSlashCommand();

        }
        private async Task MakeSlashCommand()
        {
            var testCommand = new SlashCommandBuilder().WithName("테스트").WithDescription("테스트설명");
            if(config.ForTest)
            {
                for(int i = 0; i < config.TestGuilds.Length; i++)
                {
                    SocketGuild guild = _client.GetGuild(config.TestGuilds[i]);
                    await guild.CreateApplicationCommandAsync(testCommand.Build());
                }
            }
            else
            {
                await _client.CreateGlobalApplicationCommandAsync(testCommand.Build());
            }
        }
        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            if(config.ForTest && !config.Testers.Contains(command.User.Id))
            {
                Console.WriteLine("잘못된 사용자");
                return;
            } 
            if(command.CommandName == "테스트")
            {
                await command.RespondAsync("테스트!");
            }
            else await command.RespondAsync("테스트 실패");
        }
    }
}