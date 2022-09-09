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

                var message = update.Message;
                if (message!.Text!.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Enter link web page here!", cancellationToken: cancellationToken);
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!", cancellationToken: cancellationToken);
            }
        }

        private string CreateAnswer(string message) 
        {
            if (message == "/start") 
                return "Enter link web page here!";
            
            if (message.Contains("http")) 
                return " Enter key word";

            return "";
            
        }


    }
}
