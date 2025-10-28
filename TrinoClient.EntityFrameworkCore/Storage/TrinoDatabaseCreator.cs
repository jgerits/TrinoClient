using Microsoft.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// Database creator for Trino - Trino doesn't support CREATE DATABASE
/// </summary>
public class TrinoDatabaseCreator : RelationalDatabaseCreator
{
    public TrinoDatabaseCreator(
        RelationalDatabaseCreatorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override bool Exists()
    {
        // In Trino, we assume the catalog/schema exists
        return true;
    }

    public override void Create()
    {
        // Trino doesn't support CREATE DATABASE
        throw new NotSupportedException("Trino does not support creating databases through EF Core");
    }

    public override void Delete()
    {
        // Trino doesn't support DROP DATABASE
        throw new NotSupportedException("Trino does not support deleting databases through EF Core");
    }

    public override bool HasTables()
    {
        var connection = Dependencies.Connection;
        var command = connection.DbConnection.CreateCommand();
        command.CommandText = "SHOW TABLES";
        
        using var reader = command.ExecuteReader();
        return reader.Read();
    }
}
