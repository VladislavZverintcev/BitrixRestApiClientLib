using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class MessageContent
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "DIALOG_ID")]
        public string DialogId { get; set; }

        [JsonProperty(PropertyName = "MESSAGE")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "SYSTEM")]
        public string System { get; set; }

        [JsonProperty(PropertyName = "ATTACH")]
        public Attachment Attachment { get; set; }

        [JsonProperty(PropertyName = "URL_PREVIEW")]
        public string UrlPreview { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public MessageContent()
        {
            DialogId = "0";
            Message = string.Empty;
            System = "N";
            Attachment = new Attachment();
            UrlPreview = "Y";
        }
        #endregion Public

        #endregion Constructors
    } 
}