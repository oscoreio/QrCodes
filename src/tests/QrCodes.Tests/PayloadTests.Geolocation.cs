using FluentAssertions;
using QrCodes.Payloads;
using Xunit;

namespace QrCodes.Tests;

public partial class PayloadTests
{
    [Fact]
    public void geolocation_should_build_type_GEO()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";
        var encoding = Geolocation.GeolocationEncoding.Geo;

        var generator = new Geolocation(latitude, longitude, encoding);

        generator.ToString().Should().Be("geo:51.227741,6.773456");
    }

    [Fact]
    public void geolocation_should_build_type_GoogleMaps()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";
        var encoding = Geolocation.GeolocationEncoding.GoogleMaps;

        var generator = new Geolocation(latitude, longitude, encoding);

        generator.ToString().Should().Be("http://maps.google.com/maps?q=51.227741,6.773456");
    }

    [Fact]
    public void geolocation_should_escape_input()
    {
        var latitude = "51,227741";
        var longitude = "6,773456";
        var encoding = Geolocation.GeolocationEncoding.Geo;

        var generator = new Geolocation(latitude, longitude, encoding);

        generator.ToString().Should().Be("geo:51.227741,6.773456");
    }

    [Fact]
    public void geolocation_should_add_unused_params()
    {
        var latitude = "51.227741";
        var longitude = "6.773456";

        var generator = new Geolocation(latitude, longitude);

        generator.ToString().Should().Be("geo:51.227741,6.773456");
    }
}