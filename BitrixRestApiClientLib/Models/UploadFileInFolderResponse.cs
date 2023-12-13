using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class UploadFileInFolderResponse
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "result")]
        public File Result { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public UploadFileInFolderResponse()
        {
            Result = new File();
        }
        #endregion Public

        #endregion Constructors
    }
}