using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void phonenumber_should_build()
    {
        var number = "+495321123456";

        var generator = new PhoneNumber(number);

        generator.ToString().Should().Be("tel:+495321123456");
    }
}