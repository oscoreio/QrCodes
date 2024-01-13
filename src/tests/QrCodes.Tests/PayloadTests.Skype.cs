using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void skype_should_build()
    {
        var username = "johndoe123";

        var generator = new SkypeCall(username);

        generator.ToString().Should().Be("skype:johndoe123?call");
    }
}