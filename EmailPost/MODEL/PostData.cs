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

        public string Token { get => token; set => token = value; }
        public string To { get => to; set => to = value; }
        public string To_name { get => to_name; set => to_name = value; }
    }
}
