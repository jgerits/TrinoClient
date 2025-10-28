using System.Collections;
using System.Data;
using System.Data.Common;
using TrinoClient.Model.Statement;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbDataReader implementation for Trino
/// </summary>
public class TrinoDbDataReader : DbDataReader
{
    private readonly ExecuteQueryV1Response _response;
    private readonly List<List<object?>> _data;
    private int _currentRow = -1;
    private bool _isClosed = false;
    private DbConnection? _connectionToClose;

    public TrinoDbDataReader(ExecuteQueryV1Response response)
    {
        _response = response ?? throw new ArgumentNullException(nameof(response));
        _data = response.Data?.Select(row => row.Cast<object?>().ToList()).ToList() ?? new List<List<object?>>();
    }

    internal void SetCloseConnection(DbConnection connection)
    {
        _connectionToClose = connection;
    }

    public override object this[int ordinal] => GetValue(ordinal);

    public override object this[string name] => GetValue(GetOrdinal(name));

    public override int Depth => 0;

    public override int FieldCount => _response.Columns?.Count ?? 0;

    public override bool HasRows => _data.Count > 0;

    public override bool IsClosed => _isClosed;

    public override int RecordsAffected => -1;

    public override bool GetBoolean(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is bool b ? b : Convert.ToBoolean(value);
    }

    public override byte GetByte(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is byte b ? b : Convert.ToByte(value);
    }

    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        throw new NotSupportedException();
    }

    public override char GetChar(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is char c ? c : Convert.ToChar(value);
    }

    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
    {
        throw new NotSupportedException();
    }

    public override string GetDataTypeName(int ordinal)
    {
        if (_response.Columns == null || ordinal < 0 || ordinal >= _response.Columns.Count)
            throw new IndexOutOfRangeException();

        return _response.Columns[ordinal].Type ?? "unknown";
    }

    public override DateTime GetDateTime(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is DateTime dt ? dt : Convert.ToDateTime(value);
    }

    public override decimal GetDecimal(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is decimal d ? d : Convert.ToDecimal(value);
    }

    public override double GetDouble(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is double dbl ? dbl : Convert.ToDouble(value);
    }

    public override Type GetFieldType(int ordinal)
    {
        if (_response.Columns == null || ordinal < 0 || ordinal >= _response.Columns.Count)
            throw new IndexOutOfRangeException();

        var trinoType = _response.Columns[ordinal].Type?.ToLowerInvariant() ?? "unknown";
        return MapTrinoTypeToClrType(trinoType);
    }

    public override float GetFloat(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is float f ? f : Convert.ToSingle(value);
    }

    public override Guid GetGuid(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is Guid g ? g : Guid.Parse(value?.ToString() ?? string.Empty);
    }

    public override short GetInt16(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is short s ? s : Convert.ToInt16(value);
    }

    public override int GetInt32(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is int i ? i : Convert.ToInt32(value);
    }

    public override long GetInt64(int ordinal)
    {
        var value = GetValue(ordinal);
        return value is long l ? l : Convert.ToInt64(value);
    }

    public override string GetName(int ordinal)
    {
        if (_response.Columns == null || ordinal < 0 || ordinal >= _response.Columns.Count)
            throw new IndexOutOfRangeException();

        return _response.Columns[ordinal].Name ?? string.Empty;
    }

    public override int GetOrdinal(string name)
    {
        if (_response.Columns == null)
            throw new InvalidOperationException("No columns available");

        for (int i = 0; i < _response.Columns.Count; i++)
        {
            if (string.Equals(_response.Columns[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        throw new IndexOutOfRangeException($"Column '{name}' not found");
    }

    public override string GetString(int ordinal)
    {
        var value = GetValue(ordinal);
        return value?.ToString() ?? string.Empty;
    }

    public override object GetValue(int ordinal)
    {
        if (_currentRow < 0 || _currentRow >= _data.Count)
            throw new InvalidOperationException("Invalid position for reading data");

        if (ordinal < 0 || ordinal >= _data[_currentRow].Count)
            throw new IndexOutOfRangeException();

        return _data[_currentRow][ordinal] ?? DBNull.Value;
    }

    public override int GetValues(object[] values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        int count = Math.Min(values.Length, FieldCount);
        for (int i = 0; i < count; i++)
        {
            values[i] = GetValue(i);
        }
        return count;
    }

    public override bool IsDBNull(int ordinal)
    {
        var value = GetValue(ordinal);
        return value == null || value == DBNull.Value;
    }

    public override bool NextResult()
    {
        return false;
    }

    public override bool Read()
    {
        if (_isClosed)
            return false;

        _currentRow++;
        return _currentRow < _data.Count;
    }

    public override IEnumerator GetEnumerator()
    {
        return new DbEnumerator(this);
    }

    public override void Close()
    {
        if (_isClosed)
            return;

        _isClosed = true;
        
        // Close connection if CommandBehavior.CloseConnection was specified
        if (_connectionToClose != null)
        {
            _connectionToClose.Close();
            _connectionToClose = null;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Close();
        }
        base.Dispose(disposing);
    }

    private static Type MapTrinoTypeToClrType(string trinoType)
    {
        return trinoType switch
        {
            var t when t.StartsWith("boolean") => typeof(bool),
            var t when t.StartsWith("tinyint") => typeof(sbyte),
            var t when t.StartsWith("smallint") => typeof(short),
            var t when t.StartsWith("integer") => typeof(int),
            var t when t.StartsWith("bigint") => typeof(long),
            var t when t.StartsWith("real") => typeof(float),
            var t when t.StartsWith("double") => typeof(double),
            var t when t.StartsWith("decimal") => typeof(decimal),
            var t when t.StartsWith("varchar") => typeof(string),
            var t when t.StartsWith("char") => typeof(string),
            var t when t.StartsWith("varbinary") => typeof(byte[]),
            var t when t.StartsWith("date") => typeof(DateTime),
            var t when t.StartsWith("time") => typeof(TimeSpan),
            var t when t.StartsWith("timestamp") => typeof(DateTime),
            var t when t.StartsWith("uuid") => typeof(Guid),
            _ => typeof(object)
        };
    }
}
