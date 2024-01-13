using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void bookmark_should_build()
    {
        var url = "http://code-bude.net";
        var title = "A nerd's blog";

        var generator = new Bookmark(url, title);

        generator.ToString().Should().Be("MEBKM:TITLE:A nerd's blog;URL:http\\://code-bude.net;;");
    }

    [Fact]
    public void bookmark_should_escape_input()
    {
        var url = "http://code-bude.net/fake,url.html";
        var title = "A nerd's blog: \\All;the;things\\";

        var generator = new Bookmark(url, title);

        generator.ToString().Should()
            .Be("MEBKM:TITLE:A nerd's blog\\: \\\\All\\;the\\;things\\\\;URL:http\\://code-bude.net/fake\\,url.html;;");
    }
}