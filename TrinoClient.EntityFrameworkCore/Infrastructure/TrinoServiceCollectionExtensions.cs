using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TrinoClient.EntityFrameworkCore.Query;
using TrinoClient.EntityFrameworkCore.Scaffolding;
using TrinoClient.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Infrastructure;

/// <summary>
/// Service collection extensions for Trino
/// </summary>
public static class TrinoServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkTrino(this IServiceCollection services)
    {
        var builder = new EntityFrameworkRelationalServicesBuilder(services)
            .TryAdd<IDatabaseProvider, DatabaseProvider<TrinoOptionsExtension>>()
            .TryAdd<IRelationalConnection, TrinoRelationalConnection>()
            .TryAdd<IRelationalDatabaseCreator, TrinoDatabaseCreator>()
            .TryAdd<IRelationalTypeMappingSource, TrinoTypeMappingSource>()
            .TryAdd<IQuerySqlGeneratorFactory, TrinoQuerySqlGeneratorFactory>()
            .TryAdd<ISqlGenerationHelper, TrinoSqlGenerationHelper>()
            .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, TrinoQueryableMethodTranslatingExpressionVisitorFactory>();

        builder.TryAddCoreServices();

        return services;
    }
}
