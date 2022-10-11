using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TrinoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.LateralJoinNode.java (internal enum Type)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LateralJoinType
    {
        INNER,
        LEFT
    }
}
