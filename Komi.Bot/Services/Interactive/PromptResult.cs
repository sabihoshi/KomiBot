using System;
using Discord.Commands;

namespace Komi.Bot.Services.Interactive
{
    public class PromptResult
    {
        public PromptResult(string question, object? userResponse)
        {
            Question = question;
            UserResponse = userResponse;
        }

        public string Question { get; }

        public object? UserResponse { get; }

        public static implicit operator string?(PromptResult? result) => result?.UserResponse?.ToString();

        public T As<T>()
        {
            if (UserResponse is TypeReaderResult result)
                return (T)result.BestMatch;

            return (T)UserResponse!;
        }

        public T As<T>(Func<TypeReaderResult, T> selector) => selector((TypeReaderResult)UserResponse!);
    }
}