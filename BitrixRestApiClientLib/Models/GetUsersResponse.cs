using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class GetUsersResponse
    {
        #region Properties

        #region Public
        [JsonProperty(PropertyName = "result")]
        public List<User> Result { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public GetUsersResponse()
        {
            Result = new List<User>();
        }
        #endregion Public

        #endregion Constructors
    }
}