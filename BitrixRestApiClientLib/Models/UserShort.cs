using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Models
{
    public class UserShort
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "XML_ID")]
        public int Xml_ID { get; set; }
        [JsonProperty(PropertyName = "NAME")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "LAST_NAME")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "EMAIL")]
        public string Email { get; set; }

    }
}
