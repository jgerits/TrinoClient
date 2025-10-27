using System;
using Xunit;
using TrinoClient.Model.Client;

namespace TrinoClient.UnitTests
{
    public class ColumnTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            var name = "column1";
            var type = "varchar";

            // Act
            var column = new Column(name, type, null);

            // Assert
            Assert.Equal(name, column.Name);
            Assert.Equal(type, column.Type);
            Assert.Null(column.TypeSignature);
        }

        [Fact]
        public void Constructor_NullName_ThrowsException()
        {
            // Arrange
            string name = null;
            var type = "varchar";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Column(name, type, null));
        }

        [Fact]
        public void Constructor_EmptyName_ThrowsException()
        {
            // Arrange
            var name = string.Empty;
            var type = "varchar";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Column(name, type, null));
        }

        [Fact]
        public void Constructor_NullType_ThrowsException()
        {
            // Arrange
            var name = "column1";
            string type = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Column(name, type, null));
        }

        [Fact]
        public void Constructor_EmptyType_ThrowsException()
        {
            // Arrange
            var name = "column1";
            var type = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Column(name, type, null));
        }

        [Fact]
        public void Constructor_CommonTypes_CreateInstances()
        {
            // Test various common SQL types
            var types = new[] { "bigint", "integer", "double", "varchar", "boolean", "date", "timestamp" };

            foreach (var type in types)
            {
                // Act
                var column = new Column("test_column", type, null);

                // Assert
                Assert.Equal("test_column", column.Name);
                Assert.Equal(type, column.Type);
            }
        }
    }
}
