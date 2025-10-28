using System.Data;
using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbCommand implementation for Trino
/// </summary>
public class TrinoDbCommand : DbCommand
{
    private TrinoDbConnection? _connection;
    private string _commandText = string.Empty;

    public TrinoDbCommand()
    {
    }

    public TrinoDbCommand(TrinoDbConnection connection)
    {
        _connection = connection;
    }

    public TrinoDbCommand(string commandText, TrinoDbConnection connection)
    {
        _commandText = commandText;
        _connection = connection;
    }

    public override string CommandText
    {
        get => _commandText;
        set => _commandText = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override int CommandTimeout { get; set; } = 30;

    public override CommandType CommandType { get; set; } = CommandType.Text;

    public override bool DesignTimeVisible { get; set; }

    public override UpdateRowSource UpdatedRowSource { get; set; }

    protected override DbConnection? DbConnection
    {
        get => _connection;
        set => _connection = (TrinoDbConnection?)value;
    }

    protected override DbParameterCollection DbParameterCollection { get; } = new TrinoDbParameterCollection();

    protected override DbTransaction? DbTransaction { get; set; }

    public override void Cancel()
    {
        // Trino query cancellation would require tracking query IDs
        // For now, this is not implemented
    }

    public override int ExecuteNonQuery()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Connection must be open to execute commands");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("CommandText must be set");

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        var response = client.ExecuteQueryV1(request).GetAwaiter().GetResult();

        // Return 0 as we don't track affected rows in Trino
        return 0;
    }

    public override object? ExecuteScalar()
    {
        using var reader = ExecuteReader();
        if (reader.Read() && reader.FieldCount > 0)
        {
            return reader.GetValue(0);
        }
        return null;
    }

    public override void Prepare()
    {
        // Trino doesn't support prepared statements in the traditional sense
    }

    protected override DbParameter CreateDbParameter()
    {
        return new TrinoDbParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Connection must be open to execute commands");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("CommandText must be set");

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        var response = client.ExecuteQueryV1(request).GetAwaiter().GetResult();

        return new TrinoDbDataReader(response);
    }
}
