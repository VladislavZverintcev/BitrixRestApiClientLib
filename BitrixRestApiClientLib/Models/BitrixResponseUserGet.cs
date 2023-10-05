using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Models
{
    public class BitrixResponseUserGet
    {
        [JsonProperty(PropertyName = "result")]
        public List<UserShort> Result { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }
}
