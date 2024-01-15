using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void swissqrcode_generator_should_throw_reference_not_allowed()
    {
        var refType = SwissQrCode.Reference.ReferenceType.NON;
        var reference = "1234567890123456";
        var refTextType = SwissQrCode.Reference.ReferenceTextType.CreditorReferenceIso11649;

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Reference is only allowed when referenceType not equals \"NON\"");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_missing_reftexttype()
    {
        var refType = SwissQrCode.Reference.ReferenceType.SCOR;
        var reference = "1234567890123456";

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("You have to set an ReferenceTextType when using the reference text.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_too_long()
    {
        var refType = SwissQrCode.Reference.ReferenceType.QRR;
        var reference = "9900050000000003200710123031234654574398214093682164062138462089364";
        var refTextType = SwissQrCode.Reference.ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("QR-references have to be shorter than 28 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_wrong_char()
    {
        var refType = SwissQrCode.Reference.ReferenceType.QRR;
        var reference = "99000ABCDF5000032007101230";
        var refTextType = SwissQrCode.Reference.ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("QR-reference must exist out of digits only.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_qrr_ref_checksum_invalid()
    {
        var refType = SwissQrCode.Reference.ReferenceType.QRR;
        var reference = "990005000000000320071012304";
        var refTextType = SwissQrCode.Reference.ReferenceTextType.QrReference;

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("QR-references is invalid. Checksum error.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_iso11649_ref_too_long()
    {
        var refType = SwissQrCode.Reference.ReferenceType.QRR;
        var reference = "99000500000000032007101230312346545743982162138462089364";
        var refTextType = SwissQrCode.Reference.ReferenceTextType.CreditorReferenceIso11649;

        var exception = Record.Exception(() => new SwissQrCode.Reference(refType, reference, refTextType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Creditor references (ISO 11649) have to be shorter than 26 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_unstructured_msg_too_long()
    {
        var billInformation = "This is sample bill information with a length below 140.";
        var unstructuredMessage =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum";

        var exception =
            Record.Exception(() => new SwissQrCode.AdditionalInformation(unstructuredMessage, billInformation));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should()
            .Be("Unstructured message and bill information must be shorter than 141 chars in total/combined.");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_iban()
    {
        var iban = "CH2609000000857666015";
        var ibanType = SwissQrCode.Iban.IbanType.Iban;

        var generator = new SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .Should().Be("CH2609000000857666015");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_iban_2()
    {
        var iban = "CH47048350000GABRIELS";
        var ibanType = SwissQrCode.Iban.IbanType.Iban;

        var generator = new SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .Should().Be("CH47048350000GABRIELS");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_iban_qr()
    {
        var iban = "CH2430043000000789012";
        var ibanType = SwissQrCode.Iban.IbanType.QrIban;

        var generator = new SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .Should().Be("CH2430043000000789012");
    }

    [Fact]
    public void swissqrcode_generator_should_remove_spaces_iban()
    {
        var iban = "CH26 0900 0000 8576 6601 5";
        var ibanType = SwissQrCode.Iban.IbanType.Iban;

        var generator = new SwissQrCode.Iban(iban, ibanType);

        generator
            .ToString()
            .Should().Be("CH2609000000857666015");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_invalid_iban()
    {
        var iban = "CHC2609000000857666015";
        var ibanType = SwissQrCode.Iban.IbanType.Iban;

        var exception = Record.Exception(() => new SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("The IBAN entered isn't valid.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_invalid_qriban()
    {
        var iban = "CHC2609000000857666015";
        var ibanType = SwissQrCode.Iban.IbanType.QrIban;

        var exception = Record.Exception(() => new SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("The QR-IBAN entered isn't valid.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_ivalid_iban_country()
    {
        var iban = "DE2609000000857666015";
        var ibanType = SwissQrCode.Iban.IbanType.Iban;

        var exception = Record.Exception(() => new SwissQrCode.Iban(iban, ibanType));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("The IBAN must start with \"CH\" or \"LI\".");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_contact_simple()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";

        var generator = SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, null, null);

        generator
            .ToString()
            .Should().Be("S\r\nJohn Doe\r\n\r\n\r\n3003\r\nBern\r\nCH\r\n");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_contact_full()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var generator = SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber);

        generator
            .ToString()
            .Should().Be("S\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_name_empty()
    {
        var name = "";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Name must not be empty.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_name_too_long()
    {
        var name = "John Dorian Peter Charles Lord of the Rings and Master of Disaster Grayham";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Name must be shorter than 71 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_street_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude in der wunderschönen aber auch ziemlich teuren Stadt Bern in der Schweiz";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Street must be shorter than 71 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_street_with_illegal_char()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude 1 ♥";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should()
            .Be(
                @"Street must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_housenumber_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "123456789123456789";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("House number must be shorter than 17 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_zip_empty()
    {
        var name = "John Doe";
        var zip = "";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Zip code must not be empty.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_zip_too_long()
    {
        var name = "John Doe";
        var zip = "30031234567891234";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Zip code must be shorter than 17 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_zip_has_illegal_char()
    {
        var name = "John Doe";
        var zip = "3003CHF♥";
        var city = "Bern";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should()
            .Be(
                @"Zip code must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_city_empty()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("City must not be empty.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_city_too_long()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Berner-Sangerhausen-Ober-Hinter-der-Alm-Stadt-am-Unter-Über-Berg";
        var country = "CH";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("City name must be shorter than 36 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_wrong_countrycode()
    {
        var name = "John Doe";
        var zip = "3003";
        var city = "Bern";
        var country = "CHE";
        var street = "Parlamentsgebäude";
        var houseNumber = "1";

        var exception = Record.Exception(() =>
            SwissQrCode.Contact.WithStructuredAddress(name, zip, city, country, street, houseNumber));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should()
            .Be("Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_simple()
    {
        var creditor =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR,
            "990005000000000320071012303", SwissQrCode.Reference.ReferenceTextType.QrReference);
        var currency = SwissQrCode.Currency.EUR;

        var generator = new SwissQrCode(iban, currency, creditor, reference);

        generator
            .ToString()
            .Should().Be(
                "SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nEUR\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nQRR\r\n990005000000000320071012303\r\n\r\nEPD\r\n");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_full()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR,
            "990005000000000320071012303", SwissQrCode.Reference.ReferenceTextType.QrReference);
        var currency = SwissQrCode.Currency.CHF;
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation,
            contactGeneral, amount, reqDateOfPayment, contactGeneral);

        generator
            .ToString()
            .Should().Be(
                "SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_clean_end_linebreaks()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR,
            "990005000000000320071012303", SwissQrCode.Reference.ReferenceTextType.QrReference);
        var currency = SwissQrCode.Currency.CHF;
        var additionalInformation = new SwissQrCode.AdditionalInformation("This is my unstructured message.");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation,
            contactGeneral, amount, reqDateOfPayment, contactGeneral);

        generator
            .ToString()
            .Should().Be(
                "SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD\r\n");
    }

    [Fact]
    public void swissqrcode_generator_should_generate_swisscode_full_alt()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR,
            "990005000000000320071012303", SwissQrCode.Reference.ReferenceTextType.QrReference);
        var currency = SwissQrCode.Currency.CHF;
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation,
            contactGeneral, amount, reqDateOfPayment, contactGeneral, "alt1", "alt2");

        generator
            .ToString()
            .Should().Be(
                "SPC\r\n0200\r\n1\r\nCH2430043000000789012\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n100.25\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...\r\nalt1\r\nalt2");
    }

    [Fact]
    public void swissqrcode_generator_should_not_generate_space_as_thousands_separator()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2609000000857666015", SwissQrCode.Iban.IbanType.Iban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.SCOR, "99000500000000032003",
            SwissQrCode.Reference.ReferenceTextType.CreditorReferenceIso11649);
        var currency = SwissQrCode.Currency.CHF;
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var amount = 1234567.89m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var generator = new SwissQrCode(iban, currency, contactGeneral, reference, additionalInformation,
            contactGeneral, amount, reqDateOfPayment, contactGeneral, "alt1", "alt2");

        generator
            .ToString()
            .Should().Be(
                "SPC\r\n0200\r\n1\r\nCH2609000000857666015\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n1234567.89\r\nCHF\r\nS\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nSCOR\r\n99000500000000032003\r\nThis is my unstructured message.\r\nEPD\r\nSome bill information here...\r\nalt1\r\nalt2");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_amount_too_big()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2609000000857666015", SwissQrCode.Iban.IbanType.Iban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR,
            "990005000000000320071012303", SwissQrCode.Reference.ReferenceTextType.QrReference);
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = SwissQrCode.Currency.CHF;
        var amount = 1234567891.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var exception = Record.Exception(() => new SwissQrCode(iban, currency, contactGeneral, reference,
            additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Amount (including decimals) must be shorter than 13 places.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_incompatible_reftype()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.NON);
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);

        var exception = Record.Exception(() => new SwissQrCode(iban, currency, contactGeneral, reference,
            additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("If QR-IBAN is used, you have to choose \"QRR\" as reference type!");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_alt1_too_long()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR);
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);
        var alt1 =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";

        var exception = Record.Exception(() => new SwissQrCode(iban, currency, contactGeneral, reference,
            additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Alternative procedure information block 1 must be shorter than 101 chars.");
    }

    [Fact]
    public void swissqrcode_generator_should_throw_alt2_too_long()
    {
        var contactGeneral =
            SwissQrCode.Contact.WithStructuredAddress("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
        var iban = new SwissQrCode.Iban("CH2430043000000789012", SwissQrCode.Iban.IbanType.QrIban);
        var reference = new SwissQrCode.Reference(SwissQrCode.Reference.ReferenceType.QRR);
        var additionalInformation =
            new SwissQrCode.AdditionalInformation("This is my unstructured message.", "Some bill information here...");
        var currency = SwissQrCode.Currency.CHF;
        var amount = 100.25m;
        var reqDateOfPayment = new DateTime(2017, 03, 01);
        var alt1 = "lorem ipsum";
        var alt2 =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";
        var exception = Record.Exception(() => new SwissQrCode(iban, currency, contactGeneral, reference,
            additionalInformation, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1, alt2));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        exception.Message.Should().Be("Alternative procedure information block 2 must be shorter than 101 chars.");
    }
}