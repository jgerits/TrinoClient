using System;
using Xunit;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.UnitTests
{
    public class TimeZoneKeyTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            var id = "America/New_York";
            short key = 1;

            // Act
            var timeZoneKey = new TimeZoneKey(id, key);

            // Assert
            Assert.Equal(id, timeZoneKey.Id);
            Assert.Equal(key, timeZoneKey.Key);
        }

        [Fact]
        public void Constructor_NullId_ThrowsException()
        {
            // Arrange
            string id = null;
            short key = 1;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TimeZoneKey(id, key));
        }

        [Fact]
        public void Constructor_EmptyId_ThrowsException()
        {
            // Arrange
            var id = string.Empty;
            short key = 1;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TimeZoneKey(id, key));
        }

        [Fact]
        public void Constructor_NegativeKey_ThrowsException()
        {
            // Arrange
            var id = "America/New_York";
            short key = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeZoneKey(id, key));
        }

        [Fact]
        public void UTC_KEY_IsInitialized()
        {
            // Assert
            Assert.NotNull(TimeZoneKey.UTC_KEY);
            Assert.Equal("UTC", TimeZoneKey.UTC_KEY.Id);
            Assert.Equal(0, TimeZoneKey.UTC_KEY.Key);
        }

        [Fact]
        public void GetTimeZoneKey_ValidKey_ReturnsTimeZoneKey()
        {
            // Act
            var timeZoneKey = TimeZoneKey.GetTimeZoneKey(0);

            // Assert
            Assert.NotNull(timeZoneKey);
            Assert.Equal("UTC", timeZoneKey.Id);
            Assert.Equal(0, timeZoneKey.Key);
        }
    }
}
