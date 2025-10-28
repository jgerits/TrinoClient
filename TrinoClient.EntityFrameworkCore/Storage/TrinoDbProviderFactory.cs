using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// DbProviderFactory for Trino
/// </summary>
public class TrinoDbProviderFactory : DbProviderFactory
{
    public static readonly TrinoDbProviderFactory Instance = new();

    private TrinoDbProviderFactory()
    {
    }

    public override DbConnection CreateConnection()
    {
        return new TrinoDbConnection();
    }

    public override DbCommand CreateCommand()
    {
        return new TrinoDbCommand();
    }

    public override DbParameter CreateParameter()
    {
        return new TrinoDbParameter();
    }

    public override DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
        return new DbConnectionStringBuilder();
    }
}
