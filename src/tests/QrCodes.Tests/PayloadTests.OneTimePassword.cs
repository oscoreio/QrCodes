using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void one_time_password_generator_time_based_generates_with_standard_options()
    {
        var pg = new OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
        };

        pg.ToString().Should().Be("otpauth://totp/Google:test@google.com?secret=pwq65q55&issuer=Google");
    }

    [Fact]
    public void one_time_password_generator_hmac_based_generates_with_standard_options()
    {
        var pg = new OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = OneTimePassword.OneTimePasswordAuthType.HashBased,
            Counter = 500,
        };

        pg.ToString().Should().Be("otpauth://hotp/Google:test@google.com?secret=pwq65q55&issuer=Google&counter=500");
    }
}