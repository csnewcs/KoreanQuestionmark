
namespace KoreanQuestionMark.Commands
{
    using Discord;
    using Discord.WebSocket;

    using StdictAPI;

    class KoreanDictionary
    {
        public static async Task Search(SocketSlashCommand command, string stdictKey)
        {
            await command.RespondAsync("검색 중...");
            Stdict stdict = new Stdict(stdictKey);
            string q = command.Data.Options.First().Value.ToString();
            SimpleWord[] words = stdict.Search(q);

            if(words.Length == 0)
            {
                await command.ModifyOriginalResponseAsync(m => {m.Content = "검색 결과가 없습니다.";});
            }
            else
            {
                EmbedBuilder builder = new EmbedBuilder();
                foreach(SimpleWord word in words)
                {
                    int supNo = word.SupNo == 0 ? 1 : word.SupNo;
                    builder.AddField($"{supNo}. {word.Word}", $"「{word.Pos}」 {word.Definition}");
                }
                var time = DateTime.Now - command.CreatedAt;
                builder.WithFooter($"출처: 표준국어대사전, 처리 시간: {Math.Round(time.TotalSeconds, 2)}초");
                await command.ModifyOriginalResponseAsync(m => {m.Embed = builder.Build(); m.Content = "";});
            }
        }        
    }
}