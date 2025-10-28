using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;
using System.Data.Common;
using TrinoClient.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Scaffolding;

/// <summary>
/// Database model factory for scaffolding Trino databases
/// </summary>
public class TrinoDatabaseModelFactory : IDatabaseModelFactory
{
    public TrinoDatabaseModelFactory()
    {
    }

    public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
    {
        using var connection = new TrinoDbConnection(connectionString);
        return Create(connection, options);
    }

    public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
    {
        var databaseModel = new DatabaseModel();

        var trinoConnection = connection as TrinoDbConnection 
            ?? throw new ArgumentException("Connection must be a TrinoDbConnection", nameof(connection));

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        var catalog = connection.Database;
        databaseModel.DatabaseName = catalog;

        var tables = GetTables(trinoConnection, options);
        foreach (var table in tables)
        {
            table.Database = databaseModel;
            databaseModel.Tables.Add(table);
        }

        return databaseModel;
    }

    private IEnumerable<DatabaseTable> GetTables(TrinoDbConnection connection, DatabaseModelFactoryOptions options)
    {
        var tables = new List<DatabaseTable>();

        var showTablesCommand = connection.CreateCommand();
        showTablesCommand.CommandText = "SHOW TABLES";

        using (var reader = showTablesCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                var tableName = reader.GetString(0);

                // Filter tables if specified
                if (options.Tables.Any() && !options.Tables.Contains(tableName))
                    continue;

                var table = new DatabaseTable
                {
                    Name = tableName,
                    Schema = connection.Database
                };

                // Get columns for the table
                var columns = GetColumns(connection, tableName);
                foreach (var column in columns)
                {
                    column.Table = table;
                    table.Columns.Add(column);
                }

                tables.Add(table);
            }
        }

        return tables;
    }

    private IEnumerable<DatabaseColumn> GetColumns(TrinoDbConnection connection, string tableName)
    {
        var columns = new List<DatabaseColumn>();

        var describeCommand = connection.CreateCommand();
        describeCommand.CommandText = $"DESCRIBE {DelimitIdentifier(tableName)}";

        using (var reader = describeCommand.ExecuteReader())
        {
            var ordinal = 0;
            while (reader.Read())
            {
                var columnName = reader.GetString(0);
                var dataType = reader.GetString(1);
                var nullable = reader.FieldCount > 2 ? reader.GetString(2) : string.Empty;

                var column = new DatabaseColumn
                {
                    Name = columnName,
                    StoreType = dataType,
                    IsNullable = string.IsNullOrEmpty(nullable) || !nullable.Equals("NOT NULL", StringComparison.OrdinalIgnoreCase),
                    DefaultValueSql = null,
                    ComputedColumnSql = null
                };

                column["Ordinal"] = ordinal++;
                columns.Add(column);
            }
        }

        return columns;
    }

    private static string DelimitIdentifier(string identifier)
    {
        return $"\"{identifier.Replace("\"", "\"\"")}\"";
    }
}
