using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void mail_should_build_type_mailto()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var message = "Just see if it works!";
        var encoding = Mail.MailEncoding.MailTo;

        var generator = new Mail(receiver, subject, message, encoding);

        generator.ToString().Should()
            .Be("mailto:john@doe.com?subject=A%20test%20mail&body=Just%20see%20if%20it%20works%21");
    }

    [Fact]
    public void mail_should_build_type_mailto_receiver_only()
    {
        var receiver = "john@doe.com";
        var encoding = Mail.MailEncoding.MailTo;

        var generator = new Mail(email: receiver, encoding: encoding);

        generator.ToString().Should().Be("mailto:john@doe.com");
    }

    [Fact]
    public void mail_should_build_type_mailto_subject_only()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var encoding = Mail.MailEncoding.MailTo;

        var generator = new Mail(receiver, subject, encoding: encoding);

        generator.ToString().Should().Be("mailto:john@doe.com?subject=A%20test%20mail");
    }

    [Fact]
    public void mail_should_build_type_mailto_message_only()
    {
        var receiver = "john@doe.com";
        var message = "Just see if it works!";
        var encoding = Mail.MailEncoding.MailTo;

        var generator = new Mail(receiver, message: message, encoding: encoding);

        generator.ToString().Should().Be("mailto:john@doe.com?body=Just%20see%20if%20it%20works%21");
    }

    [Fact]
    public void mail_should_build_type_mailto_no_receiver()
    {
        var subject = "A test mail";
        var message = "Just see if it works!";
        var encoding = Mail.MailEncoding.MailTo;

        var generator = new Mail(subject: subject, message: message, encoding: encoding);

        generator.ToString().Should().Be("mailto:?subject=A%20test%20mail&body=Just%20see%20if%20it%20works%21");
    }

    [Fact]
    public void mail_should_build_type_MATMSG()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var message = "Just see if it works!";
        var encoding = Mail.MailEncoding.MatMsg;

        var generator = new Mail(receiver, subject, message, encoding);

        generator.ToString().Should().Be("MATMSG:TO:john@doe.com;SUB:A test mail;BODY:Just see if it works!;;");
    }

    [Fact]
    public void mail_should_build_type_SMTP()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var message = "Just see if it works!";
        var encoding = Mail.MailEncoding.Smtp;

        var generator = new Mail(receiver, subject, message, encoding);

        generator.ToString().Should().Be("SMTP:john@doe.com:A test mail:Just see if it works!");
    }

    [Fact]
    public void mail_should_escape_input_MATMSG()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var message = "Just see if \\:;, it works!";
        var encoding = Mail.MailEncoding.MatMsg;

        var generator = new Mail(receiver, subject, message, encoding);

        generator.ToString().Should()
            .Be("MATMSG:TO:john@doe.com;SUB:A test mail;BODY:Just see if \\\\\\:\\;\\, it works!;;");
    }

    [Fact]
    public void mail_should_escape_input_SMTP()
    {
        var receiver = "john@doe.com";
        var subject = "A test mail";
        var message = "Just see: if it works!";
        var encoding = Mail.MailEncoding.Smtp;

        var generator = new Mail(receiver, subject, message, encoding);

        generator.ToString().Should().Be("SMTP:john@doe.com:A test mail:Just see\\: if it works!");
    }
}