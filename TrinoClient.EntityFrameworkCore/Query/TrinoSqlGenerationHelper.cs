using Microsoft.EntityFrameworkCore.Storage;
using System.Text;

namespace TrinoClient.EntityFrameworkCore.Query;

/// <summary>
/// SQL generation helper for Trino
/// </summary>
public class TrinoSqlGenerationHelper : RelationalSqlGenerationHelper
{
    public TrinoSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies)
        : base(dependencies)
    {
    }

    public override string StatementTerminator => ";";

    public override string StartTransactionStatement => throw new NotSupportedException("Trino does not support transactions");

    public override string DelimitIdentifier(string identifier)
    {
        return $"\"{EscapeIdentifier(identifier)}\"";
    }

    public override void DelimitIdentifier(StringBuilder builder, string identifier)
    {
        builder.Append('"');
        EscapeIdentifier(builder, identifier);
        builder.Append('"');
    }

    public override string EscapeIdentifier(string identifier)
    {
        return identifier.Replace("\"", "\"\"");
    }

    public override void EscapeIdentifier(StringBuilder builder, string identifier)
    {
        var escapedIdentifier = EscapeIdentifier(identifier);
        builder.Append(escapedIdentifier);
    }

    public override string GenerateParameterName(string name)
    {
        // Trino doesn't support parameterized queries in the traditional sense
        return name;
    }

    public override void GenerateParameterName(StringBuilder builder, string name)
    {
        builder.Append(name);
    }
}
