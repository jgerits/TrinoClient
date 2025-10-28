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

    [Fact]
    public void Constructor_WithInvalidPort_ThrowsArgumentException()
    {
        // Arrange
        var connectionString = "Host=localhost;Port=invalid;Catalog=hive;Schema=default;SSL=false;User=test";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new TrinoDbConnection(connectionString));
        Assert.Contains("Invalid port value", ex.Message);
    }

    [Fact]
    public void Constructor_WithInvalidSSL_ThrowsArgumentException()
    {
        // Arrange
        var connectionString = "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=invalid;User=test";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new TrinoDbConnection(connectionString));
        Assert.Contains("Invalid SSL value", ex.Message);
    }

    [Fact]
    public void Constructor_WithNullConnectionString_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TrinoDbConnection((string)null!));
    }

    [Fact]
    public void Constructor_WithEmptyConnectionString_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TrinoDbConnection(string.Empty));
    }
}
