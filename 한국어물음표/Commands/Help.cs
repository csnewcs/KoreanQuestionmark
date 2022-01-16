namespace KoreanQuestionMark.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;

    class Help
    {
        public static async Task HelpCommand(SocketSlashCommand command)
        {
            EmbedBuilder builder = new EmbedBuilder()
            .WithTitle("한국어물음표 도움말")
            .WithThumbnailUrl("https://cdn.discordapp.com/attachments/923739801516134461/924151983181529108/b85f58a9aeb0270d.png")
            .AddField("도움말", "```지금 보이는 이 도움말을 띄워요.```")
            .AddField("핑", "```핑을 날려 봇이 살아있는가를 보여줘요. 겸사겸사 지연 시간도 확인해요.```")
            .AddField("국어사전", "```공지에 쓰여있는 저 단어가 뭔지 모르겠다고요? 괜찮아요. 이걸로 검색해 보세요. 품사를 지정해서 검색할 수도 있어요. (결과는 본인만 볼 수 있어요.)```")
            .AddField("맞춤법검사", "```지금 치고 있는 문장이 이 봇의 개발자 같은 불편러들이 보기에도 괜찮은지 모르겠다고요? 그럴 수 있어요. 이걸 사용해 보세요. (결과는 본인만 볼 수 있어요.)```")
            .AddField("라이선스", "```이 봇이 혼자 만들어진건 아니죠. 이 봇에 사용된 오픈소스 프로젝트들을 알려줘요.```")
            .WithColor(new Color(0x6fa5b2))
            .WithFooter("csnewcs#8817 개발");
            await command.RespondAsync(embed: builder.Build());
        }
    }
}