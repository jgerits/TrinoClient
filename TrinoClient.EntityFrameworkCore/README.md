# JGerits.TrinoClient.EntityFrameworkCore

An Entity Framework Core database provider for Trino (formerly PrestoSQL). This package enables you to use EF Core to query Trino databases with LINQ and generate entity classes from your database schema.

## Features

- ✅ **LINQ Queries**: Write type-safe LINQ queries against Trino databases
- ✅ **Database Scaffolding**: Generate entity classes from existing Trino tables
- ✅ **Async/Await Support**: Full asynchronous query execution
- ✅ **Type Mappings**: Comprehensive support for Trino data types
- ✅ **Read Operations**: Query data using EF Core's familiar API
- ⚠️ **Limited Write Support**: Trino is primarily a query engine; CREATE/UPDATE/DELETE support depends on connector configuration
- ❌ **No Transactions**: Trino doesn't support traditional ACID transactions
- ❌ **No Migrations**: Schema changes should be managed through underlying data sources

## Installation

```bash
dotnet add package JGerits.TrinoClient.EntityFrameworkCore
```

**Note**: This package requires `JGerits.TrinoClient` which will be installed automatically as a dependency.

## Quick Start

### 1. Create Your DbContext

```csharp
using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

public class MyTrinoContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }

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
    public DateTime CreatedDate { get; set; }
}
```

### 2. Query Data with LINQ

```csharp
using var context = new MyTrinoContext();

// Simple query
var products = await context.Products.ToListAsync();

// Filtered query
var expensiveProducts = await context.Products
    .Where(p => p.Price > 100)
    .OrderBy(p => p.Name)
    .ToListAsync();

// Projection
var productNames = await context.Products
    .Select(p => p.Name)
    .ToListAsync();

// Aggregation
var avgPrice = await context.Products.AverageAsync(p => p.Price);
var totalCount = await context.Products.CountAsync();
```

## Connection String Format

```
Host=<hostname>;Port=<port>;Catalog=<catalog>;Schema=<schema>;SSL=<true|false>;User=<username>
```

**Parameters:**
- `Host`: Trino server hostname (required)
- `Port`: Trino server port (default: 8080)
- `Catalog`: Trino catalog name (required)
- `Schema`: Schema name (required)
- `SSL`: Enable SSL/TLS connection (true/false)
- `User`: Username for authentication

**Example:**
```
Host=trino.example.com;Port=8080;Catalog=hive;Schema=sales;SSL=true;User=myuser
```

## Database Scaffolding

Generate entity classes from your existing Trino database:

### Using .NET CLI

```bash
dotnet ef dbcontext scaffold \
  "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin" \
  TrinoClient.EntityFrameworkCore \
  --output-dir Models
```

### Using Package Manager Console

```powershell
Scaffold-DbContext "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin" `
  TrinoClient.EntityFrameworkCore `
  -OutputDir Models
```

### Scaffolding Options

```bash
# Scaffold specific tables only
dotnet ef dbcontext scaffold "..." TrinoClient.EntityFrameworkCore \
  --table products --table customers \
  --output-dir Models

# Force overwrite existing files
dotnet ef dbcontext scaffold "..." TrinoClient.EntityFrameworkCore \
  --output-dir Models --force

# Use data annotations instead of fluent API
dotnet ef dbcontext scaffold "..." TrinoClient.EntityFrameworkCore \
  --output-dir Models --data-annotations
```

## Supported LINQ Operations

### ✅ Fully Supported
- `Where` - Filtering
- `Select` - Projection
- `OrderBy` / `OrderByDescending` - Sorting
- `ThenBy` / `ThenByDescending` - Secondary sorting
- `Take` / `Skip` - Pagination (LIMIT/OFFSET)
- `First` / `FirstOrDefault` - Single row retrieval
- `Single` / `SingleOrDefault` - Single row with validation
- `Any` - Existence check
- `Count` / `LongCount` - Row counting
- `Sum` / `Average` / `Min` / `Max` - Aggregations
- `GroupBy` - Grouping
- `Join` - Inner joins
- `ToList` / `ToArray` - Materialization

### ⚠️ Partial Support
- `Include` / `ThenInclude` - Navigation properties (generates joins)
- `Distinct` - Supported, but may not optimize in all cases

### ❌ Not Supported
- Change tracking (use `AsNoTracking()` for better performance)
- `Add` / `Update` / `Remove` - Limited write operations
- Transactions (`BeginTransaction` throws `NotSupportedException`)
- Migrations

## Type Mappings

### Trino to .NET Type Mappings

| Trino Type | .NET Type | Notes |
|------------|-----------|-------|
| `boolean` | `bool` | |
| `tinyint` | `sbyte` | |
| `smallint` | `short` | |
| `integer` | `int` | |
| `bigint` | `long` | |
| `real` | `float` | |
| `double` | `double` | |
| `decimal` | `decimal` | |
| `varchar` | `string` | |
| `char` | `string` | |
| `varbinary` | `byte[]` | |
| `date` | `DateTime` | |
| `timestamp` | `DateTime` | |
| `time` | `TimeSpan` | |
| `uuid` | `Guid` | |

## Best Practices

### 1. Use AsNoTracking for Read-Only Queries

Since Trino is primarily a query engine, disable change tracking for better performance:

```csharp
var products = await context.Products
    .AsNoTracking()
    .Where(p => p.Price > 100)
    .ToListAsync();
```

### 2. Prefer Async Methods

Always use async methods for database operations:

```csharp
// ✅ Good
var products = await context.Products.ToListAsync();

// ❌ Avoid
var products = context.Products.ToList();
```

### 3. Use Projections to Reduce Data Transfer

Select only the columns you need:

```csharp
var productSummary = await context.Products
    .Select(p => new { p.Id, p.Name })
    .ToListAsync();
```

### 4. Implement Pagination

Use `Skip` and `Take` for large result sets:

```csharp
var page = await context.Products
    .OrderBy(p => p.Id)
    .Skip(pageNumber * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### 5. Connection String in Configuration

Store connection strings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "TrinoConnection": "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin"
  }
}
```

```csharp
public class MyTrinoContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MyTrinoContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTrino(_configuration.GetConnectionString("TrinoConnection"));
    }
}
```

## Dependency Injection

### ASP.NET Core Setup

In `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddDbContext<MyTrinoContext>(options =>
    options.UseTrino(builder.Configuration.GetConnectionString("TrinoConnection")));
```

Usage in controllers:

```csharp
public class ProductsController : ControllerBase
{
    private readonly MyTrinoContext _context;

    public ProductsController(MyTrinoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .AsNoTracking()
            .ToListAsync();
        return Ok(products);
    }
}
```

## Limitations

### Transactions Not Supported

Trino doesn't support traditional ACID transactions:

```csharp
// ❌ This will throw NotSupportedException
using var transaction = context.Database.BeginTransaction();
```

### Migrations Not Supported

Schema changes should be managed through your Trino data sources. EF Core migrations are not supported:

```bash
# ❌ This won't work
dotnet ef migrations add InitialCreate
```

### Write Operations Limited

While the provider supports basic write operations through ADO.NET, write support depends on your Trino connector configuration. Most Trino deployments are read-only.

## Troubleshooting

### Connection Issues

**Problem**: Cannot connect to Trino server

**Solutions**:
1. Verify Trino server is running: `curl http://localhost:8080/v1/info`
2. Check connection string parameters
3. Ensure catalog and schema exist
4. Verify firewall rules

### Scaffolding Issues

**Problem**: "Unable to connect to database"

**Solutions**:
1. Test connection string with basic queries first
2. Ensure the schema contains tables
3. Verify user has SELECT permissions

### Query Performance

**Problem**: Slow query execution

**Solutions**:
1. Use `AsNoTracking()` for read-only queries
2. Add appropriate projections with `Select()`
3. Implement pagination
4. Optimize Trino queries at the SQL level

### Type Conversion Errors

**Problem**: "Cannot convert Trino type X to .NET type Y"

**Solutions**:
1. Check type mapping table above
2. Use explicit casting in LINQ queries
3. Map to `object` or `dynamic` for unsupported types

## Performance Tips

1. **Disable Change Tracking**: Use `AsNoTracking()` for all queries
2. **Use Projections**: Select only needed columns
3. **Implement Pagination**: Don't load all rows at once
4. **Leverage Async**: Use async methods throughout
5. **Connection Pooling**: Reuse DbContext instances appropriately in DI
6. **Batch Queries**: Combine multiple queries when possible

## Example: Complete Application

```csharp
using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

// DbContext
public class SalesContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTrino(
            "Host=localhost;Port=8080;Catalog=hive;Schema=sales;SSL=false;User=admin"
        );
    }
}

// Entities
public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// Usage
class Program
{
    static async Task Main(string[] args)
    {
        using var context = new SalesContext();

        // Get recent orders
        var recentOrders = await context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate > DateTime.Now.AddDays(-30))
            .OrderByDescending(o => o.OrderDate)
            .Take(10)
            .ToListAsync();

        // Get customer statistics
        var customerStats = await context.Customers
            .AsNoTracking()
            .Select(c => new
            {
                c.Name,
                OrderCount = context.Orders.Count(o => o.CustomerId == c.CustomerId),
                TotalSpent = context.Orders
                    .Where(o => o.CustomerId == c.CustomerId)
                    .Sum(o => o.TotalAmount)
            })
            .ToListAsync();

        foreach (var stat in customerStats)
        {
            Console.WriteLine($"{stat.Name}: {stat.OrderCount} orders, ${stat.TotalSpent:N2} spent");
        }
    }
}
```

## License

MIT License - See [LICENSE](../LICENSE) for details.

## Related Packages

- **[JGerits.TrinoClient](../TrinoClient/README.md)**: Core Trino client library (required dependency)

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/jgerits/TrinoClient).

## Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Trino Documentation](https://trino.io/docs/current/)
- [LINQ Query Syntax](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
```

Example:
```
Host=trino.example.com;Port=8080;Catalog=hive;Schema=sales;SSL=true;User=admin
```

### Querying Data

Use standard EF Core LINQ queries:

```csharp
using var context = new MyDbContext();

// Simple query
var products = await context.Products
    .Where(p => p.Price > 100)
    .ToListAsync();

// Projection
var productNames = await context.Products
    .Select(p => p.Name)
    .ToListAsync();

// Join
var query = from p in context.Products
            join c in context.Categories on p.CategoryId equals c.Id
            select new { p.Name, CategoryName = c.Name };

var results = await query.ToListAsync();
```

### Database Scaffolding

Generate entity classes from an existing Trino database:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=myuser" TrinoClient.EntityFrameworkCore -o Models
```

This will:
1. Connect to your Trino database
2. Read the schema from the specified catalog and schema
3. Generate entity classes for each table
4. Create a DbContext class configured for your database

## Supported Trino Types

| Trino Type | .NET Type |
|------------|-----------|
| boolean | bool |
| tinyint | byte |
| smallint | short |
| integer | int |
| bigint | long |
| real | float |
| double | double |
| decimal | decimal |
| varchar | string |
| char | string |
| varbinary | byte[] |
| date | DateTime |
| timestamp | DateTime |
| time | TimeSpan |
| uuid | Guid |

## Limitations

### No Transaction Support
Trino does not support traditional ACID transactions. Attempting to use transactions will throw a `NotSupportedException`.

```csharp
// This will throw NotSupportedException
using var transaction = context.Database.BeginTransaction();
```

### No Database Migrations
Creating or modifying database schema through EF Core migrations is not supported. Trino is primarily a query engine, and schema management should be done through the underlying data sources.

```csharp
// These operations are not supported
context.Database.EnsureCreated();  // Not supported
context.Database.Migrate();        // Not supported
```

### Read-Only Operations Recommended
While the provider supports the EF Core API, Trino is optimized for querying. INSERT, UPDATE, and DELETE operations support depends on the underlying connector configuration.

## Example: Complete Application

```csharp
using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

// Define your DbContext
public class TrinoContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTrino(
            "Host=localhost;Port=8080;Catalog=hive;Schema=sales;SSL=false;User=admin"
        );
    }
}

// Define your entities
public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public Customer Customer { get; set; }
}

// Use in your application
class Program
{
    static async Task Main(string[] args)
    {
        using var context = new TrinoContext();

        // Query customers
        var customers = await context.Customers
            .Where(c => c.Email.Contains("@example.com"))
            .ToListAsync();

        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Name}: {customer.Email}");
        }

        // Query with join
        var ordersWithCustomers = await context.Orders
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-1))
            .ToListAsync();

        foreach (var order in ordersWithCustomers)
        {
            Console.WriteLine($"Order {order.OrderId} by {order.Customer.Name}: ${order.TotalAmount}");
        }
    }
}
```

## Advanced Configuration

### Using Dependency Injection

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TrinoContext>(options =>
            options.UseTrino(Configuration.GetConnectionString("Trino")));
    }
}
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "Trino": "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin"
  }
}
```

## Performance Tips

1. **Use Projections**: Select only the columns you need
   ```csharp
   var names = await context.Products.Select(p => p.Name).ToListAsync();
   ```

2. **Leverage Trino's Distributed Query Engine**: Complex aggregations and joins are handled efficiently by Trino

3. **Use AsNoTracking for Read-Only Queries**: Disable change tracking for better performance
   ```csharp
   var products = await context.Products.AsNoTracking().ToListAsync();
   ```

## Troubleshooting

### Connection Issues
- Verify your Trino server is running and accessible
- Check that the catalog and schema exist
- Ensure SSL settings match your server configuration

### Query Errors
- Some LINQ operations may not translate to Trino SQL
- Use `.ToQueryString()` to see the generated SQL for debugging
- Fall back to raw SQL queries if needed:
  ```csharp
  var results = await context.Products
      .FromSqlRaw("SELECT * FROM products WHERE price > 100")
      .ToListAsync();
  ```

## Contributing

Contributions are welcome! Please submit issues and pull requests on the [GitHub repository](https://github.com/jgerits/TrinoClient).

## License

This project is licensed under the MIT License - see the LICENSE file for details.
