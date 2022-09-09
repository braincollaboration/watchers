using FluentAssertions;
using Moq;
using Telegram.Bot;
using Watchers.WebApi.Bots;

namespace Watchers.UnitTests;

public class ExternalServiceSendTests
{
    private Mock<ITelegramBotClient> _telegramBotClientMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _telegramBotClientMock = new Mock<ITelegramBotClient>();
    }
    
    [Test]
    public async Task SendSuccess()
    {
        var serviceBot = new ExternalService(_telegramBotClientMock.Object);
        const string message = "meesage";

        var handler = async() => await serviceBot.Send("123123", message);

        await handler.Should().NotThrowAsync();
    } 
}