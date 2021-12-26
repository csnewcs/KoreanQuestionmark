namespace KoreanQuestionMark.Commands
{
    using Discord;
    using Discord.WebSocket;

    class Ping
    {
        public static async Task PingCommand(SocketSlashCommand command)
        {
            DateTime now = DateTime.Now;

            await command.RespondAsync($":ping_pong: 퐁! (메세지 인식까지 {Math.Round((now - command.CreatedAt).TotalMilliseconds)}ms)");
        }
    }
}