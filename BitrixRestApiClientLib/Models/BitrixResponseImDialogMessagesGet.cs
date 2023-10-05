using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Models
{
    public class BitrixResponseImDialogMessagesGet
    {
        [JsonProperty(PropertyName = "result")]
        public ResultOfMessages Result { get; set; }
    }
    public class ResultOfMessages
    {
        [JsonProperty(PropertyName = "messages")]
        public List<Message> Messages { get; set; }
    }
}
