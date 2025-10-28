# Trino Entity Framework Core Provider

This package provides an Entity Framework Core database provider for Trino (formerly PrestoSQL). It enables you to use EF Core to query Trino databases and generate entity classes from your database schema.

## Features

- ✅ **LINQ Queries**: Write type-safe LINQ queries against Trino databases
- ✅ **Database Scaffolding**: Generate entity classes from existing Trino tables
- ✅ **Type Mappings**: Full support for Trino data types
- ✅ **Read Operations**: Query data using EF Core's familiar API
- ⚠️ **Limited Write Support**: Trino is primarily a query engine; CREATE/UPDATE/DELETE operations are limited

## Installation

```bash
dotnet add package JGerits.TrinoClient.EntityFrameworkCore
```

## Usage

### Basic Configuration

Configure your `DbContext` to use Trino:

```csharp
using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseTrino("Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=myuser");
    }

    public DbSet<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### Connection String Format

```
Host=<hostname>;Port=<port>;Catalog=<catalog>;Schema=<schema>;SSL=<true|false>;User=<username>
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
