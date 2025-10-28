# TrinoClient for .NET

A comprehensive .NET solution for working with Trino (formerly PrestoSQL), including a high-performance client library and an Entity Framework Core database provider.

## 📦 Packages

This repository contains two NuGet packages:

| Package | Description | Documentation |
|---------|-------------|---------------|
| **[JGerits.TrinoClient](TrinoClient/)** | High-performance Trino client with HTTP/2, compression, and connection pooling | [README](TrinoClient/README.md) |
| **[JGerits.TrinoClient.EntityFrameworkCore](TrinoClient.EntityFrameworkCore/)** | Entity Framework Core database provider for Trino | [README](TrinoClient.EntityFrameworkCore/README.md) |

## 🚀 Quick Start

### Core Client

Execute queries directly against Trino:

```bash
dotnet add package JGerits.TrinoClient
```

```csharp
using TrinoClient;

var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "localhost",
    Port = 8080
};

var client = new TrinodbClient(config);
var request = new ExecuteQueryV1Request("SELECT * FROM customers LIMIT 10");
var response = await client.ExecuteQueryV1(request);
```

👉 **[Full documentation](TrinoClient/README.md)**

### Entity Framework Core Provider

Query Trino with LINQ and EF Core:

```bash
dotnet add package JGerits.TrinoClient.EntityFrameworkCore
```

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

// Query with LINQ
var expensiveProducts = await context.Products
    .Where(p => p.Price > 100)
    .OrderBy(p => p.Name)
    .ToListAsync();
```

👉 **[Full documentation](TrinoClient.EntityFrameworkCore/README.md)**

## ✨ Key Features

### Core Client (JGerits.TrinoClient)

- ✅ **HTTP Compression**: Automatic GZip/Deflate compression reduces bandwidth by 70-90%
- ✅ **HTTP/2 Support**: Modern protocol with multiplexing and header compression
- ✅ **Connection Pooling**: Advanced connection management with configurable lifetimes
- ✅ **Smart Retry Logic**: Handles transient failures with exponential backoff
- ✅ **Async/Await**: Fully asynchronous with optimized context handling
- ✅ **Keep-Alive**: Connection health monitoring for long-running queries

### Entity Framework Core Provider (JGerits.TrinoClient.EntityFrameworkCore)

- ✅ **LINQ Queries**: Write type-safe LINQ queries against Trino databases
- ✅ **Database Scaffolding**: Generate entity classes from existing Trino tables
- ✅ **Type Mappings**: Full support for Trino data types
- ✅ **Async/Await**: Full asynchronous query execution
- ✅ **Read Operations**: Query data using EF Core's familiar API

## 📖 Documentation

### Package-Specific Documentation

- **[Core Client Documentation](TrinoClient/README.md)** - API reference, configuration, and advanced features
- **[EF Core Provider Documentation](TrinoClient.EntityFrameworkCore/README.md)** - LINQ queries, scaffolding, and best practices

### Additional Resources

- **[Optimization Summary](OPTIMIZATION_SUMMARY.md)** - Performance optimizations and benchmarks
- **[Testing Guide](TESTING.md)** - Running and contributing tests

## 🎯 Use Cases

### When to Use the Core Client

Use **JGerits.TrinoClient** when you need:
- Direct control over SQL queries
- Raw query execution and result processing
- Custom data transformations
- High-performance batch processing
- CSV/JSON export functionality

### When to Use the EF Core Provider

Use **JGerits.TrinoClient.EntityFrameworkCore** when you need:
- LINQ query capabilities
- Type-safe database access
- Integration with existing EF Core applications
- Automatic entity generation from database schema
- Object-relational mapping (ORM)

## 📦 Installation

### Core Client Only

```bash
dotnet add package JGerits.TrinoClient
```

### Entity Framework Core Provider

```bash
# EF Core provider includes the core client as a dependency
dotnet add package JGerits.TrinoClient.EntityFrameworkCore
```

## 🔧 Configuration Examples

### Core Client - Advanced Configuration

```csharp
var config = new TrinoClientSessionConfig("hive", "default")
{
    Host = "trino.example.com",
    Port = 8080,
    UseSsl = true,
    
    // Connection pooling
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
    MaxConnectionsPerServer = 10,
    
    // Session properties
    Properties = new Dictionary<string, string>
    {
        { "query_max_memory", "10GB" },
        { "distributed_sort", "true" }
    }
};
```

### EF Core Provider - Dependency Injection

```csharp
// Program.cs (ASP.NET Core)
builder.Services.AddDbContext<MyTrinoContext>(options =>
    options.UseTrino(builder.Configuration.GetConnectionString("TrinoConnection")));
```

## 🏗️ Database Scaffolding

Generate entity classes from your existing Trino database:

```bash
dotnet ef dbcontext scaffold \
  "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin" \
  TrinoClient.EntityFrameworkCore \
  --output-dir Models
```

This will:
1. Connect to your Trino database
2. Read the schema from the specified catalog and schema
3. Generate entity classes for each table
4. Create a DbContext class configured for your database

## 🔄 Version History

### 1.0.0 (Current)
- ✅ Entity Framework Core provider with full LINQ support
- ✅ Database scaffolding capabilities
- ✅ SocketsHttpHandler with advanced connection pooling
- ✅ HTTP/2 support with automatic fallback
- ✅ Connection keep-alive configuration
- ✅ Configurable connection pooling settings

### 0.2
- HTTP compression support (GZip/Deflate)
- Enhanced retry logic for gateway errors
- ConfigureAwait(false) optimizations
- HttpClient timeout configuration

### 0.1
- Initial fork from [PrestoClient](https://github.com/bamcis-io/PrestoClient)

## 🤝 Contributing

Contributions are welcome! Please see the [GitHub repository](https://github.com/jgerits/TrinoClient) for:
- Issue reporting
- Feature requests
- Pull requests
- Discussion

## 📄 License

MIT License - See [LICENSE](LICENSE) for details.

## 🔗 Links

- **GitHub Repository**: https://github.com/jgerits/TrinoClient
- **NuGet Package (Core)**: JGerits.TrinoClient
- **NuGet Package (EF Core)**: JGerits.TrinoClient.EntityFrameworkCore
- **Trino Documentation**: https://trino.io/docs/current/
- **Entity Framework Core Documentation**: https://docs.microsoft.com/en-us/ef/core/

## ⚠️ Important Notes

### Limitations of Entity Framework Core Provider

- ❌ **No Transactions**: Trino doesn't support traditional ACID transactions
- ❌ **No Migrations**: Schema changes should be managed through underlying data sources
- ⚠️ **Limited Write Support**: Trino is primarily a query engine; write operations depend on connector configuration

### Performance Recommendations

1. **Use Async Methods**: Always prefer async methods for better scalability
2. **Disable Change Tracking**: Use `AsNoTracking()` for read-only queries in EF Core
3. **Enable Compression**: Keep HTTP compression enabled (default) for 70-90% bandwidth savings
4. **Configure Connection Pooling**: Optimize connection pool settings for your workload
5. **Use Projections**: Select only the columns you need to reduce data transfer

## 📞 Support

For questions, issues, or feature requests:

- **GitHub Issues**: [Report an issue](https://github.com/jgerits/TrinoClient/issues)
- **Discussions**: [GitHub Discussions](https://github.com/jgerits/TrinoClient/discussions)

---

Made with ❤️ for the .NET and Trino communities