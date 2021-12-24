using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json.Linq;

using KoreanQuestionMark.BotConfig;
using KoreanQuestionMark.Logging;
using KoreanQuestionMark.Commands;

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
            var koreanDictionarySearch = new SlashCommandBuilder().WithName("국어사전").WithDescription("국어사전에서 단어를 검색해요!").AddOption("검색어", ApplicationCommandOptionType.String, "무엇을 검색할 것인지 알려주세요", true); //.AddOption(new SlashCommandOptionBuilder().WithName("9품사").WithDescription("검색할 단어의 품사를 선택하세요").WithRequired(false).AddChoice("모두", 0).AddChoice("명사", 1).AddChoice("대명사", 2).AddChoice("수사", 3).AddChoice("동사", 4).AddChoice("형용사", 5).AddChoice("관형사", 6).AddChoice("부사", 7).AddChoice("조사", 8).AddChoice("감탄사", 9))     9품사 검색기능은 나중에\
            var grammerCheck = new SlashCommandBuilder().WithName("문법검사").WithDescription("문법, 맞춤법이 헷갈리시면 사용해주세요!").AddOption("문장", ApplicationCommandOptionType.String, "어떤 문장(들)을 검사할건지 적어주세요", true);
            if(config.ForTest)
            {
                for(int i = 0; i < config.TestGuilds.Length; i++)
                {
                    SocketGuild guild = _client.GetGuild(config.TestGuilds[i]);
                    await guild.CreateApplicationCommandAsync(testCommand.Build());
                    await guild.CreateApplicationCommandAsync(koreanDictionarySearch.Build());
                    await guild.CreateApplicationCommandAsync(grammerCheck.Build());
                }
            }
            else
            {
                await _client.CreateGlobalApplicationCommandAsync(testCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(koreanDictionarySearch.Build());
                await _client.CreateGlobalApplicationCommandAsync(grammerCheck.Build());
            }
        }
        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            if(config.ForTest && !config.Testers.Contains(command.User.Id)) //테스트 상황에서 테스터 아닌 사람 거르기
            {
                Console.WriteLine("잘못된 사용자");
                return;
            } 

            switch (command.CommandName)
            {
                case "테스트":
                    await command.RespondAsync("테스트 성공");
                    break;
                case "국어사전":
                    await KoreanDictionary.Search(command, config.StdictKey);
                    break;
                
            }
        }
    }
}