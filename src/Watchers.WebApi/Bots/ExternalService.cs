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

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            if (update.Message != null && update.Type == UpdateType.Message && update.Message.Text != null)
            {
                try
                {
                 var htmlPage = await GetHtmlPage(update.Message.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        private async Task<string> GetHtmlPage(string customerRequest) 
        {
            using HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, customerRequest);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:70.0) Gecko/20100101 Firefox/70.0");
            HttpResponseMessage response = await client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return responseBody;
        }
    }
}
