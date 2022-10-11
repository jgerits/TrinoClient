using TrinoClient.Serialization;
using Newtonsoft.Json;

namespace TrinoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorIndexHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorIndexHandle
    {
        // Intentionally empty
    }
}
