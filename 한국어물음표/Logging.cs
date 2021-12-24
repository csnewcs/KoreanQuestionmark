namespace KoreanQuestionMark.Logging
{
    using Discord;

    public static class Logging
    {
        public static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}