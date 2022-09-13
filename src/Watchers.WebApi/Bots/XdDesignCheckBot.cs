using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Watchers.WebApi.Bots;

public class XdDesignCheckBot : IHostedService
{
    private readonly ExternalService _externalService;

    public XdDesignCheckBot(ExternalService externalService)
    {
       
        _externalService = externalService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _externalService.ReceiveMessage();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}