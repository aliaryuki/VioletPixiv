using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class UserStats
    {

        [JsonProperty("works")]
        public int? Works { get; set; }

        [JsonProperty("favorites")]
        public int? Favorites { get; set; }

        [JsonProperty("following")]
        public int? Following { get; set; }

        [JsonProperty("friends")]
        public int? Friends { get; set; }
    }

    public class Contacts
    {

        [JsonProperty("twitter")]
        public string Twitter { get; set; }
    }

    public class Workspace
    {

        [JsonProperty("pc")]
        public string Pc { get; set; }

        [JsonProperty("monitor")]
        public string Monitor { get; set; }

        [JsonProperty("tool")]
        public string Tool { get; set; }

        [JsonProperty("scanner")]
        public string Scanner { get; set; }

        [JsonProperty("tablet")]
        public string Tablet { get; set; }

        [JsonProperty("mouse")]
        public string Mouse { get; set; }

        [JsonProperty("printer")]
        public string Printer { get; set; }

        [JsonProperty("desktop")]
        public string Desktop { get; set; }

        [JsonProperty("music")]
        public string Music { get; set; }

        [JsonProperty("desk")]
        public string Desk { get; set; }

        [JsonProperty("chair")]
        public string Chair { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("workspace_image_url")]
        public string WorkspaceImageUrl { get; set; }
    }

    public class Profile
    {

        [JsonProperty("contacts")]
        public Contacts Contacts { get; set; }

        [JsonProperty("workspace")]
        public Workspace Workspace { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("introduction")]
        public string Introduction { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("tags")]
        public object Tags { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("blood_type")]
        public string BloodType { get; set; }

        [JsonProperty("webpage")]
        public string Webpage { get; set; }

        [JsonProperty("birth")]
        public string Birth { get; set; }

        [JsonProperty("birth_day")]
        public string BirthDay { get; set; }

        [JsonProperty("birth_year")]
        public int BirthYear { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("address_id")]
        public int AddressId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("job_id")]
        public int JobId { get; set; }

        [JsonProperty("total_follow_users")]
        public int TotalFollowUsers { get; set; }

        [JsonProperty("total_mypixiv_users")]
        public int TotalMypixivUsers { get; set; }

        [JsonProperty("total_illusts")]
        public int TotalIllusts { get; set; }

        [JsonProperty("total_manga")]
        public int TotalManga { get; set; }

        [JsonProperty("total_novels")]
        public int TotalNovels { get; set; }

        [JsonProperty("total_illust_bookmarks_public")]
        public int TotalIllustBookmarksPublic { get; set; }

        [JsonProperty("total_illust_series")]
        public int TotalIllustSeries { get; set; }

        [JsonProperty("total_novel_series")]
        public int TotalNovelSeries { get; set; }

        [JsonProperty("background_image_url")]
        public string BackgroundImageUrl { get; set; }

        [JsonProperty("twitter_account")]
        public string TwitterAccount { get; set; }

        [JsonProperty("twitter_url")]
        public string TwitterUrl { get; set; }

        [JsonProperty("pawoo_url")]
        public string PawooUrl { get; set; }

        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        [JsonProperty("is_using_custom_profile_image")]
        public bool IsUsingCustomProfileImage { get; set; }
    }

    public class User
    {

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("is_following")]
        public bool? IsFollowing { get; set; }

        [JsonProperty("is_follower")]
        public bool? IsFollower { get; set; }

        [JsonProperty("is_friend")]
        public bool? IsFriend { get; set; }

        [JsonProperty("is_premium")]
        public bool? IsPremium { get; set; }

        [JsonProperty("profile_image_urls")]
        public ProfileImageUrls ProfileImageUrls { get; set; }

        [JsonProperty("stats")]
        public UserStats Stats { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("is_followed")]
        public bool IsFollowed { get; set; }
    }
}
