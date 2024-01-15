using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void shadowsocks_generator_can_generate_payload()
    {
        new ShadowSocksConfig(
                hostname: "192.168.2.5",
                port: 1,
                password: "s3cr3t",
                method: ShadowSocksConfig.Method.Rc4Md5)
            .ToString()
            .Should().Be("ss://cmM0LW1kNTpzM2NyM3RAMTkyLjE2OC4yLjU6MQ==");
    }

    [Fact]
    public void shadowsocks_generator_can_generate_payload_with_tag()
    {
        new ShadowSocksConfig(
                hostname: "192.168.2.5",
                port: 65535,
                password: "s3cr3t",
                method: ShadowSocksConfig.Method.Rc4Md5,
                tag: "server42")
            .ToString()
            .Should().Be("ss://cmM0LW1kNTpzM2NyM3RAMTkyLjE2OC4yLjU6NjU1MzU=#server42");
    }

    [Fact]
    public void shadowsocks_generator_should_throw_portrange_low_exception()
    {
        var host = "192.168.2.5";
        var port = 0;
        var password = "s3cr3t";
        var method = ShadowSocksConfig.Method.Rc4Md5;

        var exception = Record.Exception(() => new ShadowSocksConfig(host, port, password, method));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Value of 'port' must be within 0 and 65535.");
    }

    [Fact]
    public void shadowsocks_generator_should_throw_portrange_high_exception()
    {
        var host = "192.168.2.5";
        var port = 65536;
        var password = "s3cr3t";
        var method = ShadowSocksConfig.Method.Rc4Md5;

        var exception = Record.Exception(() => new ShadowSocksConfig(host, port, password, method));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Value of 'port' must be within 0 and 65535.");
    }

    [Fact]
    public void shadowsocks_generator_can_generate_payload_with_plugin()
    {
        var host = "192.168.100.1";
        var port = 8888;
        var password = "test";
        var method = ShadowSocksConfig.Method.BfCfb;
        var plugin = "obfs-local";
        var pluginOption = "obfs=http;obfs-host=google.com";
        var generator = new ShadowSocksConfig(host, port, password, method, plugin, pluginOption);

        generator
            .ToString()
            .Should().Be(
                "ss://YmYtY2ZiOnRlc3Q@192.168.100.1:8888/?plugin=obfs-local%3bobfs%3dhttp%3bobfs-host%3dgoogle.com");
    }
}