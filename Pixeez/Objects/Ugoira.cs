using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class Ugoira
    {
        [JsonProperty("ugoira_metadata")]
        public UgoiraMetadata UgoiraMetadata { get; set; }
    }

    public class UgoiraMetadata
    {
        [JsonProperty("zip_urls")]
        public ZipUrls ZipUrls { get; set; }
        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }
    }

    public class ZipUrls
    {
        [JsonProperty("medium")]
        public string Medium { get; set; }
    }

    public class Frame
    {
        [JsonProperty("file")]
        public string File { get; set; }
        [JsonProperty("delay")]
        public int Delay { get; set; }
    }

}
