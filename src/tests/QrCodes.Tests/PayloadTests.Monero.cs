using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void monero_generator_can_generate_payload_simple()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var generator = new MoneroTransaction(address);

        generator
            .ToString()
            .Should().Be(
                "monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em");
    }

    [Fact]
    public void monero_generator_can_generate_payload_first_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = 1.3f;
        var generator = new MoneroTransaction(address, amount);

        generator
            .ToString()
            .Should().Be(
                "monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?tx_amount=1.3");
    }

    [Fact]
    public void monero_generator_can_generate_payload_named_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var recipient = "Raffael Herrmann";
        var generator = new MoneroTransaction(address, recipientName: recipient);

        generator
            .ToString()
            .Should().Be(
                "monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?recipient_name=Raffael%20Herrmann");
    }

    [Fact]
    public void monero_generator_can_generate_payload_full_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = 1.3f;
        var paymentId = "1234567890123456789012345678901234567890123456789012345678901234";
        var recipient = "Raffael Herrmann";
        var description = "Monero transaction via QrCoder.NET.";
        var generator = new MoneroTransaction(address, amount, paymentId, recipient, description);

        generator
            .ToString()
            .Should().Be(
                "monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?tx_payment_id=1234567890123456789012345678901234567890123456789012345678901234&recipient_name=Raffael%20Herrmann&tx_amount=1.3&tx_description=Monero%20transaction%20via%20QrCoder.NET.");
    }

    [Fact]
    public void monero_generator_should_throw_wrong_amount_exception()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = -1f;

        var exception = Record.Exception(() => new MoneroTransaction(address, amount));

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        exception.Message.Should().Be("Value of 'txAmount' must be greater than 0.");
    }

    [Fact]
    public void monero_generator_should_throw_no_address_exception()
    {
        var address = "";

        var exception = Record.Exception(() => new MoneroTransaction(address));

        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
        exception.Message.Should().Be("The address is mandatory and has to be set.");
    }
}