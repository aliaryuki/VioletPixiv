using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class IllustsComments
    {
        [JsonProperty("total_comments")]
        public int TotalComments { get; set; }

        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }

        [JsonProperty("next_url")]
        public object NextUrl { get; set; }

    }

    public class Comment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("comment")]
        public string CommentContent { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("parent_comment")]
        public ParentComment ParentComment { get; set; }
    }

    public class ParentComment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
