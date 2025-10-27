using System;
using Xunit;
using TrinoClient.Model;

namespace TrinoClient.UnitTests
{
    public class DataSizeTests
    {
        [Fact]
        public void Constructor_ValidParameters_CreatesInstance()
        {
            // Arrange
            var size = 1024.0;
            var unit = DataSizeUnit.BYTE;

            // Act
            var dataSize = new DataSize(size, unit);

            // Assert
            Assert.Equal(size, dataSize.Size);
            Assert.Equal(unit, dataSize.Unit);
        }

        [Fact]
        public void Constructor_InfinitySize_ThrowsException()
        {
            // Arrange
            var size = double.PositiveInfinity;
            var unit = DataSizeUnit.BYTE;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new DataSize(size, unit));
        }

        [Fact]
        public void Constructor_NaNSize_ThrowsException()
        {
            // Arrange
            var size = double.NaN;
            var unit = DataSizeUnit.BYTE;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new DataSize(size, unit));
        }

        [Fact]
        public void Constructor_NegativeSize_ThrowsException()
        {
            // Arrange
            var size = -1.0;
            var unit = DataSizeUnit.BYTE;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new DataSize(size, unit));
        }

        [Fact]
        public void SuccinctBytes_ValidBytes_CreatesSuccinctDataSize()
        {
            // Arrange
            long bytes = 1024;

            // Act
            var dataSize = DataSize.SuccinctBytes(bytes);

            // Assert
            Assert.NotNull(dataSize);
            Assert.Equal(1.0, dataSize.Size);
            Assert.Equal(DataSizeUnit.KILOBYTE, dataSize.Unit);
        }

        [Fact]
        public void ConvertTo_DifferentUnit_ConvertsCorrectly()
        {
            // Arrange
            var dataSize = new DataSize(1, DataSizeUnit.KILOBYTE);

            // Act
            var convertedSize = dataSize.ConvertTo(DataSizeUnit.BYTE);

            // Assert
            Assert.Equal(1024, convertedSize.Size);
            Assert.Equal(DataSizeUnit.BYTE, convertedSize.Unit);
        }

        [Fact]
        public void GetValue_DifferentUnit_ReturnsCorrectValue()
        {
            // Arrange
            var dataSize = new DataSize(1, DataSizeUnit.MEGABYTE);

            // Act
            var kilobyteValue = dataSize.GetValue(DataSizeUnit.KILOBYTE);

            // Assert
            Assert.Equal(1024, kilobyteValue);
        }

        [Fact]
        public void ToBytes_ValidSize_ReturnsBytes()
        {
            // Arrange
            var dataSize = new DataSize(1, DataSizeUnit.KILOBYTE);

            // Act
            var bytes = dataSize.ToBytes();

            // Assert
            Assert.Equal(1024, bytes);
        }

        [Fact]
        public void CompareTo_SmallerSize_ReturnsNegative()
        {
            // Arrange
            var size1 = new DataSize(1, DataSizeUnit.KILOBYTE);
            var size2 = new DataSize(1, DataSizeUnit.MEGABYTE);

            // Act
            var result = size1.CompareTo(size2);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void CompareTo_LargerSize_ReturnsPositive()
        {
            // Arrange
            var size1 = new DataSize(1, DataSizeUnit.MEGABYTE);
            var size2 = new DataSize(1, DataSizeUnit.KILOBYTE);

            // Act
            var result = size1.CompareTo(size2);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_EqualSize_ReturnsZero()
        {
            // Arrange
            var size1 = new DataSize(1024, DataSizeUnit.BYTE);
            var size2 = new DataSize(1, DataSizeUnit.KILOBYTE);

            // Act
            var result = size1.CompareTo(size2);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConvertToMostSuccinctDataSize_LargeValue_ConvertsToLargerUnit()
        {
            // Arrange
            var dataSize = new DataSize(2048, DataSizeUnit.BYTE);

            // Act
            var succinct = dataSize.ConvertToMostSuccinctDataSize();

            // Assert
            Assert.Equal(DataSizeUnit.KILOBYTE, succinct.Unit);
            Assert.Equal(2.0, succinct.Size);
        }

        [Fact]
        public void ConvertToMostSuccinctDataSize_SmallValue_KeepsSmallUnit()
        {
            // Arrange
            var dataSize = new DataSize(512, DataSizeUnit.BYTE);

            // Act
            var succinct = dataSize.ConvertToMostSuccinctDataSize();

            // Assert
            Assert.Equal(DataSizeUnit.BYTE, succinct.Unit);
            Assert.Equal(512, succinct.Size);
        }
    }
}
