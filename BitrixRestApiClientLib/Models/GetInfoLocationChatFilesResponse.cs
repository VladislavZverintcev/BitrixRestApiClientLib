using Newtonsoft.Json;

namespace BitrixRestApiClientLib.Models
{
    public class GetInfoLocationChatFilesResponse
    {
        #region Classes

        #region Public
        public class ResultInfoLocationChatFiles
        {
            #region Properties

            #region Public
            [JsonProperty(PropertyName = "ID")]
            public int Id { get; set; }
            #endregion Public

            #endregion Properties

            #region Constructors 

            #region Public
            public ResultInfoLocationChatFiles()
            {

            }
            #endregion Public

            #endregion Constructors
        }
        #endregion Public

        #endregion Classes

        #region Properties

        #region Public
        [JsonProperty(PropertyName = "result")]
        public ResultInfoLocationChatFiles Result { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public GetInfoLocationChatFilesResponse()
        {
            Result = new ResultInfoLocationChatFiles();
        }
        #endregion Public

        #endregion Constructors
    }
}