using FluentAssertions;
using static ItsyBIT.Utilities.EncodedIdentifierGenerator;

namespace ItsyBIT.Utilities.Tests
{
    public class EncodedIdentifierTests
    {
        const string GuidString = "9dc2205a-2ff0-4ae4-a48f-5557be2485bf";
        const string shortGuid = "YQAgMy0MBx5/JD0mCHtjbkAANzY1HQ==";
        Guid Guid = System.Guid.Parse(GuidString);
        const string key = "6idpCjFhJaLMbJ56";
        const string toEncode = "HleUiCsPgkGZpHwxWPthrQ";
        private EncodedIdentifierGenerator generator = null;
        public EncodedIdentifierTests()
        {
            generator = new EncodedIdentifierGenerator(key);
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

        // this was a utility to find a Guid that gets encoded into a string with a slash in it
        public void EncodeUntilContains()
        {
            string encoded;
            Guid guid;
            do
            {
                guid = System.Guid.NewGuid();
                encoded = generator.Create(guid).EncodedShortKey;

            } while (!encoded.Contains("/"));
            encoded.Should().Contain("/");
        }
    }
}