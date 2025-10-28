using System.Data;
using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbCommand implementation for Trino
/// </summary>
/// <remarks>
/// Note: The synchronous Execute methods (ExecuteNonQuery, ExecuteReader, ExecuteScalar) use GetAwaiter().GetResult()
/// internally due to the asynchronous nature of the underlying TrinodbClient. While this is an architectural limitation
/// of ADO.NET, it may cause deadlocks in UI or ASP.NET contexts. Prefer using the async versions (ExecuteNonQueryAsync,
/// ExecuteReaderAsync, ExecuteScalarAsync) whenever possible.
/// </remarks>
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

    public override string? CommandText
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
        if (_connection == null)
            throw new InvalidOperationException("ExecuteNonQuery requires an open and available connection. The connection's current state is closed.");
        
        if (_connection.State != ConnectionState.Open)
            throw new InvalidOperationException($"ExecuteNonQuery requires an open connection. The connection's current state is {_connection.State}.");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("ExecuteNonQuery: CommandText property has not been initialized.");

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        var response = client.ExecuteQueryV1(request).GetAwaiter().GetResult();

        // Return 0 as we don't track affected rows in Trino
        return 0;
    }

    public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
        if (_connection == null)
            throw new InvalidOperationException("ExecuteNonQueryAsync requires an open and available connection. The connection's current state is closed.");
        
        if (_connection.State != ConnectionState.Open)
            throw new InvalidOperationException($"ExecuteNonQueryAsync requires an open connection. The connection's current state is {_connection.State}.");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("ExecuteNonQueryAsync: CommandText property has not been initialized.");

        cancellationToken.ThrowIfCancellationRequested();

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        await client.ExecuteQueryV1(request).ConfigureAwait(false);

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

    public override async Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken)
    {
        using var reader = await ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false) && reader.FieldCount > 0)
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
        if (_connection == null)
            throw new InvalidOperationException("ExecuteReader requires an open and available connection. The connection's current state is closed.");
        
        if (_connection.State != ConnectionState.Open)
            throw new InvalidOperationException($"ExecuteReader requires an open connection. The connection's current state is {_connection.State}.");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("ExecuteReader: CommandText property has not been initialized.");

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        var response = client.ExecuteQueryV1(request).GetAwaiter().GetResult();

        var reader = new TrinoDbDataReader(response);
        
        // Honor CommandBehavior.CloseConnection
        if ((behavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection)
        {
            reader.SetCloseConnection(_connection);
        }

        return reader;
    }

    protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
        if (_connection == null)
            throw new InvalidOperationException("ExecuteReaderAsync requires an open and available connection. The connection's current state is closed.");
        
        if (_connection.State != ConnectionState.Open)
            throw new InvalidOperationException($"ExecuteReaderAsync requires an open connection. The connection's current state is {_connection.State}.");

        if (string.IsNullOrWhiteSpace(_commandText))
            throw new InvalidOperationException("ExecuteReaderAsync: CommandText property has not been initialized.");

        cancellationToken.ThrowIfCancellationRequested();

        var client = _connection.GetTrinoClient();
        var request = new Model.Statement.ExecuteQueryV1Request(_commandText);
        var response = await client.ExecuteQueryV1(request).ConfigureAwait(false);

        var reader = new TrinoDbDataReader(response);
        
        // Honor CommandBehavior.CloseConnection
        if ((behavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection)
        {
            reader.SetCloseConnection(_connection);
        }

        return reader;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection = null;
        }
        base.Dispose(disposing);
    }
}
