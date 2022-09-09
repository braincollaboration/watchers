using FluentAssertions;
using Watchers.WebApi.Bots;

namespace Watchers.UnitTests;

[TestFixture]
public class CreateAnswerTests
{
    [Test]
    public void Test_CreateAnswer_ShouldReturs_EnterLinkWebPageHere()
    {
        var serviceBot = new ExternalService(null);

        var message = serviceBot.CreateAnswer("/start");

        message.Should().BeEquivalentTo("Enter link web page here!");
    }

    [Test]
    public void Test_CreateAnswer_ShouldBeCorrect_WhenMessageContains_http()
    {
        var serviceBot = new ExternalService(null);

        var message = serviceBot.CreateAnswer("http");

        message.Should().BeEquivalentTo("You should enter key word from website to help me. For example: out of stock");
    }
}