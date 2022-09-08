using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Watchers.WebApi.Bots;

public class XdDesignCheckBot : IHostedService
{
    private readonly ITelegramBotClient _bot;
    private readonly ILogger<XdDesignCheckBot> _logger;

    public XdDesignCheckBot(ILogger<XdDesignCheckBot> logger, ITelegramBotClient bot)
    {
        _logger = logger;
        _bot = bot;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions(),
            cancellationToken
        );

        return Task.CompletedTask;
    }
    
    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(JsonConvert.SerializeObject(exception));

        return Task.CompletedTask;
    }


    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(update));
        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message!.Text!.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!", cancellationToken: cancellationToken);
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!", cancellationToken: cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}