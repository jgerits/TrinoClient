using Xunit;
using TrinoClient.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Tests;

public class TrinoDbConnectionTests
{
    [Fact]
    public void Constructor_WithConnectionString_ParsesCorrectly()
    {
        // Arrange
        var connectionString = "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=testuser";

        // Act
        var connection = new TrinoDbConnection(connectionString);

        // Assert
        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.Equal("default", connection.Database);
        Assert.Equal("localhost:8080", connection.DataSource);
    }

    [Fact]
    public void ConnectionString_CanBeSet()
    {
        // Arrange
        var connection = new TrinoDbConnection();
        var connectionString = "Host=trino.example.com;Port=443;Catalog=iceberg;Schema=production;SSL=true;User=admin";

        // Act
        connection.ConnectionString = connectionString;

        // Assert
        Assert.Equal(connectionString, connection.ConnectionString);
        Assert.Equal("production", connection.Database);
        Assert.Equal("trino.example.com:443", connection.DataSource);
    }

    [Fact]
    public void CreateCommand_ReturnsTrinoDbCommand()
    {
        // Arrange
        var connection = new TrinoDbConnection("Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=test");

        // Act
        var command = connection.CreateCommand();

        // Assert
        Assert.IsType<TrinoDbCommand>(command);
        Assert.Same(connection, command.Connection);
    }

    [Fact]
    public void BeginTransaction_ThrowsNotSupportedException()
    {
        // Arrange
        var connection = new TrinoDbConnection("Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=test");

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => connection.BeginTransaction());
    }
}
