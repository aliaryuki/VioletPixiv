using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class TrendTagsList
    {
        [JsonProperty("trend_tags")]
        public List<TrendTags> TrendTags { get; set; }
    }

    public class TrendTags
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [JsonProperty("translated_name")]
        public string TranslatedName { get; set; }
        [JsonProperty("illust")]
        public Illust Illust { get; set; }
    }


}
