using TrinoClient.Serialization;
using Newtonsoft.Json;

namespace TrinoClient.Model
{
    /// <summary>
    /// From io.airlift.units.Duration.java
    /// </summary>
    [JsonConverter(typeof(TimeSpanConverter))]
    public class Duration
    {

    }
}
