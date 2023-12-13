using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class User
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "XML_ID")]
        public int XmlId { get; set; }

        [JsonProperty(PropertyName = "NAME")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "LAST_NAME")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "EMAIL")]
        public string Email { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
        #endregion Public

        #endregion Constructors
    }
}