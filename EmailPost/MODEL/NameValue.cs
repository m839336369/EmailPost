using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailPost.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class NameValue
    {
        private string name;
        private string value;

        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }

        public NameValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
