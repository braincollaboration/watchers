using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Watchers.WebApi.Bots
{
    public class ExternalService
    {
        private readonly ITelegramBotClient _bot;

        public ExternalService(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task ReceiveMessage()
        {
            await _bot.ReceiveAsync(HandleUpdateAsync,
                (_, _, _) => Task.CompletedTask,
                new ReceiverOptions());
        }

        public async Task Send(string charId, string message)
        {
            await _bot.SendChatActionAsync(new ChatId("432228649"), ChatAction.Typing);
            await _bot.SendTextMessageAsync(new ChatId(charId), message);
        }


        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var answer = CreateAnswer(update.Message.Text.ToLower());

                await botClient.SendTextMessageAsync(update.Message.Chat, answer, cancellationToken: cancellationToken);
            }
        }

        public string CreateAnswer(string message) 
        {
            if (message == "/start") 
                return "Enter link web page here!";
            
            if (message.Contains("http"))
                return "You should enter key word from website to help me. For example: out of stock";

            return "";
        }


    }
}
