using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json.Linq;

using KoreanQuestionMark.BotConfig;
using KoreanQuestionMark.Logging;
using KoreanQuestionMark.Commands;

using StdictAPI;

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
            _client.SelectMenuExecuted += SelectMenuExecuted;

            KoreanDictionary.NewStdict(config.StdictKey);

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
            var posSelectOption = new SlashCommandOptionBuilder().WithRequired(false).WithName("품사선택").WithDescription("검색할 단어의 품사를 선택하세요.").WithType(ApplicationCommandOptionType.Integer).AddChoice("모두", 0).AddChoice("명사", 1).AddChoice("대명사", 2).AddChoice("수사", 3).AddChoice("조사", 4).AddChoice("동사", 5).AddChoice("형용사", 6).AddChoice("관형사", 7).AddChoice("부사", 8).AddChoice("감탄사", 9).AddChoice("접사", 10).AddChoice("의존 명사", 11).AddChoice("보조 동사", 12).AddChoice("보조 형용사", 13).AddChoice("어미", 14).AddChoice("품사 없음", 15);

            var pingCommand = new SlashCommandBuilder().WithName("핑").WithDescription("핑을 날려요. 지연시간도 확인해 보세요.");
            var helpCommand = new SlashCommandBuilder().WithName("도움말").WithDescription("이 봇을 어떻게 쓰는 지 모르겠다고요? 이거 부터 한 번 써보세요!");
            var koreanDictionarySearch = new SlashCommandBuilder().WithName("국어사전").WithDescription("국어사전에서 단어를 검색해요!").AddOption("검색어", ApplicationCommandOptionType.String, "무엇을 검색할 것인지 알려주세요", true).AddOption(posSelectOption); //.AddOption(new SlashCommandOptionBuilder().WithName("9품사").WithDescription("검색할 단어의 품사를 선택하세요").WithRequired(false).AddChoice("모두", 0).AddChoice("명사", 1).AddChoice("대명사", 2).AddChoice("수사", 3).AddChoice("동사", 4).AddChoice("형용사", 5).AddChoice("관형사", 6).AddChoice("부사", 7).AddChoice("조사", 8).AddChoice("감탄사", 9))     9품사 검색기능은 나중에\
            var grammerCheck = new SlashCommandBuilder().WithName("맞춤법검사").WithDescription("맞춤법, 문법이 헷갈리시면 사용해주세요!").AddOption("문장", ApplicationCommandOptionType.String, "어떤 문장(들)을 검사할건지 적어주세요", true);
            var licenses = new SlashCommandBuilder().WithName("라이선스").WithDescription("이 봇이 사용하는 오픈소스 라이선스들을 알려줘요.");
            if(config.onlyTestGuilds)
            {
                for(int i = 0; i < config.TestGuilds.Length; i++)
                {
                    SocketGuild guild = _client.GetGuild(config.TestGuilds[i]);
                    // await guild.DeleteApplicationCommandsAsync();
                    await guild.CreateApplicationCommandAsync(helpCommand.Build());
                    await guild.CreateApplicationCommandAsync(pingCommand.Build());
                    await guild.CreateApplicationCommandAsync(koreanDictionarySearch.Build());
                    await guild.CreateApplicationCommandAsync(grammerCheck.Build());
                    await guild.CreateApplicationCommandAsync(licenses.Build());
                }
            }
            else
            {
                await _client.CreateGlobalApplicationCommandAsync(helpCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(pingCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(koreanDictionarySearch.Build());
                await _client.CreateGlobalApplicationCommandAsync(grammerCheck.Build());
                await _client.CreateGlobalApplicationCommandAsync(licenses.Build());
            }
        }
        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            if(config.onlyTesteUsers && !config.Testers.Contains(command.User.Id)) //테스트 상황에서 테스터 아닌 사람 거르기
            {
                await command.RespondAsync("지금 이 봇은 테스트 중이라 일반 사용자분들은 사용하실 수 없어요.");
                return;
            } 

            switch (command.CommandName)
            {
                case "도움말":
                    await Help.HelpCommand(command);
                    break;
                case "핑":
                    await Ping.PingCommand(command);
                    break;
                case "국어사전":
                    await KoreanDictionary.Search(command);
                    break;
                case "맞춤법검사":
                    await GrammerCheck.Check(command);
                    break;
                case "라이선스":
                    await Licenses.ViewLicenses(command);
                    break;                
            }
        }
        private async Task SelectMenuExecuted(SocketMessageComponent component)
        {
            switch(component.Data.CustomId)
            {
                case "MoreView":
                    await KoreanDictionary.MoreSearch(component);
                    break;
            }
        }
    }
}