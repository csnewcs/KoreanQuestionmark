namespace KoreanQuestionMark.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.WebSocket;

    class Licenses
    {
        public static async Task ViewLicenses(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithTitle("한국어물음표에 사용된 오픈소스 프로젝트들")
                .AddField(".Net 6.0", "MIT License / https://github.com/dotnet/sdk")
                .AddField("Discord.Net 3.0.0", "MIT License / https://github.com/discord-net/Discord.Net")
                .AddField("Newtonsoft.Json 13.0.1", "MIT License / https://github.com/JamesNK/Newtonsoft.Json");
            await command.RespondAsync(embed: builder.Build());
        }
    }
}