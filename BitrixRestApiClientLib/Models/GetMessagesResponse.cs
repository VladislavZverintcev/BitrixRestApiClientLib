using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class GetMessagesResponse
    {
        #region Classes

        #region Public
        public class ResultMessages
        {
            #region Properties

            #region Public
            [JsonProperty(PropertyName = "messages")]
            public List<BaseMessage> Messages { get; set; }
            #endregion Public

            #endregion Properties

            #region Constructors

            #region Public
            public ResultMessages()
            {
                Messages = new List<BaseMessage>();
            }
            #endregion Public

            #endregion Constructors
        }
        #endregion Public

        #endregion Classes

        #region Properties

        #region Public
        [JsonProperty(PropertyName = "result")]
        public ResultMessages Result { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public GetMessagesResponse()
        {
            Result = new ResultMessages();
        }
        #endregion Public

        #endregion Constructors
    }
}