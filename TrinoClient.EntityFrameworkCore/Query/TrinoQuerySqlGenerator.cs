using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace TrinoClient.EntityFrameworkCore.Query;

/// <summary>
/// SQL generator for Trino queries
/// </summary>
public class TrinoQuerySqlGenerator : QuerySqlGenerator
{
    public TrinoQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override void GenerateLimitOffset(SelectExpression selectExpression)
    {
        if (selectExpression.Limit != null)
        {
            Sql.AppendLine().Append("LIMIT ");
            Visit(selectExpression.Limit);
        }

        if (selectExpression.Offset != null)
        {
            if (selectExpression.Limit == null)
            {
                // Trino requires LIMIT when using OFFSET
                Sql.AppendLine().Append("LIMIT 9223372036854775807");
            }
            
            Sql.AppendLine().Append("OFFSET ");
            Visit(selectExpression.Offset);
        }
    }

    protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
    {
        // Handle any Trino-specific binary operations
        return base.VisitSqlBinary(sqlBinaryExpression);
    }

    protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
    {
        // Handle any Trino-specific functions
        return base.VisitSqlFunction(sqlFunctionExpression);
    }
}

/// <summary>
/// Factory for creating Trino query SQL generators
/// </summary>
public class TrinoQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
{
    private readonly QuerySqlGeneratorDependencies _dependencies;

    public TrinoQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public QuerySqlGenerator Create()
    {
        return new TrinoQuerySqlGenerator(_dependencies);
    }
}
