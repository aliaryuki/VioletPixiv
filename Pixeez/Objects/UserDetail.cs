using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pixeez.Objects
{
    public class HasUser
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class UserDetail : HasUser
    {

        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("profile_publicity")]
        public ProfilePublicity ProfilePublicity { get; set; }

        [JsonProperty("workspace")]
        public Workspace Workspace { get; set; }


    }

    public class ProfilePublicity
    {
        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("birth_day")]
        public string BirthDay { get; set; }

        [JsonProperty("birth_year")]
        public string BirthYear { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("pawoo")]
        public bool Pawoo { get; set; }

    }
}
