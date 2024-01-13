using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void iban_validator_validate_german_iban()
    {
        "DE15268500010154131577".IsValidIban().Should().Be(true);
    }

    [Fact]
    public void iban_validator_validate_swiss_iban()
    {
        "CH1900767000U00121977".IsValidIban().Should().Be(true);
    }

    [Fact]
    public void iban_validator_invalidates_iban()
    {
        "DE29268500010154131577".IsValidIban().Should().Be(false);
    }

    [Fact]
    public void qriban_validator_validates_iban()
    {
        SwissQrCode.Iban.IsValidQrIban("CH2430043000000789012").Should().Be(true);
    }

    [Fact]
    public void qriban_validator_invalidates_iban()
    {
        SwissQrCode.Iban.IsValidQrIban("CH3908704016075473007").Should().Be(false);
    }
}