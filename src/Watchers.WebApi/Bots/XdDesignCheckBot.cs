using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Watchers.WebApi.Bots;

public class XdDesignCheckBot : BackgroundService
{
    private readonly ExternalService _externalService;

    public XdDesignCheckBot(ExternalService externalService)
    {
       
        _externalService = externalService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _externalService.ReceiveMessage(stoppingToken);
    }
}