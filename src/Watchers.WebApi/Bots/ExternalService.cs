using RestSharp;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace Watchers.WebApi.Bots
{
    public class ExternalService
    {
        private HashSet<string> _searchKeys = new HashSet<string> {"Нет в наличии", "нет в наличии", "out of stock" };
        private Dictionary<string, HashSet<PageData>> _users;
        private readonly static string _pathUsersData = @"D:\MyFiles\telegrammBot\watchers\src\Watchers.WebApi\bin\Debug\net6.0";


        private static class BotMessages
        {
            public static string messageAddedProduct = "Товар занесен в список опрашиваемых";
            public static string messageNotFound = "Товар занесен в список опрашиваемых";
            public static string messageWebPageAlreadyExists = "Товар уже занесен в список опрашиваемых";
        }

        private readonly ITelegramBotClient _bot;

        public ExternalService(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task ReceiveMessage()
        {
            await _bot.ReceiveAsync(HandleUpdateAsync,
                                (_, _, _) => Task.CompletedTask,
                                new ReceiverOptions()).ContinueWith(UpdateJsonUserFile);

        }

        private async void UpdateJsonUserFile(Task task) 
        {
            await File.WriteAllTextAsync(_pathUsersData, JsonSerializer.Serialize(_users));
        }



        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _users = JsonSerializer.Deserialize<Dictionary<string, HashSet<PageData>>>(await File.ReadAllTextAsync(_pathUsersData)) ?? new Dictionary<string, HashSet<PageData>>();
            if (update.Type == UpdateType.Message && update.Message.Text != null)
            {
                //await botClient.SendTextMessageAsync(update.Message.Chat.Id, update.Message.Chat.Id.ToString(), cancellationToken: cancellationToken);

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, update.Message.Text);
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:70.0) Gecko/20100101 Firefox/70.0");
                
                    //HttpResponseMessage response = await client.GetAsync(update.Message.Text, HttpCompletionOption.ResponseHeadersRead);
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    foreach(string key in _searchKeys) 
                    {
                        if (responseBody.Contains(key)) 
                        {   
                            if (_users.ContainsKey(update.Message.Chat.Id.ToString()))
                            {
                                if (_users[update.Message.Chat.Id.ToString()].Contains(new PageData(update.Message.Text, key)))
                                {
                                    await botClient.SendTextMessageAsync(update.Message.Chat, BotMessages.messageWebPageAlreadyExists, cancellationToken: cancellationToken);
                                    return;
                                }
                                _users[update.Message.Chat.Id.ToString()].Add(new PageData(update.Message.Text, key));
                                await botClient.SendTextMessageAsync(update.Message.Chat, BotMessages.messageAddedProduct, cancellationToken: cancellationToken);
                                return;
                            }
                            _users.Add(update.Message.Chat.Id.ToString(), new HashSet<PageData> {new PageData(update.Message.Text, key)});
                            
                        }                
                    }
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, BotMessages.messageNotFound, cancellationToken: cancellationToken);
                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public string CreateAnswer(string message) 
        {
            if (message == "/start") 
                return "Enter the link to the web page here";

            if (message.Contains("http"))
            {
                //var page = GetWebPageCode();

                //return  page.AsyncState as string;



                //string responseBody = await response.Content.ReadAsStringAsync();
                //return new Task() { AsyncState =  responseBody };
                //return "You should enter the key word from website to help me. For example: out of stock";
            }

            return "I don't understand you";
        }

        //private async Task GetWebPageCode()
        //{
        //    //HttpClient client = new HttpClient();
        //    //HttpResponseMessage response;
        //    //try
        //    //{
        //    //    response = await client.GetAsync("http://www.contoso.com/");
        //    //    response.EnsureSuccessStatusCode();
        //    //}
        //    //catch 
        //    //{
        //    //    await response.Content.ReadAsStringAsync();
        //    //}
            



        //    //string responseBody = await response.Content.ReadAsStringAsync();
        //    //return new Task() { AsyncState =  responseBody };
        //}
    }
}
