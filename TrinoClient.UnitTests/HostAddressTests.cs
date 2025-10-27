using System;
using Xunit;
using TrinoClient.Model.SPI;

namespace TrinoClient.UnitTests
{
    public class HostAddressTests
    {
        [Fact]
        public void Constructor_HostAndPort_CreatesInstance()
        {
            // Arrange
            var host = "localhost";
            var port = 8080;

            // Act
            var hostAddress = new HostAddress(host, port);

            // Assert
            Assert.Equal(host, hostAddress.Host);
            Assert.Equal(port, hostAddress.Port);
        }

        [Fact]
        public void Constructor_FromString_ParsesCorrectly()
        {
            // Arrange
            var hostPortString = "localhost:8080";

            // Act
            var hostAddress = new HostAddress(hostPortString);

            // Assert
            Assert.Equal("localhost", hostAddress.Host);
            Assert.Equal(8080, hostAddress.Port);
        }

        [Fact]
        public void FromString_HostOnly_ParsesWithoutPort()
        {
            // Arrange
            var hostString = "localhost";

            // Act
            var hostAddress = HostAddress.FromString(hostString);

            // Assert
            Assert.Equal("localhost", hostAddress.Host);
            Assert.Equal(-1, hostAddress.Port);
            Assert.False(hostAddress.HasPort());
        }

        [Fact]
        public void FromString_NullString_ThrowsException()
        {
            // Arrange
            string hostString = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HostAddress.FromString(hostString));
        }

        [Fact]
        public void FromString_EmptyString_ThrowsException()
        {
            // Arrange
            var hostString = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HostAddress.FromString(hostString));
        }

        [Fact]
        public void FromString_InvalidPort_ThrowsException()
        {
            // Arrange
            var hostPortString = "localhost:abc";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => HostAddress.FromString(hostPortString));
        }

        [Fact]
        public void FromString_PortTooHigh_ThrowsException()
        {
            // Arrange
            var hostPortString = "localhost:99999";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => HostAddress.FromString(hostPortString));
        }

        [Fact]
        public void FromString_IPv6Address_ParsesCorrectly()
        {
            // Arrange
            var hostPortString = "[2001:db8::1]:8080";

            // Act
            var hostAddress = HostAddress.FromString(hostPortString);

            // Assert
            Assert.Equal("2001:db8::1", hostAddress.Host);
            Assert.Equal(8080, hostAddress.Port);
        }

        [Fact]
        public void FromString_IPv6AddressNoPort_ParsesCorrectly()
        {
            // Arrange
            var hostString = "[2001:db8::1]";

            // Act
            var hostAddress = HostAddress.FromString(hostString);

            // Assert
            Assert.Equal("2001:db8::1", hostAddress.Host);
            Assert.Equal(-1, hostAddress.Port);
        }

        [Fact]
        public void FromString_InvalidBracketedAddress_ThrowsException()
        {
            // Arrange
            var hostPortString = "[invalid";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => HostAddress.FromString(hostPortString));
        }

        [Fact]
        public void HasPort_WithPort_ReturnsTrue()
        {
            // Arrange
            var hostAddress = new HostAddress("localhost", 8080);

            // Act
            var result = hostAddress.HasPort();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasPort_NoPort_ReturnsFalse()
        {
            // Arrange
            var hostAddress = new HostAddress("localhost", -1);

            // Act
            var result = hostAddress.HasPort();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToString_HostAndPort_FormatsCorrectly()
        {
            // Arrange
            var hostAddress = new HostAddress("localhost", 8080);

            // Act
            var result = hostAddress.ToString();

            // Assert
            Assert.Equal("localhost:8080", result);
        }

        [Fact]
        public void ToString_HostOnly_FormatsCorrectly()
        {
            // Arrange
            var hostAddress = new HostAddress("localhost", -1);

            // Act
            var result = hostAddress.ToString();

            // Assert
            Assert.Equal("localhost", result);
        }

        [Fact]
        public void ToString_IPv6AddressWithPort_FormatsCorrectly()
        {
            // Arrange
            var hostAddress = new HostAddress("2001:db8::1", 8080);

            // Act
            var result = hostAddress.ToString();

            // Assert
            Assert.Equal("[2001:db8::1]:8080", result);
        }

        [Fact]
        public void FromString_PortZero_ParsesCorrectly()
        {
            // Arrange
            var hostPortString = "localhost:0";

            // Act
            var hostAddress = HostAddress.FromString(hostPortString);

            // Assert
            Assert.Equal("localhost", hostAddress.Host);
            Assert.Equal(0, hostAddress.Port);
        }

        [Fact]
        public void FromString_MaxValidPort_ParsesCorrectly()
        {
            // Arrange
            var hostPortString = "localhost:65535";

            // Act
            var hostAddress = HostAddress.FromString(hostPortString);

            // Assert
            Assert.Equal("localhost", hostAddress.Host);
            Assert.Equal(65535, hostAddress.Port);
        }
    }
}
