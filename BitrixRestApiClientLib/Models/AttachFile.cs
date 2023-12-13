using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class AttachFile
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "FILE")]
        public BaseFile File { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public AttachFile()
        {
            File = new BaseFile();
        }
        #endregion Public

        #endregion Constructors
    }
}