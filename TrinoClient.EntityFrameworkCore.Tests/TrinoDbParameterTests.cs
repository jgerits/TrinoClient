using Xunit;
using TrinoClient.EntityFrameworkCore.Storage;
using System.Data;

namespace TrinoClient.EntityFrameworkCore.Tests;

public class TrinoDbParameterTests
{
    [Fact]
    public void ParameterName_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameter = new TrinoDbParameter();
        var name = "@productId";

        // Act
        parameter.ParameterName = name;

        // Assert
        Assert.Equal(name, parameter.ParameterName);
    }

    [Fact]
    public void Value_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameter = new TrinoDbParameter();
        var value = 42;

        // Act
        parameter.Value = value;

        // Assert
        Assert.Equal(value, parameter.Value);
    }

    [Fact]
    public void DbType_CanBeSetAndRetrieved()
    {
        // Arrange
        var parameter = new TrinoDbParameter();

        // Act
        parameter.DbType = DbType.Int32;

        // Assert
        Assert.Equal(DbType.Int32, parameter.DbType);
    }

    [Fact]
    public void Direction_DefaultsToInput()
    {
        // Arrange & Act
        var parameter = new TrinoDbParameter();

        // Assert
        Assert.Equal(ParameterDirection.Input, parameter.Direction);
    }
}
