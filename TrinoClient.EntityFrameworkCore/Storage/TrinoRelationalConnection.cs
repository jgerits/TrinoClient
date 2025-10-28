using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// Relational connection for Trino
/// </summary>
public class TrinoRelationalConnection : RelationalConnection, IRelationalConnection
{
    public TrinoRelationalConnection(RelationalConnectionDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override DbConnection CreateDbConnection()
    {
        var connectionString = ConnectionString;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string is not set");
        }

        return new TrinoDbConnection(connectionString);
    }
}
