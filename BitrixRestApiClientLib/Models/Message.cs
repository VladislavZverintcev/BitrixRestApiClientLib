using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Models
{
    public class Message
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "CHAT_ID")]
        public string ChatID { get; set; }
        [JsonProperty(PropertyName = "AUTHOR_ID")]
        public int AuthorID { get; set; }
        [JsonProperty(PropertyName = "DATE")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "TEXT")]
        public string Text { get; set; }
    }
}
