using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class UserFollow
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("response")]
        public List<Response> Response { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
    public class Response
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("target_user")]
        public TargetUser TargetUser { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("publicity")]
        public string Publicity { get; set; }
    }
    public class TargetUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

}
