namespace Komi.Bot.Services.Interactive
{
    public class PromptResult
    {
        public PromptResult(string question, string userResponse)
        {
            Question = question;
            UserResponse = userResponse;
        }

        public string Question { get; }

        public string UserResponse { get; }
    }
}