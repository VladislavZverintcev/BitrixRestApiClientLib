using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Models
{
    public class MsgResult
    {
        [JsonProperty(PropertyName = "result")]
        public int result { get; set; }
    }
}
