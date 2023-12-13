using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class UploadFileInFolderRequest
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "data")]
        public FileContent Data { get; set; }

        [JsonProperty(PropertyName = "fileContent")]
        public string FileContent { get; set; }

        [JsonProperty(PropertyName = "generateUniqueName")]
        public bool GenerateUniqueName { get; set; }

        [JsonProperty(PropertyName = "rights")]
        public List<Right>? Rights { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public UploadFileInFolderRequest()
        {
            Data = new FileContent();
            FileContent = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}