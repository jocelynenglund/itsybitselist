using FluentAssertions;
using System.Text;

namespace ItsyBIT.Utilities.Tests
{
    public class EncodedIdentifierTests
    {
        const string GuidString = "8894571e-0f2b-4182-99a4-7c3158fb61ad";
        const string shortGuid = "PAkWITomECICHww/CXlDTSM1BxwhNA==";
        Guid Guid = System.Guid.Parse(GuidString);
        const string key = "testSecretKey145";
        const string toEncode = "HleUiCsPgkGZpHwxWPthrQ";

        [Fact]
        public void CanEncodeGuid()
        {

            EncodedIdentifier.GetKey = () => key;

            var encoded = new EncodedIdentifier(Guid);

            encoded.EncodedShortKey.Should().Be($"{shortGuid}");

        }
        [Fact]
        public void CanDecodeGuid()
        {
            EncodedIdentifier.GetKey = () => key;

            var decoded = new EncodedIdentifier(shortGuid).Guid;

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