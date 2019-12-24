using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class UserPreviewsList : HasNextUrl
    {
        public override IEnumerable<object> GetList()
        {
            return this.UserPreviews;
        }

        [JsonProperty("user_previews")]
        public List<UserPreviews> UserPreviews { get; set; }

    }

    public class UserPreviews : IHasUser, IMainInfoObject
    {

        [JsonProperty("illusts")]
        public List<Illust> Illusts { get; set; }

        [JsonProperty("novels")]
        public List<object> Novels { get; set; }

        [JsonProperty("is_muted")]
        public bool IsMuted { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
