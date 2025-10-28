using Microsoft.EntityFrameworkCore;
using TrinoClient.EntityFrameworkCore.Infrastructure;

namespace TrinoClient.EntityFrameworkCore.Example;

/// <summary>
/// Example DbContext for Trino
/// </summary>
public class SampleTrinoContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure connection to your Trino instance
        // Update these values to match your Trino server
        optionsBuilder.UseTrino(
            "Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entity mappings if needed
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(e => e.CustomerId);
        });
    }
}

/// <summary>
/// Example Product entity
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Example Customer entity
/// </summary>
public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
}

class Program
{
    static Task Main(string[] args)
    {
        Console.WriteLine("Trino Entity Framework Core Provider - Example Application");
        Console.WriteLine("============================================================");
        Console.WriteLine();

        // Note: This example assumes you have a running Trino instance with sample data
        // Uncomment and modify the code below to test with your actual Trino server

        /*
        try
        {
            using var context = new SampleTrinoContext();

            // Example 1: Query all products
            Console.WriteLine("Example 1: Query all products");
            Console.WriteLine("------------------------------");
            var products = await context.Products
                .AsNoTracking()
                .ToListAsync();

            foreach (var product in products)
            {
                Console.WriteLine($"  {product.Id}: {product.Name} - ${product.Price}");
            }
            Console.WriteLine();

            // Example 2: Query with filtering
            Console.WriteLine("Example 2: Filter products by price");
            Console.WriteLine("------------------------------------");
            var expensiveProducts = await context.Products
                .AsNoTracking()
                .Where(p => p.Price > 100)
                .OrderBy(p => p.Price)
                .ToListAsync();

            foreach (var product in expensiveProducts)
            {
                Console.WriteLine($"  {product.Name}: ${product.Price}");
            }
            Console.WriteLine();

            // Example 3: Projection (select specific columns)
            Console.WriteLine("Example 3: Select product names only");
            Console.WriteLine("-------------------------------------");
            var productNames = await context.Products
                .AsNoTracking()
                .Select(p => p.Name)
                .ToListAsync();

            foreach (var name in productNames)
            {
                Console.WriteLine($"  - {name}");
            }
            Console.WriteLine();

            // Example 4: Query with aggregation
            Console.WriteLine("Example 4: Aggregate queries");
            Console.WriteLine("-----------------------------");
            var totalProducts = await context.Products.CountAsync();
            var avgPrice = await context.Products.AverageAsync(p => p.Price);
            var maxPrice = await context.Products.MaxAsync(p => p.Price);

            Console.WriteLine($"  Total Products: {totalProducts}");
            Console.WriteLine($"  Average Price: ${avgPrice:F2}");
            Console.WriteLine($"  Max Price: ${maxPrice:F2}");
            Console.WriteLine();

            // Example 5: Join query
            Console.WriteLine("Example 5: Join customers with their orders");
            Console.WriteLine("--------------------------------------------");
            // Note: This assumes you have an Orders table
            // Adjust the query based on your actual schema
            
            var customers = await context.Customers
                .AsNoTracking()
                .Take(5)
                .ToListAsync();

            foreach (var customer in customers)
            {
                Console.WriteLine($"  {customer.Name} ({customer.Email})");
            }
            Console.WriteLine();

            Console.WriteLine("All examples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine();
            Console.WriteLine("Make sure:");
            Console.WriteLine("  1. Your Trino server is running");
            Console.WriteLine("  2. The connection string is correct");
            Console.WriteLine("  3. The catalog and schema exist");
            Console.WriteLine("  4. The tables (products, customers) exist in your schema");
        }
        */

        Console.WriteLine("Setup Instructions:");
        Console.WriteLine("-------------------");
        Console.WriteLine("1. Update the connection string in SampleTrinoContext.OnConfiguring()");
        Console.WriteLine("2. Ensure your Trino server is running and accessible");
        Console.WriteLine("3. Create sample tables in your Trino database:");
        Console.WriteLine();
        Console.WriteLine("   Example SQL for Trino:");
        Console.WriteLine("   ----------------------");
        Console.WriteLine("   CREATE TABLE products (");
        Console.WriteLine("     id INTEGER,");
        Console.WriteLine("     name VARCHAR,");
        Console.WriteLine("     price DECIMAL(10,2),");
        Console.WriteLine("     description VARCHAR,");
        Console.WriteLine("     created_date TIMESTAMP");
        Console.WriteLine("   );");
        Console.WriteLine();
        Console.WriteLine("   CREATE TABLE customers (");
        Console.WriteLine("     customer_id INTEGER,");
        Console.WriteLine("     name VARCHAR,");
        Console.WriteLine("     email VARCHAR,");
        Console.WriteLine("     registration_date TIMESTAMP");
        Console.WriteLine("   );");
        Console.WriteLine();
        Console.WriteLine("4. Uncomment the example code in Program.cs");
        Console.WriteLine("5. Run the application again");
        Console.WriteLine();
        Console.WriteLine("For database scaffolding (generate entities from existing tables), run:");
        Console.WriteLine("  dotnet ef dbcontext scaffold \"Host=localhost;Port=8080;Catalog=hive;Schema=default;SSL=false;User=admin\" TrinoClient.EntityFrameworkCore -o Models");
        
        return Task.CompletedTask;
    }
}
