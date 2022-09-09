using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Watchers.WebApi.Bots
{
    public class ExternalService
    {
        private readonly ITelegramBotClient _bot;
        private string _keyWord;

        public ExternalService(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task Get()
        {
            await _bot.ReceiveAsync(HandleUpdateAsync,
                                    HandleErrorAsync,
                                    new ReceiverOptions());
        }

        public async Task Send()
        {
            await _bot.SendChatActionAsync(new ChatId("432228649"), ChatAction.Typing);
            await _bot.SendTextMessageAsync(new ChatId("432228649"), "");


        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия


            return Task.CompletedTask;
        }


        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
       
            if (update.Type == UpdateType.Message)
            {
                var answer = CreateAnswer(update.Message.Text.ToLower());

                await botClient.SendTextMessageAsync(update.Message.Chat, answer, cancellationToken: cancellationToken);

            }
        }

        private string CreateAnswer(string message) 
        {
            if (message == "/start") 
                return "Enter link web page here!";
            
            if (message.Contains("http"))
                return "You should enter key word from website to help me. For example: out of stock";

            return "";
        }


    }
}
