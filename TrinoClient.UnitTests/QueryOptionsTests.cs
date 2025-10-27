using System;
using System.Collections.Generic;
using Xunit;
using TrinoClient.Model;

namespace TrinoClient.UnitTests
{
    public class QueryOptionsTests
    {
        [Fact]
        public void Constructor_Default_InitializesCollections()
        {
            // Act
            var options = new QueryOptions();

            // Assert
            Assert.NotNull(options.Properties);
            Assert.NotNull(options.PreparedStatements);
            Assert.NotNull(options.ClientTags);
            Assert.Empty(options.Properties);
            Assert.Empty(options.PreparedStatements);
            Assert.Empty(options.ClientTags);
            Assert.Equal(string.Empty, options.TransactionId);
        }

        [Fact]
        public void Properties_ValidProperties_SetsValue()
        {
            // Arrange
            var options = new QueryOptions();
            var properties = new Dictionary<string, string>
            {
                { "property1", "value1" },
                { "property2", "value2" }
            };

            // Act
            options.Properties = properties;

            // Assert
            Assert.Equal(properties, options.Properties);
        }

        [Fact]
        public void Properties_WithEmptyKey_ThrowsException()
        {
            // Arrange
            var options = new QueryOptions();
            var properties = new Dictionary<string, string>
            {
                { "", "value1" }
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => options.Properties = properties);
        }

        [Fact]
        public void Properties_WithEqualsInKey_ThrowsException()
        {
            // Arrange
            var options = new QueryOptions();
            var properties = new Dictionary<string, string>
            {
                { "key=value", "value1" }
            };

            // Act & Assert
            Assert.Throws<FormatException>(() => options.Properties = properties);
        }

        [Fact]
        public void ClientTags_ValidTags_SetsValue()
        {
            // Arrange
            var options = new QueryOptions();
            var tags = new HashSet<string> { "tag1", "tag2" };

            // Act
            options.ClientTags = tags;

            // Assert
            Assert.Equal(tags, options.ClientTags);
        }

        [Fact]
        public void ClientTags_WithComma_ThrowsException()
        {
            // Arrange
            var options = new QueryOptions();
            var tags = new HashSet<string> { "tag,with,comma" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => options.ClientTags = tags);
        }

        [Fact]
        public void PreparedStatements_ValidStatements_SetsValue()
        {
            // Arrange
            var options = new QueryOptions();
            var statements = new Dictionary<string, string>
            {
                { "stmt1", "SELECT * FROM table1" },
                { "stmt2", "SELECT * FROM table2" }
            };

            // Act
            options.PreparedStatements = statements;

            // Assert
            Assert.Equal(statements, options.PreparedStatements);
        }

        [Fact]
        public void TransactionId_ValidValue_SetsValue()
        {
            // Arrange
            var options = new QueryOptions();
            var transactionId = "transaction_12345";

            // Act
            options.TransactionId = transactionId;

            // Assert
            Assert.Equal(transactionId, options.TransactionId);
        }
    }
}
