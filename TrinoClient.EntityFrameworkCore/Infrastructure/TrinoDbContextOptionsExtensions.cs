using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TrinoClient.EntityFrameworkCore.Infrastructure;

/// <summary>
/// Extension methods for configuring Trino in DbContext
/// </summary>
public static class TrinoDbContextOptionsExtensions
{
    /// <summary>
    /// Configures the context to connect to a Trino database
    /// </summary>
    public static DbContextOptionsBuilder UseTrino(
        this DbContextOptionsBuilder optionsBuilder,
        string connectionString,
        Action<TrinoDbContextOptionsBuilder>? trinoOptionsAction = null)
    {
        if (optionsBuilder == null)
            throw new ArgumentNullException(nameof(optionsBuilder));

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));

        var extension = GetOrCreateExtension(optionsBuilder);
        extension = extension.WithConnectionString(connectionString);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        trinoOptionsAction?.Invoke(new TrinoDbContextOptionsBuilder(optionsBuilder));

        return optionsBuilder;
    }

    /// <summary>
    /// Configures the context to connect to a Trino database
    /// </summary>
    public static DbContextOptionsBuilder<TContext> UseTrino<TContext>(
        this DbContextOptionsBuilder<TContext> optionsBuilder,
        string connectionString,
        Action<TrinoDbContextOptionsBuilder>? trinoOptionsAction = null)
        where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)UseTrino(
            (DbContextOptionsBuilder)optionsBuilder, connectionString, trinoOptionsAction);
    }

    private static TrinoOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    {
        var existing = optionsBuilder.Options.FindExtension<TrinoOptionsExtension>();
        return existing ?? new TrinoOptionsExtension();
    }
}

/// <summary>
/// Builder for Trino-specific options
/// </summary>
public class TrinoDbContextOptionsBuilder
{
    private readonly DbContextOptionsBuilder _optionsBuilder;

    public TrinoDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
    {
        _optionsBuilder = optionsBuilder;
    }

    public virtual DbContextOptionsBuilder OptionsBuilder => _optionsBuilder;
}
