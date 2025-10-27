using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using TrinoClient;
using TrinoClient.Model.SPI.Type;

namespace TrinoClient.UnitTests
{
    public class TrinoClientSessionConfigTests
    {
        [Fact]
        public void Constructor_Default_SetsDefaultValues()
        {
            // Arrange & Act
            var config = new TrinoClientSessionConfig();

            // Assert
            Assert.Equal("localhost", config.Host);
            Assert.Equal(8080, config.Port);
            Assert.NotNull(config.User);
            Assert.False(config.IgnoreSslErrors);
            Assert.False(config.UseSsl);
            Assert.NotNull(config.Properties);
            Assert.NotNull(config.PreparedStatements);
            Assert.NotNull(config.ClientTags);
            Assert.False(config.Debug);
            Assert.Equal(-1, config.ClientRequestTimeout);
        }

        [Fact]
        public void Constructor_WithCatalog_SetsCatalog()
        {
            // Arrange & Act
            var config = new TrinoClientSessionConfig("test_catalog");

            // Assert
            Assert.Equal("test_catalog", config.Catalog);
        }

        [Fact]
        public void Constructor_WithCatalogAndSchema_SetsBoth()
        {
            // Arrange & Act
            var config = new TrinoClientSessionConfig("test_catalog", "test_schema");

            // Assert
            Assert.Equal("test_catalog", config.Catalog);
            Assert.Equal("test_schema", config.Schema);
        }

        [Fact]
        public void Constructor_WithHostPortCatalog_SetsAllValues()
        {
            // Arrange & Act
            var config = new TrinoClientSessionConfig("test_host", 9090, "test_catalog");

            // Assert
            Assert.Equal("test_host", config.Host);
            Assert.Equal(9090, config.Port);
            Assert.Equal("test_catalog", config.Catalog);
        }

        [Fact]
        public void Port_ValidPort_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act
            config.Port = 443;

            // Assert
            Assert.Equal(443, config.Port);
        }

        [Fact]
        public void Port_PortTooLow_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => config.Port = 0);
        }

        [Fact]
        public void Port_PortTooHigh_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => config.Port = 65536);
        }

        [Fact]
        public void User_NullValue_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => config.User = null);
        }

        [Fact]
        public void User_EmptyValue_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => config.User = string.Empty);
        }

        [Fact]
        public void User_ValidValue_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act
            config.User = "testuser";

            // Assert
            Assert.Equal("testuser", config.User);
        }

        [Fact]
        public void Properties_ValidProperties_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var properties = new Dictionary<string, string>
            {
                { "property1", "value1" },
                { "property2", "value2" }
            };

            // Act
            config.Properties = properties;

            // Assert
            Assert.Equal(properties, config.Properties);
        }

        [Fact]
        public void Properties_WithEmptyKey_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var properties = new Dictionary<string, string>
            {
                { "", "value1" }
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => config.Properties = properties);
        }

        [Fact]
        public void Properties_WithEqualsInKey_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var properties = new Dictionary<string, string>
            {
                { "key=value", "value1" }
            };

            // Act & Assert
            Assert.Throws<FormatException>(() => config.Properties = properties);
        }

        [Fact]
        public void ClientTags_ValidTags_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var tags = new HashSet<string> { "tag1", "tag2" };

            // Act
            config.ClientTags = tags;

            // Assert
            Assert.Equal(tags, config.ClientTags);
        }

        [Fact]
        public void ClientTags_WithComma_ThrowsException()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var tags = new HashSet<string> { "tag,with,comma" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => config.ClientTags = tags);
        }

        [Fact]
        public void Password_ValidValue_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();

            // Act
            config.Password = "testpassword";

            // Assert
            Assert.Equal("testpassword", config.Password);
        }

        [Fact]
        public void PreparedStatements_ValidStatements_SetsValue()
        {
            // Arrange
            var config = new TrinoClientSessionConfig();
            var statements = new Dictionary<string, string>
            {
                { "stmt1", "SELECT * FROM table1" },
                { "stmt2", "SELECT * FROM table2" }
            };

            // Act
            config.PreparedStatements = statements;

            // Assert
            Assert.Equal(statements, config.PreparedStatements);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsAllValues()
        {
            // Arrange
            var clientTags = new HashSet<string> { "tag1" };
            var properties = new Dictionary<string, string> { { "key1", "value1" } };
            var preparedStatements = new Dictionary<string, string> { { "stmt1", "SELECT 1" } };
            var locale = CultureInfo.GetCultureInfo("en-US");

            // Act
            var config = new TrinoClientSessionConfig(
                "test_host",
                9090,
                "test_catalog",
                "test_schema",
                "test_user",
                clientTags,
                "test_client_info",
                locale,
                properties,
                preparedStatements,
                true,
                300
            );

            // Assert
            Assert.Equal("test_host", config.Host);
            Assert.Equal(9090, config.Port);
            Assert.Equal("test_catalog", config.Catalog);
            Assert.Equal("test_schema", config.Schema);
            Assert.Equal("test_user", config.User);
            Assert.Equal(clientTags, config.ClientTags);
            Assert.Equal("test_client_info", config.ClientInfo);
            Assert.Equal(locale, config.Locale);
            Assert.Equal(properties, config.Properties);
            Assert.Equal(preparedStatements, config.PreparedStatements);
            Assert.True(config.Debug);
            Assert.Equal(300, config.ClientRequestTimeout);
        }
    }
}
