using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void mms_should_build_type_MMS()
    {
        var number = "01601234567";
        var message = "A tiny MMS";
        var encoding = Mms.MmsEncoding.Mms;

        var generator = new Mms(number, message, encoding);

        generator.ToString().Should().Be("mms:01601234567?body=A%20tiny%20MMS");
    }

    [Fact]
    public void mms_should_build_type_MMSTO()
    {
        var number = "01601234567";
        var message = "A tiny SMS";
        var encoding = Mms.MmsEncoding.MmsTo;

        var generator = new Mms(number, message, encoding);

        generator.ToString().Should().Be("mmsto:01601234567?subject=A%20tiny%20SMS");
    }

    [Fact]
    public void mms_should_not_add_unused_params()
    {
        var number = "01601234567";

        var generator = new Mms(number);

        generator.ToString().Should().Be("mms:01601234567");
    }
}