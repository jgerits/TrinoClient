using System;
using Xunit;
using TrinoClient.Model.SPI;

namespace TrinoClient.UnitTests
{
    public class ErrorCodeTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            var code = 1;
            var name = "SYNTAX_ERROR";
            var type = ErrorType.USER_ERROR;

            // Act
            var errorCode = new ErrorCode(code, name, type);

            // Assert
            Assert.Equal(code, errorCode.Code);
            Assert.Equal(name, errorCode.Name);
            Assert.Equal(type, errorCode.Type);
        }

        [Fact]
        public void Constructor_NegativeCode_ThrowsException()
        {
            // Arrange
            var code = -1;
            var name = "ERROR";
            var type = ErrorType.INTERNAL_ERROR;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ErrorCode(code, name, type));
        }

        [Fact]
        public void Constructor_NullName_ThrowsException()
        {
            // Arrange
            var code = 1;
            string name = null;
            var type = ErrorType.USER_ERROR;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ErrorCode(code, name, type));
        }

        [Fact]
        public void Constructor_EmptyName_ThrowsException()
        {
            // Arrange
            var code = 1;
            var name = string.Empty;
            var type = ErrorType.USER_ERROR;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ErrorCode(code, name, type));
        }

        [Fact]
        public void Constructor_CodeZero_CreatesInstance()
        {
            // Arrange
            var code = 0;
            var name = "GENERIC_ERROR";
            var type = ErrorType.INTERNAL_ERROR;

            // Act
            var errorCode = new ErrorCode(code, name, type);

            // Assert
            Assert.Equal(code, errorCode.Code);
            Assert.Equal(name, errorCode.Name);
            Assert.Equal(type, errorCode.Type);
        }

        [Fact]
        public void ToString_FormatsCorrectly()
        {
            // Arrange
            var code = 42;
            var name = "CUSTOM_ERROR";
            var type = ErrorType.EXTERNAL;
            var errorCode = new ErrorCode(code, name, type);

            // Act
            var result = errorCode.ToString();

            // Assert
            Assert.Equal("CUSTOM_ERROR:42", result);
        }

        [Fact]
        public void Constructor_DifferentErrorTypes_CreatesInstances()
        {
            // Arrange & Act
            var userError = new ErrorCode(1, "USER_ERROR", ErrorType.USER_ERROR);
            var internalError = new ErrorCode(2, "INTERNAL_ERROR", ErrorType.INTERNAL_ERROR);
            var externalError = new ErrorCode(3, "EXTERNAL_ERROR", ErrorType.EXTERNAL);
            var resourceError = new ErrorCode(4, "RESOURCE_ERROR", ErrorType.INSUFFICIENT_RESOURCES);

            // Assert
            Assert.Equal(ErrorType.USER_ERROR, userError.Type);
            Assert.Equal(ErrorType.INTERNAL_ERROR, internalError.Type);
            Assert.Equal(ErrorType.EXTERNAL, externalError.Type);
            Assert.Equal(ErrorType.INSUFFICIENT_RESOURCES, resourceError.Type);
        }

        [Fact]
        public void Constructor_LargeCode_CreatesInstance()
        {
            // Arrange
            var code = 999999;
            var name = "LARGE_CODE_ERROR";
            var type = ErrorType.INTERNAL_ERROR;

            // Act
            var errorCode = new ErrorCode(code, name, type);

            // Assert
            Assert.Equal(code, errorCode.Code);
            Assert.Equal(name, errorCode.Name);
            Assert.Equal(type, errorCode.Type);
        }
    }
}
