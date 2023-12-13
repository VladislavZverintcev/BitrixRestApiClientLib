using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class FileContent
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "NAME")]
        public string Name { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public FileContent()
        {
            Name = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}