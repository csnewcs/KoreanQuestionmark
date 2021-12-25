
namespace KoreanQuestionMark.Commands
{
    using Discord;
    using Discord.WebSocket;

    using StdictAPI;

    class KoreanDictionary
    {
        static Stdict _stdict;
        public static void NewStdict(string stdictKey)
        {
            _stdict = new Stdict(stdictKey);
        }
        public static async Task Search(SocketSlashCommand command)
        {
            await command.RespondAsync("검색 중...");
            string q = command.Data.Options.First().Value.ToString();
            SimpleWord[] words = _stdict.Search(q);

            if(words.Length == 0)
            {
                await command.ModifyOriginalResponseAsync(m => {m.Content = "검색 결과가 없습니다.";});
            }
            else
            {
                EmbedBuilder builder = new EmbedBuilder();
                SelectMenuBuilder selectMenuBuilder = new SelectMenuBuilder().WithPlaceholder("자세히 보기").WithCustomId("MoreView");

                foreach(SimpleWord word in words)
                {
                    int supNo = word.SupNo == 0 ? 1 : word.SupNo;
                    builder.AddField($"{supNo}. {word.Word}", $"「{word.Pos}」 {word.Definition}");
                    selectMenuBuilder.AddOption(new SelectMenuOptionBuilder($"{supNo}. {word.Word}", word.TargetCode.ToString(), word.Definition.Substring(0, word.Definition.Length > 100 ? 100 : word.Definition.Length)));
                }
                var componentBuilder = new ComponentBuilder().WithSelectMenu(selectMenuBuilder);
                var time = DateTime.Now - command.CreatedAt;
                builder.WithFooter($"출처: 표준국어대사전, 처리 시간: {Math.Round(time.TotalSeconds, 2)}초");
                await command.ModifyOriginalResponseAsync(m => {m.Embed = builder.Build(); m.Content = ""; m.Components = componentBuilder.Build(); });
            }
        }        
        public static async Task MoreSearch(SocketMessageComponent component)
        {
            var targetCode = component.Data.Values.First();
            await component.UpdateAsync(m => {m.Content = "검색 중...";  m.Components = null;});
            DetailWord[] words = _stdict.MoreSearch(int.Parse(targetCode));
            if(words.Length == 0)
            {
                await component.ModifyOriginalResponseAsync(m => {m.Content = "표준국어대사전 오류. 응답이 없습니다.";});
                return;
            }
            EmbedBuilder builder = new EmbedBuilder();
            for(int i = 0; i < words.Length; i++)
            {
                string pronunciation = words[i].Pronunciation == null ? "" : $"발음: [{words[i].Pronunciation}]\n";
                string exampleAdd = words[i].Examples.Length == 0 ? "```" : $"\n\n예문\n\t-{string.Join("\n\t-", words[i].Examples)}```";
                builder.AddField($"{i+1}. {words[i].Word}", $"```{pronunciation}뜻: {words[i].Definition}" + exampleAdd);
            }
            await component.ModifyOriginalResponseAsync(m => {m.Embed = builder.Build(); m.Content = "";});
        }
    }
}