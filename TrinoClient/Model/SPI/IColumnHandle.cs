using TrinoClient.Serialization;
using Newtonsoft.Json;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ColumnHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IColumnHandle
    {
        // Intentionally empty
    }
}
