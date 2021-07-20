using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailPost.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Config
    {
        private string token;
        private int minTime;
        private int maxTime;
        private string emailTo;
        private string senderName;
        private string uRL;
        public string Token { get => token; set => token = value; }
        public int MinTime { get => minTime; set => minTime = value; }
        public int MaxTime { get => maxTime; set => maxTime = value; }
        public string EmailTo { get => emailTo; set => emailTo = value; }
        public string SenderName { get => senderName; set => senderName = value; }
        public string URL { get => uRL; set => uRL = value; }
    }
}
