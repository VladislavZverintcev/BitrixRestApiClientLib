using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class Right
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "TASK_ID")]
        public int TaskId { get; set; }

        [JsonProperty(PropertyName = "ACCESS_CODE")]
        public string AccessCode { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructotrs

        #region Public
        public Right()
        {
            AccessCode = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}