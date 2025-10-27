using System;
using Xunit;
using TrinoClient.Model.SPI;

namespace TrinoClient.UnitTests
{
    public class QueryIdTests
    {
        [Fact]
        public void Constructor_ValidId_CreatesInstance()
        {
            // Arrange
            var id = "test_query_123";

            // Act
            var queryId = new QueryId(id);

            // Assert
            Assert.Equal(id, queryId.Id);
        }

        [Fact]
        public void Constructor_ValidLowerCaseId_CreatesInstance()
        {
            // Arrange
            var id = "queryid123";

            // Act
            var queryId = new QueryId(id);

            // Assert
            Assert.Equal(id, queryId.Id);
        }

        [Fact]
        public void Constructor_NullId_ThrowsException()
        {
            // Arrange
            string id = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new QueryId(id));
        }

        [Fact]
        public void Constructor_EmptyId_ThrowsException()
        {
            // Arrange
            var id = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new QueryId(id));
        }

        [Fact]
        public void ValidateId_ValidId_ReturnsTrue()
        {
            // Arrange
            var id = "test_query_123";

            // Act
            var result = QueryId.ValidateId(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateId_ValidLowerCaseId_ReturnsTrue()
        {
            // Arrange
            var id = "validqueryid";

            // Act
            var result = QueryId.ValidateId(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateId_NullId_ThrowsException()
        {
            // Arrange
            string id = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => QueryId.ValidateId(id));
        }

        [Fact]
        public void ToString_ReturnsId()
        {
            // Arrange
            var id = "test_query_123";
            var queryId = new QueryId(id);

            // Act
            var result = queryId.ToString();

            // Assert
            Assert.Equal(id, result);
        }

        [Fact]
        public void Constructor_IdWithNumbers_CreatesInstance()
        {
            // Arrange
            var id = "123456";

            // Act
            var queryId = new QueryId(id);

            // Assert
            Assert.Equal(id, queryId.Id);
        }

        [Fact]
        public void Constructor_IdWithUnderscores_CreatesInstance()
        {
            // Arrange
            var id = "query_with_underscores";

            // Act
            var queryId = new QueryId(id);

            // Assert
            Assert.Equal(id, queryId.Id);
        }
    }
}
