using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void sms_should_build_type_SMS()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = Sms.SmsEncoding.Sms;

        var generator = new Sms(number, message, encoding);

        generator.ToString().Should().Be("sms:01601234567?body=A%20small%20SMS");
    }

    [Fact]
    public void sms_should_build_type_SMS_iOS()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = Sms.SmsEncoding.SmsIos;

        var generator = new Sms(number, message, encoding);

        generator.ToString().Should().Be("sms:01601234567;body=A%20small%20SMS");
    }

    [Fact]
    public void sms_should_build_type_SMSTO()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = Sms.SmsEncoding.SmsTo;

        var generator = new Sms(number, message, encoding);

        generator.ToString().Should().Be("SMSTO:01601234567:A small SMS");
    }

    [Fact]
    public void sms_should_not_add_unused_params()
    {
        var number = "01601234567";

        var generator = new Sms(number);

        generator.ToString().Should().Be("sms:01601234567");
    }
}