using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void contactdata_generator_can_generate_payload_simple_mecard()
    {
        var firstname = "John";
        var lastname = "Doe";
        var outputType = ContactData.ContactOutputType.MeCard;

        var generator = new ContactData(outputType, firstname, lastname);

        generator
            .ToString()
            .Should().Be("MECARD+\r\nN:Doe, John\r\nADR:,,,,,,");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_mecard()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.MeCard;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "MECARD+\r\nN:Doe, John\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL:+4253212222\r\nTEL:+421701234567\r\nTEL:+4253211337\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nBDAY:19700201\r\nADR:,,Long street 42,12345,Super-Town,,Starlight Country\r\nURL:http://john.doe\r\nNICKNAME:Johnny");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_mecard_reversed()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.MeCard;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note,
            addressOrder: ContactData.AddressOrder.Reversed, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "MECARD+\r\nN:Doe, John\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL:+4253212222\r\nTEL:+421701234567\r\nTEL:+4253211337\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nBDAY:19700201\r\nADR:,,42 Long street,Super-Town,,12345,Starlight Country\r\nURL:http://john.doe\r\nNICKNAME:Johnny");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_vcard21()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.VCard21;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "BEGIN:VCARD\r\nVERSION:2.1\r\nN:Doe;John;;;\r\nFN:John Doe\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL;HOME;VOICE:+4253212222\r\nTEL;HOME;CELL:+421701234567\r\nTEL;WORK;VOICE:+4253211337\r\nADR;HOME;PREF:;;Long street 42;12345;Super-Town;;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nEND:VCARD");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_vcard3()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.VCard3;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "BEGIN:VCARD\r\nVERSION:3.0\r\nN:Doe;John;;;\r\nFN:John Doe\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL;TYPE=HOME,VOICE:+4253212222\r\nTEL;TYPE=HOME,CELL:+421701234567\r\nTEL;TYPE=WORK,VOICE:+4253211337\r\nADR;TYPE=HOME,PREF:;;Long street 42;12345;Super-Town;;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nNICKNAME:Johnny\r\nEND:VCARD");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_vcard4()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.VCard4;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "BEGIN:VCARD\r\nVERSION:4.0\r\nN:Doe;John;;;\r\nFN:John Doe\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL;TYPE=home,voice;VALUE=uri:tel:+4253212222\r\nTEL;TYPE=home,cell;VALUE=uri:tel:+421701234567\r\nTEL;TYPE=work,voice;VALUE=uri:tel:+4253211337\r\nADR;TYPE=home,pref:;;Long street 42;12345;Super-Town;;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nNICKNAME:Johnny\r\nEND:VCARD");
    }

    [Fact]
    public void contactdata_generator_can_generate_payload_full_vcard4_reverse()
    {
        var firstname = "John";
        var lastname = "Doe";
        var nickname = "Johnny";
        var org = "Johnny's Badass Programming";
        var orgTitle = "Badass Manager";
        var phone = "+4253212222";
        var mobilePhone = "+421701234567";
        var workPhone = "+4253211337";
        var email = "me@john.doe";
        var birthday = new DateTime(1970, 02, 01);
        var website = "http://john.doe";
        var street = "Long street";
        var houseNumber = "42";
        var city = "Super-Town";
        var zipCode = "12345";
        var country = "Starlight Country";
        var note = "Badass programmer.";
        var outputType = ContactData.ContactOutputType.VCard4;

        var generator = new ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email,
            birthday, website, street, houseNumber, city, zipCode, country, note,
            addressOrder: ContactData.AddressOrder.Reversed, org: org, orgTitle: orgTitle);

        generator
            .ToString()
            .Should().Be(
                "BEGIN:VCARD\r\nVERSION:4.0\r\nN:Doe;John;;;\r\nFN:John Doe\r\nORG:Johnny's Badass Programming\r\nTITLE:Badass Manager\r\nTEL;TYPE=home,voice;VALUE=uri:tel:+4253212222\r\nTEL;TYPE=home,cell;VALUE=uri:tel:+421701234567\r\nTEL;TYPE=work,voice;VALUE=uri:tel:+4253211337\r\nADR;TYPE=home,pref:;;42 Long street;Super-Town;;12345;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nNICKNAME:Johnny\r\nEND:VCARD");
    }
}