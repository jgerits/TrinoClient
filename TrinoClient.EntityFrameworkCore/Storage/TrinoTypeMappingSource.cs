using Microsoft.EntityFrameworkCore.Storage;

namespace TrinoClient.EntityFrameworkCore.Storage;

/// <summary>
/// Type mapping source for Trino data types
/// </summary>
public class TrinoTypeMappingSource : RelationalTypeMappingSource
{
    public TrinoTypeMappingSource(
        TypeMappingSourceDependencies dependencies,
        RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
    }

    protected override RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType;
        var storeTypeName = mappingInfo.StoreTypeName;

        if (storeTypeName != null)
        {
            return FindMappingByStoreType(storeTypeName);
        }

        if (clrType != null)
        {
            return FindMappingByClrType(clrType);
        }

        return base.FindMapping(mappingInfo);
    }

    private RelationalTypeMapping? FindMappingByStoreType(string storeType)
    {
        var lowerStoreType = storeType.ToLowerInvariant();

        return lowerStoreType switch
        {
            "boolean" => new BoolTypeMapping("boolean"),
            "tinyint" => new ByteTypeMapping("tinyint"),
            "smallint" => new ShortTypeMapping("smallint"),
            "integer" => new IntTypeMapping("integer"),
            "bigint" => new LongTypeMapping("bigint"),
            "real" => new FloatTypeMapping("real"),
            "double" => new DoubleTypeMapping("double"),
            var t when t.StartsWith("decimal") => new DecimalTypeMapping("decimal"),
            var t when t.StartsWith("varchar") => new StringTypeMapping(storeType, System.Data.DbType.String),
            var t when t.StartsWith("char") => new StringTypeMapping(storeType, System.Data.DbType.StringFixedLength),
            "date" => new DateTimeTypeMapping("date", System.Data.DbType.Date),
            var t when t.StartsWith("timestamp") => new DateTimeTypeMapping(storeType, System.Data.DbType.DateTime),
            "uuid" => new GuidTypeMapping("uuid"),
            _ => null
        };
    }

    private RelationalTypeMapping? FindMappingByClrType(Type clrType)
    {
        return clrType switch
        {
            Type t when t == typeof(bool) => new BoolTypeMapping("boolean"),
            Type t when t == typeof(byte) => new ByteTypeMapping("tinyint"),
            Type t when t == typeof(short) => new ShortTypeMapping("smallint"),
            Type t when t == typeof(int) => new IntTypeMapping("integer"),
            Type t when t == typeof(long) => new LongTypeMapping("bigint"),
            Type t when t == typeof(float) => new FloatTypeMapping("real"),
            Type t when t == typeof(double) => new DoubleTypeMapping("double"),
            Type t when t == typeof(decimal) => new DecimalTypeMapping("decimal"),
            Type t when t == typeof(string) => new StringTypeMapping("varchar", System.Data.DbType.String),
            Type t when t == typeof(DateTime) => new DateTimeTypeMapping("timestamp", System.Data.DbType.DateTime),
            Type t when t == typeof(Guid) => new GuidTypeMapping("uuid"),
            Type t when t == typeof(byte[]) => new ByteArrayTypeMapping("varbinary"),
            _ => null
        };
    }
}
