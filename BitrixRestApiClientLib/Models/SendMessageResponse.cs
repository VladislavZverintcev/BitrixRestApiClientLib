using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class SendMessageResponse
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "result")]
        public int Result { get; set; }
        #endregion Public

        #endregion Properties
    }
}