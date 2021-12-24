namespace KoreanQuestionMark.Commands
{
    using Discord;
    using Discord.WebSocket;
    class GrammerCheck
    {
        public static async Task Check(SocketSlashCommand command, string sentenses)
        {
            await command.RespondAsync("기능을 만드는 중이에요!");
        }
    }
}