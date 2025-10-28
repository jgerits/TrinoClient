using Xunit;
using TrinoClient.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Tests;

public class TrinoDbParameterCollectionTests
{
    [Fact]
    public void Add_AddsParameter()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();
        var parameter = new TrinoDbParameter { ParameterName = "@test", Value = 123 };

        // Act
        var index = collection.Add(parameter);

        // Assert
        Assert.Equal(0, index);
        Assert.Equal(1, collection.Count);
        Assert.Same(parameter, collection[0]);
    }

    [Fact]
    public void Contains_ReturnsTrueForExistingParameter()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();
        var parameter = new TrinoDbParameter { ParameterName = "@test" };
        collection.Add(parameter);

        // Act
        var contains = collection.Contains("@test");

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_ReturnsFalseForNonExistingParameter()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();

        // Act
        var contains = collection.Contains("@nonexistent");

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Clear_RemovesAllParameters()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();
        collection.Add(new TrinoDbParameter { ParameterName = "@test1" });
        collection.Add(new TrinoDbParameter { ParameterName = "@test2" });

        // Act
        collection.Clear();

        // Assert
        Assert.Equal(0, collection.Count);
    }

    [Fact]
    public void IndexOf_ReturnsCorrectIndex()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();
        collection.Add(new TrinoDbParameter { ParameterName = "@first" });
        collection.Add(new TrinoDbParameter { ParameterName = "@second" });

        // Act
        var index = collection.IndexOf("@second");

        // Assert
        Assert.Equal(1, index);
    }

    [Fact]
    public void RemoveAt_RemovesParameterAtIndex()
    {
        // Arrange
        var collection = new TrinoDbParameterCollection();
        var param1 = new TrinoDbParameter { ParameterName = "@first" };
        var param2 = new TrinoDbParameter { ParameterName = "@second" };
        collection.Add(param1);
        collection.Add(param2);

        // Act
        collection.RemoveAt(0);

        // Assert
        Assert.Equal(1, collection.Count);
        Assert.Same(param2, collection[0]);
    }
}
