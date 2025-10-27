using System;
using Xunit;
using TrinoClient.Model;
using TrinoClient.Model.Statement;

namespace TrinoClient.UnitTests
{
    public class ExecuteQueryV2RequestTests
    {
        [Fact]
        public void Constructor_ValidQuery_CreatesRequest()
        {
            // Arrange
            var query = "SELECT * FROM table";

            // Act
            var request = new ExecuteQueryV2Request(query);

            // Assert
            Assert.Equal(query, request.Query);
            Assert.Equal(StatementApiVersion.V2, request.ApiVersion);
        }

        [Fact]
        public void Constructor_QueryWithSemicolon_TrimsSemicolon()
        {
            // Arrange
            var query = "SELECT * FROM table;";

            // Act
            var request = new ExecuteQueryV2Request(query);

            // Assert
            Assert.Equal("SELECT * FROM table", request.Query);
        }

        [Fact]
        public void Constructor_QueryWithMultipleSemicolons_TrimsAllSemicolons()
        {
            // Arrange
            var query = "SELECT * FROM table;;;";

            // Act
            var request = new ExecuteQueryV2Request(query);

            // Assert
            Assert.Equal("SELECT * FROM table", request.Query);
        }

        [Fact]
        public void Constructor_NullQuery_ThrowsException()
        {
            // Arrange
            string query = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExecuteQueryV2Request(query));
        }

        [Fact]
        public void Constructor_EmptyQuery_ThrowsException()
        {
            // Arrange
            var query = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExecuteQueryV2Request(query));
        }

        [Fact]
        public void Constructor_WithOptions_SetsOptions()
        {
            // Arrange
            var query = "SELECT * FROM table";
            var options = new QueryOptions();

            // Act
            var request = new ExecuteQueryV2Request(query, options);

            // Assert
            Assert.Equal(query, request.Query);
            Assert.Equal(options, request.Options);
        }

        [Fact]
        public void Options_CanBeSetAfterCreation()
        {
            // Arrange
            var query = "SELECT * FROM table";
            var request = new ExecuteQueryV2Request(query);
            var options = new QueryOptions();

            // Act
            request.Options = options;

            // Assert
            Assert.Equal(options, request.Options);
        }

        [Fact]
        public void ApiVersion_AlwaysReturnsV2()
        {
            // Arrange
            var query = "SELECT 1";
            var request = new ExecuteQueryV2Request(query);

            // Act & Assert
            Assert.Equal(StatementApiVersion.V2, request.ApiVersion);
        }
    }
}
