using FluentAssertions;
using Moq;
using Telegram.Bot;
using Watchers.WebApi.Bots;

namespace Watchers.UnitTests;

public class ExternalServiceReceiveTests
{
    private Mock<ITelegramBotClient> _telegramBotClientMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _telegramBotClientMock = new Mock<ITelegramBotClient>();
    }
    
    [Test]
    public async Task ReceiveSuccess()
    {
        var serviceBot = new ExternalService(_telegramBotClientMock.Object);
        const string message = "meesage";

        var handler = async() => await serviceBot.ReceiveMessage();

        await handler.Should().NotThrowAsync();
    } 
}