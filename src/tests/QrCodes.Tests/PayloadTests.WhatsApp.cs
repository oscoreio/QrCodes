using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void whatsapp_generator_can_generate_payload_simple()
    {
        var number = "491601234567";
        var msg = "This is a sample message with Umlauts: Ä,ö, ü and ß.";
        var generator = new WhatsAppMessage(number, msg);

        generator
            .ToString()
            .Should().Be(
                "https://wa.me/491601234567?text=This%20is%20a%20sample%20message%20with%20Umlauts%3A%20%C3%84%2C%C3%B6%2C%20%C3%BC%20and%20%C3%9F.");
    }

    [Fact]
    public void whatsapp_should_add_unused_params()
    {
        var msg = "This is a sample message with Umlauts: Ä,ö, ü and ß.";
        var generator = new WhatsAppMessage(msg);

        generator
            .ToString()
            .Should().Be(
                "https://wa.me/?text=This%20is%20a%20sample%20message%20with%20Umlauts%3A%20%C3%84%2C%C3%B6%2C%20%C3%BC%20and%20%C3%9F.");
    }

    [Fact]
    public void whatsapp_should_cleanup_phonenumber_1()
    {
        var number = "+49(160)1234567";
        var msg = "This is a sample message with Umlauts: Ä,ö, ü and ß.";
        var generator = new WhatsAppMessage(number, msg);

        generator
            .ToString()
            .Should().Be(
                "https://wa.me/491601234567?text=This%20is%20a%20sample%20message%20with%20Umlauts%3A%20%C3%84%2C%C3%B6%2C%20%C3%BC%20and%20%C3%9F.");
    }

    [Fact]
    public void whatsapp_should_cleanup_phonenumber_2()
    {
        var number = "0049-160-1234 567";
        var msg = "This is a sample message with Umlauts: Ä,ö, ü and ß.";
        var generator = new WhatsAppMessage(number, msg);

        generator
            .ToString()
            .Should().Be(
                "https://wa.me/491601234567?text=This%20is%20a%20sample%20message%20with%20Umlauts%3A%20%C3%84%2C%C3%B6%2C%20%C3%BC%20and%20%C3%9F.");
    }
}