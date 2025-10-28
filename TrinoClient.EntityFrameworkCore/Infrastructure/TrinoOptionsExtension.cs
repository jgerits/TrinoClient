using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TrinoClient.EntityFrameworkCore.Infrastructure;

/// <summary>
/// Options extension for Trino
/// </summary>
public class TrinoOptionsExtension : IDbContextOptionsExtension
{
    private string? _connectionString;
    private DbContextOptionsExtensionInfo? _info;

    public TrinoOptionsExtension()
    {
    }

    protected TrinoOptionsExtension(TrinoOptionsExtension copyFrom)
    {
        _connectionString = copyFrom._connectionString;
    }

    public virtual DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

    public virtual string? ConnectionString => _connectionString;

    public virtual TrinoOptionsExtension WithConnectionString(string connectionString)
    {
        var clone = Clone();
        clone._connectionString = connectionString;
        return clone;
    }

    protected virtual TrinoOptionsExtension Clone() => new(this);

    public void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkTrino();
    }

    public void Validate(IDbContextOptions options)
    {
        // Validation logic if needed
    }

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        private new TrinoOptionsExtension Extension => (TrinoOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider => true;

        public override string LogFragment => "using Trino";

        public override int GetServiceProviderHashCode() => Extension.ConnectionString?.GetHashCode() ?? 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ExtensionInfo otherInfo
                && Extension.ConnectionString == otherInfo.Extension.ConnectionString;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["Trino:ConnectionString"] = (Extension.ConnectionString?.GetHashCode() ?? 0).ToString();
        }
    }
}
