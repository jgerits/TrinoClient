using System;
using Xunit;
using TrinoClient.Model.Transaction;

namespace TrinoClient.UnitTests
{
    public class TransactionIdTests
    {
        [Fact]
        public void Constructor_ValidGuid_CreatesInstance()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var transactionId = new TransactionId(guid);

            // Assert
            Assert.Equal(guid, transactionId.UUID);
        }

        [Fact]
        public void Constructor_ValidString_CreatesInstance()
        {
            // Arrange
            var guidString = "123e4567-e89b-12d3-a456-426614174000";

            // Act
            var transactionId = new TransactionId(guidString);

            // Assert
            Assert.Equal(Guid.Parse(guidString), transactionId.UUID);
        }

        [Fact]
        public void Constructor_InvalidString_ThrowsException()
        {
            // Arrange
            var invalidString = "not-a-guid";

            // Act & Assert
            Assert.Throws<FormatException>(() => new TransactionId(invalidString));
        }

        [Fact]
        public void Create_GeneratesNewTransactionId()
        {
            // Act
            var transactionId = TransactionId.Create();

            // Assert
            Assert.NotNull(transactionId);
            Assert.NotEqual(Guid.Empty, transactionId.UUID);
        }

        [Fact]
        public void Create_GeneratesUniqueIds()
        {
            // Act
            var transactionId1 = TransactionId.Create();
            var transactionId2 = TransactionId.Create();

            // Assert
            Assert.NotEqual(transactionId1.UUID, transactionId2.UUID);
        }

        [Fact]
        public void ValueOf_ValidString_CreatesTransactionId()
        {
            // Arrange
            var guidString = "123e4567-e89b-12d3-a456-426614174000";

            // Act
            var transactionId = TransactionId.ValueOf(guidString);

            // Assert
            Assert.Equal(Guid.Parse(guidString), transactionId.UUID);
        }

        [Fact]
        public void ValueOf_InvalidString_ThrowsException()
        {
            // Arrange
            var invalidString = "invalid-guid-format";

            // Act & Assert
            Assert.Throws<FormatException>(() => TransactionId.ValueOf(invalidString));
        }

        [Fact]
        public void ToString_ReturnsGuidString()
        {
            // Arrange
            var guid = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
            var transactionId = new TransactionId(guid);

            // Act
            var result = transactionId.ToString();

            // Assert
            Assert.Equal(guid.ToString(), result);
        }

        [Fact]
        public void Equals_SameGuid_ReturnsTrue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var transactionId1 = new TransactionId(guid);
            var transactionId2 = new TransactionId(guid);

            // Act
            var result = transactionId1.Equals(transactionId2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentGuid_ReturnsFalse()
        {
            // Arrange
            var transactionId1 = new TransactionId(Guid.NewGuid());
            var transactionId2 = new TransactionId(Guid.NewGuid());

            // Act
            var result = transactionId1.Equals(transactionId2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_Null_ReturnsFalse()
        {
            // Arrange
            var transactionId = new TransactionId(Guid.NewGuid());

            // Act
            var result = transactionId.Equals(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_SameObject_ReturnsTrue()
        {
            // Arrange
            var transactionId = new TransactionId(Guid.NewGuid());

            // Act
            var result = transactionId.Equals(transactionId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetHashCode_SameGuid_ReturnsSameHashCode()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var transactionId1 = new TransactionId(guid);
            var transactionId2 = new TransactionId(guid);

            // Act
            var hashCode1 = transactionId1.GetHashCode();
            var hashCode2 = transactionId2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Equals_DifferentType_ReturnsFalse()
        {
            // Arrange
            var transactionId = new TransactionId(Guid.NewGuid());
            var otherObject = "not a transaction id";

            // Act
            var result = transactionId.Equals(otherObject);

            // Assert
            Assert.False(result);
        }
    }
}
