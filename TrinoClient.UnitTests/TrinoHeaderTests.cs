using Xunit;
using TrinoClient;

namespace TrinoClient.UnitTests
{
    public class TrinoHeaderTests
    {
        [Fact]
        public void TrinoHeader_StaticHeaders_HaveCorrectValues()
        {
            // Assert
            Assert.Equal("X-Trino-User", TrinoHeader.TRINO_USER.Value);
            Assert.Equal("X-Trino-Source", TrinoHeader.TRINO_SOURCE.Value);
            Assert.Equal("X-Trino-Catalog", TrinoHeader.TRINO_CATALOG.Value);
            Assert.Equal("X-Trino-Schema", TrinoHeader.TRINO_SCHEMA.Value);
            Assert.Equal("X-Trino-Time-Zone", TrinoHeader.TRINO_TIME_ZONE.Value);
            Assert.Equal("X-Trino-Language", TrinoHeader.TRINO_LANGUAGE.Value);
            Assert.Equal("X-Trino-Session", TrinoHeader.TRINO_SESSION.Value);
            Assert.Equal("X-Trino-Set-Catalog", TrinoHeader.TRINO_SET_CATALOG.Value);
            Assert.Equal("X-Trino-Set-Schema", TrinoHeader.TRINO_SET_SCHEMA.Value);
            Assert.Equal("X-Trino-Set-Session", TrinoHeader.TRINO_SET_SESSION.Value);
            Assert.Equal("X-Trino-Clear-Session", TrinoHeader.TRINO_CLEAR_SESSION.Value);
            Assert.Equal("X-Trino-Prepared-Statement", TrinoHeader.TRINO_PREPARED_STATEMENT.Value);
            Assert.Equal("X-Trino-Added-Prepare", TrinoHeader.TRINO_ADDED_PREPARE.Value);
            Assert.Equal("X-Trino-Deallocated-Prepare", TrinoHeader.TRINO_DEALLOCATED_PREPARE.Value);
            Assert.Equal("X-Trino-Transaction-Id", TrinoHeader.TRINO_TRANSACTION_ID.Value);
            Assert.Equal("X-Trino-Started-Transaction-Id", TrinoHeader.TRINO_STARTED_TRANSACTION_ID.Value);
            Assert.Equal("X-Trino-Clear-Transaction-Id", TrinoHeader.TRINO_CLEAR_TRANSACTION_ID.Value);
            Assert.Equal("X-Trino-Client-Info", TrinoHeader.TRINO_CLIENT_INFO.Value);
            Assert.Equal("X-Trino-Client-Tags", TrinoHeader.TRINO_CLIENT_TAGS.Value);
            Assert.Equal("X-Trino-Current-State", TrinoHeader.TRINO_CURRENT_STATE.Value);
            Assert.Equal("X-Trino-Max-Wait", TrinoHeader.TRINO_MAX_WAIT.Value);
            Assert.Equal("X-Trino-Max-Size", TrinoHeader.TRINO_MAX_SIZE.Value);
            Assert.Equal("X-Trino-Task-Instance-Id", TrinoHeader.TRINO_TASK_INSTANCE_ID.Value);
            Assert.Equal("X-Trino-Page-Sequence-Id", TrinoHeader.TRINO_PAGE_TOKEN.Value);
            Assert.Equal("X-Trino-Page-End-Sequence-Id", TrinoHeader.TRINO_PAGE_NEXT_TOKEN.Value);
            Assert.Equal("X-Trino-Buffer-Complete", TrinoHeader.TRINO_BUFFER_COMPLETE.Value);
            Assert.Equal("X-Trino-Data-Next-Uri", TrinoHeader.TRINO_DATA_NEXT_URI.Value);
        }

        [Fact]
        public void TrinoHeader_ToString_ReturnsValue()
        {
            // Arrange
            var header = TrinoHeader.TRINO_USER;

            // Act
            var result = header.ToString();

            // Assert
            Assert.Equal("X-Trino-User", result);
        }

        [Fact]
        public void TrinoHeader_AllHeaders_AreNotNull()
        {
            // Assert
            Assert.NotNull(TrinoHeader.TRINO_USER);
            Assert.NotNull(TrinoHeader.TRINO_SOURCE);
            Assert.NotNull(TrinoHeader.TRINO_CATALOG);
            Assert.NotNull(TrinoHeader.TRINO_SCHEMA);
            Assert.NotNull(TrinoHeader.TRINO_TIME_ZONE);
            Assert.NotNull(TrinoHeader.TRINO_LANGUAGE);
            Assert.NotNull(TrinoHeader.TRINO_SESSION);
            Assert.NotNull(TrinoHeader.TRINO_CLIENT_INFO);
            Assert.NotNull(TrinoHeader.TRINO_CLIENT_TAGS);
        }
    }
}
