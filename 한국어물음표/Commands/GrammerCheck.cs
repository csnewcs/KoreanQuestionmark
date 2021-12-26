namespace KoreanQuestionMark.Commands
{
    using Discord;
    using Discord.WebSocket;
    using Newtonsoft.Json.Linq;

    using GrammerCheckerAPI;
    class GrammerCheck
    {
        public static async Task Check(SocketSlashCommand command)
        {
            string source = command.Data.Options.First().Value.ToString();
            if(source.Length > 1000) 
            {
                await command.RespondAsync("너무 길어요. 1000자 이하만 검사해 주세요.");
                return;    
            }
            string result = "";
            await command.RespondAsync("검사 중...", ephemeral: true);
            int corrected = NaverGrammer.CheckGrammer(source, out result);
            EmbedBuilder embed = new EmbedBuilder()
            .AddField("입력", $"```{source}```")
            .AddField("결과", $"```{result}```")
            .WithFooter($"출처: 네이버 맞춤법검사기, {corrected}개의 부분 맞춤법 의심, 교정 전 {source.Length}자, 교정 후 {result.Length}자");
            await command.ModifyOriginalResponseAsync(m => {m.Embed = embed.Build(); m.Content = "";});
        }
    }
}