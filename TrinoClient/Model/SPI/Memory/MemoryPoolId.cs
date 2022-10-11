using TrinoClient.Serialization;
using Newtonsoft.Json;

namespace TrinoClient.Model.SPI.Memory
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class MemoryPoolId
    {
        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public MemoryPoolId(string id)
        {
            ParameterCheck.NotNullOrEmpty(id, "id");

            this.Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Id;
        }

        #endregion
    }
}
