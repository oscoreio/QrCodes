using System.Globalization;
using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void bitcoin_address_generator_can_generate_address()
    {
        new BitcoinAddress(
                address: "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W",
                amount: .123,
                label: "Some Label to Encode",
                message: "Some Message to Encode")
            .ToString()
            .Should().Be(
                "bitcoin:175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W?label=Some%20Label%20to%20Encode&message=Some%20Message%20to%20Encode&amount=.123");
    }

    [Fact]
    public void bitcoin_address_generator_should_skip_missing_label()
    {
        var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
        var amount = .123;
        var message = "Some Message to Encode";


        var generator = new BitcoinAddress(address, amount, null, message);

        generator
            .ToString()
            .Should().NotContain("label");
    }

    [Fact]
    public void bitcoin_address_generator_should_skip_missing_message()
    {
        var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
        var amount = .123;


        var generator = new BitcoinAddress(address, amount);

        generator
            .ToString()
            .Should().NotContain("message");
    }

    [Fact]
    public void bitcoin_address_generator_should_round_to_satoshi()
    {
        var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
        var amount = .123456789;


        var generator = new BitcoinAddress(address, amount);

        generator
            .ToString()
            .Should().Contain("amount=.12345679");
    }

    [Fact]
    public void bitcoin_address_generator_disregards_current_culture()
    {
#if NETCOREAPP1_1
            var currentCulture = CultureInfo.DefaultThreadCurrentCulture;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
#else
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
#endif

        var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
        var amount = .123;


        var generator = new BitcoinAddress(address, amount);

        generator
            .ToString()
            .Should().Be("bitcoin:175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W?amount=.123");

#if NETCOREAPP1_1
            CultureInfo.DefaultThreadCurrentCulture = currentCulture;
#else
        Thread.CurrentThread.CurrentCulture = currentCulture;
#endif
    }

    [Fact]
    public void bitcoincash_address_generator_can_generate_address()
    {
        var address = "qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890";
        var amount = .123;
        var label = "Some Label to Encode";
        var message = "Some Message to Encode";

        var generator = new BitcoinCashAddress(address, amount, label, message);

        generator
            .ToString()
            .Should().Be(
                "bitcoincash:qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890?label=Some%20Label%20to%20Encode&message=Some%20Message%20to%20Encode&amount=.123");
    }

    [Fact]
    public void bitcoincash_address_generator_should_skip_missing_label()
    {
        var address = "qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890";
        var amount = .123;
        var message = "Some Message to Encode";


        var generator = new BitcoinCashAddress(address, amount, null, message);

        generator
            .ToString()
            .Should().NotContain("label");
    }

    [Fact]
    public void bitcoincash_address_generator_should_skip_missing_message()
    {
        var address = "qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890";
        var amount = .123;


        var generator = new BitcoinCashAddress(address, amount);

        generator
            .ToString()
            .Should().NotContain("message");
    }

    [Fact]
    public void bitcoincash_address_generator_should_round_to_satoshi()
    {
        var address = "qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890";
        var amount = .123456789;


        var generator = new BitcoinCashAddress(address, amount);

        generator
            .ToString()
            .Should().Contain("amount=.12345679");
    }

    [Fact]
    public void bitcoincash_address_generator_disregards_current_culture()
    {
#if NETCOREAPP1_1
            var currentCulture = CultureInfo.DefaultThreadCurrentCulture;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
#else
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
#endif

        var address = "qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890";
        var amount = .123;


        var generator = new BitcoinCashAddress(address, amount);

        generator
            .ToString()
            .Should().Be("bitcoincash:qqtlfk37qyey50f4wfuhc7jw85zsdp8s2swffjk890?amount=.123");

#if NETCOREAPP1_1
            CultureInfo.DefaultThreadCurrentCulture = currentCulture;
#else
        Thread.CurrentThread.CurrentCulture = currentCulture;
#endif
    }

    [Fact]
    public void litecoin_address_generator_can_generate_address()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;
        var label = "Some Label to Encode";
        var message = "Some Message to Encode";

        var generator = new LitecoinAddress(address, amount, label, message);

        generator
            .ToString()
            .Should().Be(
                "litecoin:LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54?label=Some%20Label%20to%20Encode&message=Some%20Message%20to%20Encode&amount=.123");
    }

    [Fact]
    public void litecoin_address_generator_should_skip_missing_label()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;
        var message = "Some Message to Encode";


        var generator = new LitecoinAddress(address, amount, null, message);

        generator
            .ToString()
            .Should().NotContain("label");
    }

    [Fact]
    public void litecoin_address_generator_should_skip_missing_message()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;


        var generator = new LitecoinAddress(address, amount);

        generator
            .ToString()
            .Should().NotContain("message");
    }

    [Fact]
    public void litecoin_address_generator_should_round_to_satoshi()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123456789;


        var generator = new LitecoinAddress(address, amount);

        generator
            .ToString()
            .Should().Contain("amount=.12345679");
    }

    [Fact]
    public void litecoin_address_generator_disregards_current_culture()
    {
#if NETCOREAPP1_1
            var currentCulture = CultureInfo.DefaultThreadCurrentCulture;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
#else
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
#endif

        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;


        var generator = new LitecoinAddress(address, amount);

        generator
            .ToString()
            .Should().Be("litecoin:LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54?amount=.123");

#if NETCOREAPP1_1
            CultureInfo.DefaultThreadCurrentCulture = currentCulture;
#else
        Thread.CurrentThread.CurrentCulture = currentCulture;
#endif
    }
}