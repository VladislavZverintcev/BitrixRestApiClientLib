using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public abstract class AttachObj
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "NAME")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "LINK")]
        public string DownloadUrl { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public AttachObj()
        {
            Name = string.Empty;
            DownloadUrl = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}