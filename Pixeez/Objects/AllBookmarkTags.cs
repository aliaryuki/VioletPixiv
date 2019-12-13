using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class BookmarkTagsList
    {
        [JsonProperty("bookmark_tags")]
        public List<BookmarkTags> BookmarkTags { get; set; }

        [JsonProperty("next_url")]
        public object NextUrl { get; set; }
    }

    public class BookmarkTags
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

}
