using Xunit;
using TrinoClient.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Tests;

public class TrinoDbCommandTests
{
    [Fact]
    public void Constructor_WithConnectionAndCommandText_SetsProperties()
    {
        // Arrange
        var connection = new TrinoDbConnection("Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=test");
        var commandText = "SELECT * FROM products";

        // Act
        var command = new TrinoDbCommand(commandText, connection);

        // Assert
        Assert.Equal(commandText, command.CommandText);
        Assert.Same(connection, command.Connection);
    }

    [Fact]
    public void CreateParameter_ReturnsTrinoDbParameter()
    {
        // Arrange
        var command = new TrinoDbCommand();

        // Act
        var parameter = command.CreateParameter();

        // Assert
        Assert.IsType<TrinoDbParameter>(parameter);
    }

    [Fact]
    public void CommandText_CanBeSetAndRetrieved()
    {
        // Arrange
        var command = new TrinoDbCommand();
        var sql = "SELECT name, price FROM products WHERE price > 100";

        // Act
        command.CommandText = sql;

        // Assert
        Assert.Equal(sql, command.CommandText);
    }

    [Fact]
    public void Parameters_ReturnsCollection()
    {
        // Arrange
        var command = new TrinoDbCommand();

        // Act
        var parameters = command.Parameters;

        // Assert
        Assert.NotNull(parameters);
        Assert.IsType<TrinoDbParameterCollection>(parameters);
    }
}
