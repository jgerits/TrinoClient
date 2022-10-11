using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TrinoClient.Model.Server
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ThreadState
    {
        TIMED_WAITING,
        WAITING,
        RUNNABLE,
        FAILED
    }
}
