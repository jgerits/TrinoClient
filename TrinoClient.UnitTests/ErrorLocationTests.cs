using System;
using Xunit;
using TrinoClient.Model.Client;

namespace TrinoClient.UnitTests
{
    public class ErrorLocationTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            var lineNumber = 5;
            var columnNumber = 10;

            // Act
            var errorLocation = new ErrorLocation(lineNumber, columnNumber);

            // Assert
            Assert.Equal(lineNumber, errorLocation.LineNumber);
            Assert.Equal(columnNumber, errorLocation.ColumnNumber);
        }

        [Fact]
        public void Constructor_LineNumberZero_ThrowsException()
        {
            // Arrange
            var lineNumber = 0;
            var columnNumber = 10;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ErrorLocation(lineNumber, columnNumber));
        }

        [Fact]
        public void Constructor_LineNumberNegative_ThrowsException()
        {
            // Arrange
            var lineNumber = -1;
            var columnNumber = 10;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ErrorLocation(lineNumber, columnNumber));
        }

        [Fact]
        public void Constructor_ColumnNumberZero_ThrowsException()
        {
            // Arrange
            var lineNumber = 5;
            var columnNumber = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ErrorLocation(lineNumber, columnNumber));
        }

        [Fact]
        public void Constructor_ColumnNumberNegative_ThrowsException()
        {
            // Arrange
            var lineNumber = 5;
            var columnNumber = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ErrorLocation(lineNumber, columnNumber));
        }

        [Fact]
        public void Constructor_MinimumValidValues_CreatesInstance()
        {
            // Arrange
            var lineNumber = 1;
            var columnNumber = 1;

            // Act
            var errorLocation = new ErrorLocation(lineNumber, columnNumber);

            // Assert
            Assert.Equal(lineNumber, errorLocation.LineNumber);
            Assert.Equal(columnNumber, errorLocation.ColumnNumber);
        }

        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var errorLocation = new ErrorLocation(5, 10);

            // Act
            var result = errorLocation.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains("lineNumber", result);
            Assert.Contains("columnNumber", result);
        }
    }
}
