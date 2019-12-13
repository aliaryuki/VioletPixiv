using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public abstract class HasNextUrl<T> {

        public virtual List<T> GetList()
        {
            throw new NotImplementedException();
        }

        [JsonProperty("next_url")]
        public object NextUrl { get; set; }

    }

    public class IllustsList : HasNextUrl<Illust>
    {
        public override List<Illust> GetList()
        {
            return this.Illusts;
        }

        [JsonProperty("illusts")]
        public List<Illust> Illusts { get; set; }

        [JsonProperty("illust")]
        public Illust Illust { get; set; }

    }

    public class Illust
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string  Title { get; set; }

        [JsonProperty("type")]
        public string  Type { get; set; }

        [JsonProperty("image_urls")]
        public ImageUrls ImageUrls { get; set; }

        [JsonProperty("caption")]
        public string  Caption { get; set; }

        [JsonProperty("restrict")]
        public int? Restrict { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("tools")]
        public List<string> Tools { get; set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("page_count")]
        public int? PageCount { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("sanity_level")]
        public int? SanityLevel { get; set; }

        [JsonProperty("x_restrict")]
        public int? XRestrict { get; set; }

        [JsonProperty("series")]
        public object Series { get; set; }

        [JsonProperty("meta_single_page")]
        public MetaSinglePage MetaSinglePage { get; set; }

        [JsonProperty("meta_pages")]
        public List<MetaPages> MetaPages { get; set; }

        [JsonProperty("total_view")]
        public int? TotalView { get; set; }

        [JsonProperty("total_bookmarks")]
        public int? TotalBookmarks { get; set; }

        [JsonProperty("is_bookmarked")]
        public bool IsBookmarked { get; set; }

        [JsonProperty("visible")]
        public bool? Visible { get; set; }

        [JsonProperty("is_muted")]
        public bool? IsMuted { get; set; }

        [JsonProperty("total_comments")]
        public int? TotalComments { get; set; }
    }

    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("translated_name")]
        public string TranslatedName { get; set; }
    }

    public class MetaPages
    {
        [JsonProperty("image_urls")]
        public ImageUrls1 ImageUrls { get; set; }
    }

    public class ImageUrls1
    {
        [JsonProperty("square_medium")]
        public string SquareMedium { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }
    }

    public class MetaSinglePage
    {
        [JsonProperty("original_image_url")]
        public string OriginalImageUrl { get; set; }
    }


}
