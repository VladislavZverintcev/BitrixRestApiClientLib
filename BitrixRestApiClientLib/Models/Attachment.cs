using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class Attachment
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "BLOCKS")]
        public List<object> Blocks { get; set; }
        #endregion Public

        #endregion Properties

        #region Properties

        #region Public
        public Attachment()
        {
            Blocks = new List<object>();
        }
        #endregion Public

        #endregion Properties
    }
}