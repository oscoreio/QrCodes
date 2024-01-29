using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void ethereum_address_generator_can_generate_address()
    {
        new EthereumAddress(
                address: "0xfb6916095ca1df60bb79Ce92ce3ea74c37c5d359",
                value: 2014000000000000000D)
            .ToString()
            .Should().Be(
                "ethereum:0xfb6916095ca1df60bb79Ce92ce3ea74c37c5d359?value=2.014e18");
    }
}