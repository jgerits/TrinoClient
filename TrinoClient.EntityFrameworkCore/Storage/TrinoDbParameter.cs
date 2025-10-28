using System.Data;
using System.Data.Common;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// ADO.NET DbParameter implementation for Trino
/// Note: Trino doesn't support parameterized queries in the traditional sense,
/// but this is required for ADO.NET compliance
/// </summary>
public class TrinoDbParameter : DbParameter
{
    private string _parameterName = string.Empty;
    private object? _value;

    public override DbType DbType { get; set; }

    public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;

    public override bool IsNullable { get; set; }

    public override string? ParameterName
    {
        get => _parameterName;
        set => _parameterName = value ?? string.Empty;
    }

    public override int Size { get; set; }

    public override string? SourceColumn { get; set; }

    public override bool SourceColumnNullMapping { get; set; }

    public override object? Value
    {
        get => _value;
        set => _value = value;
    }

    public override void ResetDbType()
    {
        DbType = DbType.Object;
    }
}
