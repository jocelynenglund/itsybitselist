using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using static ItsyBIT.Utilities.EncodedIdentifierGenerator;

namespace ItsyBIT.Utilities.Tests
{
    public class EncodedIdentifierTests
    {
        const string GuidString = "8894571e-0f2b-4182-99a4-7c3158fb61ad";
        const string shortGuid = "PAkWITomECICHww/CXlDTSM1BxwhNA==";
        Guid Guid = System.Guid.Parse(GuidString);
        const string key = "testSecretKey145";
        const string toEncode = "HleUiCsPgkGZpHwxWPthrQ";
        private EncodedIdentifierGenerator generator = null;
        public EncodedIdentifierTests()
        {
            generator = new EncodedIdentifierGenerator(Mock.Of<IConfiguration>());
            EncodedIdentifierGenerator.GetKey = () => key;
        }

        [Fact]
        public void CanEncodeGuid()
        {


            var encoded = generator.Create(Guid);

            encoded.EncodedShortKey.Should().Be($"{shortGuid}");

        }
        [Fact]
        public void CanDecodeGuid()
        {

            var decoded = generator.Create(shortGuid).Guid;

            decoded.Should().Be(Guid);
        }

        [Fact]
        public void CanEncodeAndDecodeString()
        {
            var encoded = EncodedIdentifier.EncodeString(toEncode, key);
            var decoded = EncodedIdentifier.DecodeString(encoded, key);

            decoded.Should().Be(toEncode);
        }
    }
}