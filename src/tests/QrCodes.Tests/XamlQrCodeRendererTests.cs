﻿#if NETFRAMEWORK || NET5_0_WINDOWS || NET6_0_WINDOWS
using Xunit;
using QrCodes;
using QRCoder.Xaml;
using FluentAssertions;
using QrCodes.Tests.Helpers;

namespace QrCodes.Tests
{

    public class XamlQrCodeRendererTests
    {
        [Fact]
        public void can_create_xaml_qrcode_standard_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var xCode = new XamlQRCode(data).GetGraphic(10);

            var bmp = HelperFunctions.BitmapSourceToBitmap(xCode);
            var result = HelperFunctions.BitmapToHash(bmp);
            result.Should().Be("e8c61b8f0455924fe08ba68686d0d296");
        }


        [Fact]
        public void can_instantate_qrcode_parameterless()
        {
            var svgCode = new XamlQRCode();
            svgCode.ShouldNotBeNull();
            svgCode.Should().BeOfType<XamlQRCode>();
        }

        /*
        [Fact]
        public void can_render_qrcode_from_helper()
        {
            //Create QR code                   
            var bmp = QRCodeHelper.GetQRCode("This is a quick test! 123#?", 10, Color.Black, Color.White, QRCodeGenerator.ECCLevel.H);

            var result = HelperFunctions.BitmapToHash(bmp);
            result.Should().Be("e8c61b8f0455924fe08ba68686d0d296");
        }
        */
    }
}
#endif