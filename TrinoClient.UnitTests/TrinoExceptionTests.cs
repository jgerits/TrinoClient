using System;
using System.Net;
using Xunit;
using TrinoClient.Model;

namespace TrinoClient.UnitTests
{
    public class TrinoExceptionTests
    {
        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            // Arrange
            var message = "Test exception message";

            // Act
            var exception = new TrinoException(message);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(string.Empty, exception.RawResponseContent);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsBoth()
        {
            // Arrange
            var message = "Test exception message";
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new TrinoException(message, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(string.Empty, exception.RawResponseContent);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageAndRawContent_SetsBoth()
        {
            // Arrange
            var message = "Test exception message";
            var rawContent = "Raw response content";

            // Act
            var exception = new TrinoException(message, rawContent);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(rawContent, exception.RawResponseContent);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsAllProperties()
        {
            // Arrange
            var message = "Test exception message";
            var rawContent = "Raw response content";
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new TrinoException(message, rawContent, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(rawContent, exception.RawResponseContent);
            Assert.Equal(innerException, exception.InnerException);
        }
    }

    public class TrinoWebExceptionTests
    {
        [Fact]
        public void Constructor_WithMessageAndStatusCode_SetsBoth()
        {
            // Arrange
            var message = "Test web exception";
            var statusCode = HttpStatusCode.BadRequest;

            // Act
            var exception = new TrinoWebException(message, statusCode);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(string.Empty, exception.RawResponseContent);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageStatusCodeAndInnerException_SetsAll()
        {
            // Arrange
            var message = "Test web exception";
            var statusCode = HttpStatusCode.InternalServerError;
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new TrinoWebException(message, statusCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(string.Empty, exception.RawResponseContent);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageRawContentAndStatusCode_SetsAll()
        {
            // Arrange
            var message = "Test web exception";
            var rawContent = "Raw response content";
            var statusCode = HttpStatusCode.NotFound;

            // Act
            var exception = new TrinoWebException(message, rawContent, statusCode);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(rawContent, exception.RawResponseContent);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithAllParameters_SetsAllProperties()
        {
            // Arrange
            var message = "Test web exception";
            var rawContent = "Raw response content";
            var statusCode = HttpStatusCode.Unauthorized;
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new TrinoWebException(message, rawContent, statusCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(rawContent, exception.RawResponseContent);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void TrinoWebException_InheritsFromTrinoException()
        {
            // Arrange & Act
            var exception = new TrinoWebException("Test", HttpStatusCode.OK);

            // Assert
            Assert.IsAssignableFrom<TrinoException>(exception);
        }
    }
}
