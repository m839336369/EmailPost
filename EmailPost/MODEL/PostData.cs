using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailPost.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class PostData
    {
        private string token;

        private string to;
        private string to_name;
        [JsonProperty(PropertyName = "params")]
        public List<NameValue> dynamicParams;
        [JsonProperty(PropertyName = "token")]
        public string Token { get => token; set => token = value; }
        [JsonProperty(PropertyName = "to")]
        public string To { get => to; set => to = value; }
        [JsonProperty(PropertyName = "to_name")]
        public string To_name { get => to_name; set => to_name = value; }
    }
}
