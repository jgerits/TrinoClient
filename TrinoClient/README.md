# JGerits.TrinoClient

A high-performance .NET client for Trino (formerly PrestoSQL) with advanced optimizations including HTTP compression, connection pooling, HTTP/2 support, and configurable retry logic.

## Features

- ✅ **HTTP Compression**: Automatic GZip/Deflate compression reduces bandwidth by 70-90%
- ✅ **HTTP/2 Support**: Modern protocol with multiplexing and header compression
- ✅ **Connection Pooling**: Advanced connection management with configurable lifetimes
- ✅ **Keep-Alive**: Connection health monitoring for long-running queries
- ✅ **Smart Retry Logic**: Handles transient failures (503, 502, 504) with exponential backoff
- ✅ **Async/Await**: Fully asynchronous with optimized context handling
- ✅ **Configurable Timeouts**: Per-client timeout configuration

## Installation

```bash
dotnet add package JGerits.TrinoClient
```

## Quick Start

### Basic Query Execution

```csharp
using TrinoClient;
using TrinoClient.Model.Statement;

// Configure the Trino client
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "localhost",
    Port = 8080,
    UseSsl = false,
    User = "myuser"
};

// Create client and execute query
var client = new TrinodbClient(config);
var request = new ExecuteQueryV1Request("SELECT * FROM customers LIMIT 10");
var response = await client.ExecuteQueryV1(request);

// Access results
foreach (var row in response.Data)
{
    Console.WriteLine(string.Join(", ", row));
}
```

### Data Export

Export query results to CSV or JSON:

```csharp
var response = await client.ExecuteQueryV1(request);

// Export to CSV
var csvData = response.DataToCsv();
Console.WriteLine(string.Join("\n", csvData));

// Export to JSON
var jsonData = response.DataToJson();
Console.WriteLine(string.Join("\n", jsonData));
```

## Advanced Configuration

### Connection Pooling

Configure connection pooling for optimal performance:

```csharp
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "trino.example.com",
    Port = 8080,
    UseSsl = true,
    
    // Connection pooling settings
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),     // Recycle connections every 10 minutes
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),   // Close idle connections after 2 minutes
    MaxConnectionsPerServer = 10                             // Max concurrent connections
};
```

### HTTP Compression

HTTP compression is enabled by default. To disable:

```csharp
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "localhost",
    Port = 8080,
    CompressionDisabled = true  // Disable compression
};
```

### Session Properties

Set Trino session properties:

```csharp
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "localhost",
    Port = 8080,
    Properties = new Dictionary<string, string>
    {
        { "query_max_memory", "10GB" },
        { "distributed_sort", "true" },
        { "task_concurrency", "4" }
    }
};
```

### SSL/TLS Configuration

For secure connections:

```csharp
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "secure-trino.example.com",
    Port = 443,
    UseSsl = true,
    User = "myuser"
};
```

## Performance Optimizations

This client includes several performance optimizations based on best practices from official Trino clients:

### Network Optimizations
- **HTTP Compression**: Reduces payload sizes by 70-90%
- **HTTP/2 Protocol**: Multiplexing and header compression
- **Connection Reuse**: Pooling reduces connection overhead

### Application Optimizations
- **Async/Await**: Optimized with `ConfigureAwait(false)` to prevent context switching
- **Connection Pooling**: Automatic connection lifecycle management
- **Keep-Alive Pings**: Early detection of broken connections

### Reliability
- **Smart Retries**: Handles 503, 502, 504 errors with exponential backoff and jitter
- **Timeout Protection**: Configurable timeouts prevent indefinite hangs
- **DNS Awareness**: Connection recycling respects DNS changes

See [OPTIMIZATION_SUMMARY.md](../OPTIMIZATION_SUMMARY.md) for detailed information on all optimizations.

## API Reference

### TrinoClientSessionConfig

Configuration object for Trino client sessions.

**Properties:**
- `Host` (string): Trino server hostname
- `Port` (int): Trino server port (default: 8080)
- `Catalog` (string): Default catalog name
- `Schema` (string): Default schema name
- `UseSsl` (bool): Enable SSL/TLS
- `User` (string): Username for authentication
- `CompressionDisabled` (bool): Disable HTTP compression
- `PooledConnectionLifetime` (TimeSpan): Connection lifetime before recycling
- `PooledConnectionIdleTimeout` (TimeSpan): Idle timeout before closing
- `MaxConnectionsPerServer` (int): Maximum concurrent connections
- `Properties` (Dictionary<string, string>): Session properties

### TrinodbClient

Main client for executing queries against Trino.

**Methods:**
- `ExecuteQueryV1(ExecuteQueryV1Request request)`: Execute a query and return results
- `Dispose()`: Clean up resources

### ExecuteQueryV1Response

Response object containing query results.

**Properties:**
- `Data`: Query result data as enumerable rows
- `Columns`: Column metadata
- `Responses`: Incremental response status info
- `QueryClosed`: Whether query was closed successfully
- `LastError`: Any deserialization errors

**Methods:**
- `DataToCsv()`: Export data to CSV format
- `DataToJson()`: Export data to JSON format

## Troubleshooting

### Connection Issues

If you encounter connection problems:

1. Verify Trino server is running and accessible
2. Check firewall rules allow traffic on the specified port
3. Ensure SSL settings match your Trino configuration
4. Verify catalog and schema exist

### Performance Issues

For slow queries:

1. Enable HTTP compression (enabled by default)
2. Configure connection pooling appropriately
3. Use HTTP/2 if supported by your Trino server
4. Optimize your SQL queries

### Memory Issues

For large result sets:

1. Use pagination (LIMIT/OFFSET)
2. Process results in streaming fashion
3. Increase query_max_memory session property if needed

## License

MIT License - See [LICENSE](../LICENSE) for details.

## Related Packages

- **[JGerits.TrinoClient.EntityFrameworkCore](../TrinoClient.EntityFrameworkCore/README.md)**: Entity Framework Core database provider for Trino

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/jgerits/TrinoClient).
