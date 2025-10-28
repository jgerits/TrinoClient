using System.Data;
using System.Data.Common;
using TrinoClient.Model.Statement;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbConnection implementation for Trino
/// </summary>
public class TrinoDbConnection : DbConnection
{
    private readonly TrinoClientSessionConfig _config;
    private TrinodbClient? _trinoClient;
    private ConnectionState _state = ConnectionState.Closed;
    private string _connectionString = string.Empty;

    public TrinoDbConnection()
    {
        _config = new TrinoClientSessionConfig("default", "default");
    }

    public TrinoDbConnection(string connectionString)
    {
        _connectionString = connectionString;
        _config = ParseConnectionString(connectionString);
    }

    public TrinoDbConnection(TrinoClientSessionConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _connectionString = BuildConnectionString(config);
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        // Trino doesn't support traditional transactions
        throw new NotSupportedException("Trino does not support transactions");
    }

    public override void ChangeDatabase(string databaseName)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Database name cannot be null or empty", nameof(databaseName));

        _config.Schema = databaseName;
        
        // If connection is open, recreate the client
        if (_state == ConnectionState.Open)
        {
            Close();
            Open();
        }
    }

    public override void Close()
    {
        if (_state == ConnectionState.Closed)
            return;

        _trinoClient?.Dispose();
        _trinoClient = null;
        _state = ConnectionState.Closed;
        OnStateChange(new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Closed));
    }

    public override void Open()
    {
        if (_state == ConnectionState.Open)
            return;

        try
        {
            _trinoClient = new TrinodbClient(_config);
            _state = ConnectionState.Open;
            OnStateChange(new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Open));
        }
        catch (Exception ex)
        {
            _state = ConnectionState.Broken;
            throw new InvalidOperationException("Failed to open connection to Trino", ex);
        }
    }

    public override string? ConnectionString
    {
        get => _connectionString;
        set
        {
            if (_state == ConnectionState.Open)
                throw new InvalidOperationException("Cannot change connection string while connection is open");

            _connectionString = value ?? throw new ArgumentNullException(nameof(value));
            var newConfig = ParseConnectionString(value);
            
            // Update config properties
            _config.Host = newConfig.Host;
            _config.Port = newConfig.Port;
            _config.Catalog = newConfig.Catalog;
            _config.Schema = newConfig.Schema;
            _config.UseSsl = newConfig.UseSsl;
            _config.User = newConfig.User;
        }
    }

    public override string Database => _config.Schema;

    public override ConnectionState State => _state;

    public override string DataSource => $"{_config.Host}:{_config.Port}";

    public override string ServerVersion => "Trino";

    protected override DbCommand CreateDbCommand()
    {
        return new TrinoDbCommand(this);
    }

    internal TrinodbClient GetTrinoClient()
    {
        if (_trinoClient == null)
            throw new InvalidOperationException("Connection is not open");
        return _trinoClient;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }
        base.Dispose(disposing);
    }

    private static TrinoClientSessionConfig ParseConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));

        var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split('=', 2))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0].Trim().ToLowerInvariant(), p => p[1].Trim());

        var host = parts.GetValueOrDefault("host", "localhost");
        
        var portString = parts.GetValueOrDefault("port", "8080");
        if (!int.TryParse(portString, out var port) || port <= 0 || port > 65535)
            throw new ArgumentException($"Invalid port value '{portString}'. Port must be between 1 and 65535.", nameof(connectionString));
        
        var catalog = parts.GetValueOrDefault("catalog", "default");
        var schema = parts.GetValueOrDefault("schema", "default");
        
        var sslString = parts.GetValueOrDefault("ssl", "false");
        if (!bool.TryParse(sslString, out var useSsl))
            throw new ArgumentException($"Invalid SSL value '{sslString}'. Must be 'true' or 'false'.", nameof(connectionString));
        
        var user = parts.GetValueOrDefault("user", Environment.UserName);

        var config = new TrinoClientSessionConfig(catalog, schema)
        {
            Host = host,
            Port = port,
            UseSsl = useSsl,
            User = user
        };

        return config;
    }

    private static string BuildConnectionString(TrinoClientSessionConfig config)
    {
        return $"Host={config.Host};Port={config.Port};Catalog={config.Catalog};Schema={config.Schema};SSL={config.UseSsl};User={config.User ?? Environment.UserName}";
    }
}
