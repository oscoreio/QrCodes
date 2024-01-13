using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void url_should_build_http()
    {
        var url = "http://code-bude.net";

        var generator = new Url(url);

        generator.ToString().Should().Be("http://code-bude.net");
    }

    [Fact]
    public void url_should_build_https()
    {
        var url = "https://code-bude.net";

        var generator = new Url(url);

        generator.ToString().Should().Be("https://code-bude.net");
    }

    [Fact]
    public void url_should_add_http()
    {
        var url = "code-bude.net";

        var generator = new Url(url);

        generator.ToString().Should().Be("http://code-bude.net");
    }
}