using System.Collections.Generic;
using Xunit;
using TrinoClient.Model.Client;

namespace TrinoClient.UnitTests
{
    public class QueryErrorTests
    {
        [Fact]
        public void Constructor_AllParameters_CreatesInstance()
        {
            // Arrange
            var message = "Test error message";
            var sqlState = "42000";
            var errorCode = 1;
            var errorName = "SYNTAX_ERROR";
            var errorType = "USER_ERROR";
            var errorLocation = new ErrorLocation(1, 1);
            var failureInfo = new FailureInfo("TestType", "Test", null, new List<FailureInfo>(), null, null);

            // Act
            var queryError = new QueryError(message, sqlState, errorCode, errorName, errorType, errorLocation, failureInfo);

            // Assert
            Assert.Equal(message, queryError.Message);
            Assert.Equal(sqlState, queryError.SqlState);
            Assert.Equal(errorCode, queryError.ErrorCode);
            Assert.Equal(errorName, queryError.ErrorName);
            Assert.Equal(errorType, queryError.ErrorType);
            Assert.Equal(errorLocation, queryError.ErrorLocation);
            Assert.Equal(failureInfo, queryError.FailureInfo);
        }

        [Fact]
        public void Constructor_NullSqlState_CreatesInstance()
        {
            // Arrange
            var message = "Test error message";
            string sqlState = null;
            var errorCode = 1;
            var errorName = "SYNTAX_ERROR";
            var errorType = "USER_ERROR";
            var errorLocation = new ErrorLocation(1, 1);
            var failureInfo = new FailureInfo("TestType", "Test", null, new List<FailureInfo>(), null, null);

            // Act
            var queryError = new QueryError(message, sqlState, errorCode, errorName, errorType, errorLocation, failureInfo);

            // Assert
            Assert.Equal(message, queryError.Message);
            Assert.Null(queryError.SqlState);
            Assert.Equal(errorCode, queryError.ErrorCode);
            Assert.Equal(errorName, queryError.ErrorName);
            Assert.Equal(errorType, queryError.ErrorType);
        }

        [Fact]
        public void Constructor_MinimalParameters_CreatesInstance()
        {
            // Arrange
            var message = "Error";
            var sqlState = "00000";
            var errorCode = 0;
            var errorName = "UNKNOWN";
            var errorType = "INTERNAL_ERROR";
            ErrorLocation errorLocation = null;
            FailureInfo failureInfo = null;

            // Act
            var queryError = new QueryError(message, sqlState, errorCode, errorName, errorType, errorLocation, failureInfo);

            // Assert
            Assert.Equal(message, queryError.Message);
            Assert.Equal(sqlState, queryError.SqlState);
            Assert.Equal(errorCode, queryError.ErrorCode);
            Assert.Equal(errorName, queryError.ErrorName);
            Assert.Equal(errorType, queryError.ErrorType);
            Assert.Null(queryError.ErrorLocation);
            Assert.Null(queryError.FailureInfo);
        }
    }
}
