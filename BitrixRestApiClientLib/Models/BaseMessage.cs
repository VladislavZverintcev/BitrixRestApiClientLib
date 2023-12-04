using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class BaseMessage
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "CHAT_ID")]
        public int ChatId { get; set; }

        [JsonProperty(PropertyName = "AUTHOR_ID")]
        public int AuthorId { get; set; }

        [JsonProperty(PropertyName = "DATE")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "TEXT")]
        public string Text { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public BaseMessage()
        {
            Date = string.Empty;
            Text = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}