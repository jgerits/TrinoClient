using TrinoClient.Serialization;
using Newtonsoft.Json;

namespace TrinoClient.Model.SPI.Connector
{
    /// <summary>
    /// From com.facebook.presto.spi.connector.ConnectorPartitioningHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorPartitioningHandle
    {
        bool IsSingleNode();

        bool IsCoordinatorOnly();
    }
}
