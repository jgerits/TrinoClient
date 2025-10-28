# TrinoClient

A high-performance .NET client for Trino (formerly PrestoSQL) with advanced optimizations including HTTP compression, connection pooling, HTTP/2 support, and configurable retry logic.

## Packages

This repository contains two NuGet packages:

1. **JGerits.TrinoClient** - The core Trino client library
2. **JGerits.TrinoClient.EntityFrameworkCore** - Entity Framework Core database provider for Trino

## Features

### Core Client Features
- ✅ **HTTP Compression**: Automatic GZip/Deflate compression reduces bandwidth by 70-90%
- ✅ **HTTP/2 Support**: Modern protocol with multiplexing and header compression
- ✅ **Connection Pooling**: Advanced connection management with configurable lifetimes
- ✅ **Keep-Alive**: Connection health monitoring for long-running queries
- ✅ **Smart Retry Logic**: Handles transient failures (503, 502, 504) with exponential backoff
- ✅ **Async/Await**: Fully asynchronous with optimized context handling
- ✅ **Configurable Timeouts**: Per-client timeout configuration

### Entity Framework Core Provider Features
- ✅ **LINQ Queries**: Write type-safe LINQ queries against Trino databases
- ✅ **Database Scaffolding**: Generate entity classes from existing Trino tables
- ✅ **Type Mappings**: Full support for Trino data types
- ✅ **Read Operations**: Query data using EF Core's familiar API

## Installation

### Core Client
```bash
dotnet add package JGerits.TrinoClient
```

### Entity Framework Core Provider
```bash
dotnet add package JGerits.TrinoClient.EntityFrameworkCore
```

## Usage

### Core Client - Basic Example

This demonstrates creating a new client config, initializing an ITrinoClient, and executing a simple query. The
returned data can be formatted in CSV or JSON. Additionally, all of the raw data is returned from the server
in case the deserialization process fails in .NET, the user can still access and manipulate the returned data.

```csharp
TrinoClientSessionConfig config = new TrinoClientSessionConfig("hive", "cars")
{
   Host = "localhost",
   Port = 8080
};

var client = new TrinodbClient(config);
var request = new ExecuteQueryV1Request("select * from tracklets limit 5;");
var queryResponse = await client.ExecuteQueryV1(request);

Console.WriteLine(String.Join("\n", queryResponse.DataToCsv()));
Console.WriteLine("-------------------------------------------------------------------");
Console.WriteLine(String.Join("\n", queryResponse.DataToJson()));
```

### Entity Framework Core - Basic Example

Use EF Core to query Trino databases with LINQ:

```csharp
using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTrino(
            "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin"
        );
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Query with LINQ
using var context = new MyDbContext();
var expensiveProducts = await context.Products
    .Where(p => p.Price > 100)
    .OrderBy(p => p.Name)
    .ToListAsync();
```

### Entity Framework Core - Database Scaffolding

Generate entity classes from your existing Trino database:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Port=8080;Catalog=hive;Schema=sales;SSL=false;User=admin" TrinoClient.EntityFrameworkCore -o Models
```

This will:
1. Connect to your Trino database
2. Read the schema from the specified catalog and schema
3. Generate entity classes for each table
4. Create a DbContext class configured for your database

For more details on the Entity Framework Core provider, see [TrinoClient.EntityFrameworkCore/README.md](TrinoClient.EntityFrameworkCore/README.md).

### Advanced Configuration

Configure connection pooling, compression, and other optimizations:

```csharp
TrinoClientSessionConfig config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "trino.example.com",
    Port = 8080,
    UseSsl = true,
    
    // HTTP Compression (enabled by default)
    CompressionDisabled = false,
    
    // Connection Pooling Settings
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),     // Recycle connections every 10 minutes
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),   // Close idle connections after 2 minutes
    MaxConnectionsPerServer = 10,                            // Allow up to 10 concurrent connections
    
    // Session Properties
    Properties = new Dictionary<string, string>
    {
        { "query_max_memory", "10GB" },
        { "distributed_sort", "true" }
    }
};

var client = new TrinodbClient(config);
```

## Performance Optimizations

This client includes several performance optimizations based on best practices from official Trino clients:

### Network Optimizations
- **HTTP Compression**: Reduces payload sizes by 70-90%
- **HTTP/2 Protocol**: Multiplexing and header compression
- **Connection Reuse**: Pooling reduces connection overhead

### Application Optimizations
- **Async/Await**: Optimized with ConfigureAwait(false) to prevent context switching
- **Connection Pooling**: Automatic connection lifecycle management
- **Keep-Alive Pings**: Early detection of broken connections

### Reliability
- **Smart Retries**: Handles 503, 502, 504 errors with exponential backoff and jitter
- **Timeout Protection**: Configurable timeouts prevent indefinite hangs
- **DNS Awareness**: Connection recycling respects DNS changes

See [OPTIMIZATION_SUMMARY.md](OPTIMIZATION_SUMMARY.md) for detailed information on all optimizations.

## Revision History

### 1.0.0 (Current)
- Added SocketsHttpHandler with advanced connection pooling
- Added HTTP/2 support with automatic fallback
- Added connection keep-alive configuration
- Added configurable connection pooling settings
- Migrated from HttpClientHandler to SocketsHttpHandler

### 0.2
- Added HTTP compression support (GZip/Deflate)
- Added enhanced retry logic for gateway errors
- Added ConfigureAwait(false) optimizations
- Added HttpClient timeout configuration

### 0.1
Fork from [PrestoClient](https://github.com/bamcis-io/PrestoClient)