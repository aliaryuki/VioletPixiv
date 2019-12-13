using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Pixeez.Objects;
using System.Linq;
using System.IO;
using System.Text;

namespace Pixeez
{
    public enum MethodType
    {
        GET = 0,
        POST = 1,
        DELETE = 2,
    }

    public class AsyncResponse : IDisposable
    {
        public AsyncResponse(HttpResponseMessage source)
        {
            this.Source = source;
        }
        
        public HttpResponseMessage Source { get; }

        public Task<Stream> GetResponseStreamAsync()
        {
            return this.Source.Content.ReadAsStreamAsync();
        }

        public Task<string> GetResponseStringAsync()
        {
            return this.Source.Content.ReadAsStringAsync();
        }

        public Task<byte[]> GetResponseByteArrayAsync()
        {
            return this.Source.Content.ReadAsByteArrayAsync();
        }

        public bool IsSuccessStatusCode()
        {
            return this.Source.IsSuccessStatusCode;
        }

        public void Dispose()
        {
            this.Source?.Dispose();
        }
    }

    public class Auth
    {
        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> username (required)</para>
        /// <para>- <c>string</c> password (required)</para>
        /// </summary>
        /// <returns>Tokens.</returns>
        public static async Task<Tokens> AuthorizeAsync(string username, string password)
        {
            var httpClient = new HttpClient();

            //2019-10-25T10:45:13+09:00
            var date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Local).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz");

            httpClient.DefaultRequestHeaders.Add("User-Agent", "PixivIOSApp/5.8.0");
            httpClient.DefaultRequestHeaders.Add("X-Client-Time", date);
            httpClient.DefaultRequestHeaders.Add("X-Client-Hash", MD5.MD5code(date + "28c1fdd170a5204386cb1313c7077b34f83e4aaf4aa829ce78c231e05b0bae2c"));

            Console.WriteLine(httpClient.DefaultRequestHeaders);

            var param = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "grant_type", "password" },
                { "client_id", "KzEZED7aC0vird8jWyHM38mXjNTY" },
                { "client_secret", "W9JZoJe00qPvJsiyCGT3CCtC6ZUtdpKpzMbNlUGP" },
            });

            var response = await httpClient.PostAsync("https://oauth.secure.pixiv.net/auth/token", param);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException();

            var json = await response.Content.ReadAsStringAsync();
            var authorize = JToken.Parse(json).SelectToken("response").ToObject<Authorize>();

            return new Tokens(authorize, username, password);
        }

        //public static Tokens AuthorizeWithAccessToken(string accessToken)
        //{
        //    return new Tokens(accessToken, null, null);
        //}
    }

    public class Tokens
    {
        public string AccessToken { get; private set; }
        public Authorize Auth { get; private set; }
        private string username;
        private string password;

        internal Tokens(Authorize auth, string username, string password)
        {
            this.AccessToken = auth.AccessToken;
            this.Auth = auth;
            this.username = username;
            this.password = password;
        }

        private async Task RefreshToken()
        {
            var NewToken = await Pixeez.Auth.AuthorizeAsync(this.username, this.password);
            this.AccessToken = NewToken.AccessToken;
            NewToken = null;
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>MethodType</c> type (required) [ GET, POST ]</para>
        /// <para>- <c>string</c> url (required)</para>
        /// <para>- <c>IDictionary</c> param (required)</para>
        /// <para>- <c>IDictionary</c> header (optional)</para>
        /// </summary>
        /// <returns>AsyncResponse.</returns>
        private async Task<AsyncResponse> SendRequestAsync(MethodType type, string url, IDictionary<string, string> param, IDictionary<string, string> headers = null)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Referer", "http://spapi.pixiv.net/");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "PixivIOSApp/5.8.0");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);

            if (headers != null)
            {
                foreach (var header in headers)
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            AsyncResponse asyncResponse = null;

            if (type == MethodType.POST)
            {
                var reqParam = new FormUrlEncodedContent(param);
                var response = await httpClient.PostAsync(url, reqParam);
                asyncResponse = new AsyncResponse(response);
            }
            else if (type == MethodType.DELETE)
            {
                var uri = url;

                if (param != null)
                {
                    var query_string = "";
                    foreach (KeyValuePair<string, string> kvp in param)
                    {
                        if (query_string == "")
                            query_string += "?";
                        else
                            query_string += "&";

                        query_string += kvp.Key + "=" + WebUtility.UrlEncode(kvp.Value);
                    }
                    uri += query_string;
                }
                
                var response = await httpClient.DeleteAsync(uri);
                asyncResponse = new AsyncResponse(response);
            }
            else
            {
                var uri = url;

                if (param != null)
                {
                    var query_string = "";
                    foreach (KeyValuePair<string, string> kvp in param)
                    {
                        if (query_string == "")
                            query_string += "?";
                        else
                            query_string += "&";

                        query_string += kvp.Key + "=" + WebUtility.UrlEncode(kvp.Value);
                    }
                    uri += query_string;
                }

                var response = await httpClient.GetAsync(uri);
                asyncResponse = new AsyncResponse(response);
            }

            if (!asyncResponse.IsSuccessStatusCode())
            {
                switch (asyncResponse.Source.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        break;

                    default:
                        await RefreshToken();
                        break;
                }
                return await this.SendRequestAsync(type, url, param, headers);
            }

            return asyncResponse;
        }

        public async Task<T> AccessApiAsync<T>(MethodType type, string url, IDictionary<string, string> param = null, IDictionary<string, string> headers = null, bool isAppAPI = false) where T : class
        {
            using (var response = await this.SendRequestAsync(type, url, param, headers))
            {
                var json = await response.GetResponseStringAsync();
                var jsonObj = JToken.Parse(json);
                var obj = isAppAPI ? jsonObj.ToObject<T>() : jsonObj.SelectToken("response").ToObject<T>();

                if (obj is IPagenated)
                    ((IPagenated)obj).Pagination = jsonObj.SelectToken("pagination").ToObject<Pagination>();

                return obj;
            }
        }


        #region Original project

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> illustId (required)</para>
        /// </summary>
        /// <returns>Works.</returns>
        public async Task<List<Work>> GetWorksAsync(long illustId)
        {
            var url = "https://public-api.secure.pixiv.net/v1/works/" + illustId.ToString() + ".json";

            var param = new Dictionary<string, string>
            {
                { "profile_image_sizes", "px_170x170,px_50x50" },
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" },
                { "include_stats", "true" },
            };

            return await this.AccessApiAsync<List<Work>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> authorId (required)</para>
        /// </summary>
        /// <returns>Users.</returns>
        public async Task<List<User>> GetUsersAsync(long authorId)
        {
            var url = "https://public-api.secure.pixiv.net/v1/users/" + authorId.ToString() + ".json";

            var param = new Dictionary<string, string>
            {
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "include_stats", "1" } ,
                { "include_profile", "1" } ,
                { "include_workspace", "1" } ,
                { "include_contacts", "1" } ,
            };

            return await this.AccessApiAsync<List<User>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> authorId (required)</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>UsersWorks. (Pagenated)</returns>
        public async Task<Paginated<UsersWork>> GetMyFollowingWorksAsync(int page = 1, int perPage = 30, string publicity = "public", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/following/works.json";

            var param = new Dictionary<string, string>
            {
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "publicity", publicity } ,
                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<UsersWork>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> authorId (required)</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>UsersWorks. (Pagenated)</returns>
        public async Task<Paginated<UsersWork>> GetUsersWorksAsync(long authorId, int page = 1, int perPage = 30, string publicity = "public", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/users/" + authorId.ToString() + "/works.json";

            var param = new Dictionary<string, string>
            {
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "publicity", publicity } ,
                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<UsersWork>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> authorId (required)</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>UsersFavoriteWorks. (Pagenated)</returns>
        public async Task<Paginated<UsersFavoriteWork>> GetUsersFavoriteWorksAsync(long authorId, int page = 1, int perPage = 30, string publicity = "public", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/users/" + authorId.ToString() + "/favorite_works.json";

            var param = new Dictionary<string, string>
            {
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "publicity", publicity } ,
                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<UsersFavoriteWork>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> mode (optional) [ daily, weekly, monthly, male, female, rookie, daily_r18, weekly_r18, male_r18, female_r18, r18g ]</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> date (optional) [ 2015-04-01 ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>RankingAll. (Pagenated)</returns>
        public async Task<Paginated<Rank>> GetRankingAllAsync(string mode = "daily", int page = 1, int perPage = 30, string date = "", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/ranking/all";

            var param = new Dictionary<string, string>
            {
                { "mode", mode } ,
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            if (!string.IsNullOrWhiteSpace(date))
                param.Add("date", date);

            return await this.AccessApiAsync<Paginated<Rank>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> q (required)</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> mode (optional) [ text, tag, exact_tag, caption ]</para>
        /// <para>- <c>string</c> period (optional) [ all, day, week, month ]</para>
        /// <para>- <c>string</c> order (optional) [ desc, asc ]</para>
        /// <para>- <c>string</c> sort (optional) [ date ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>Works. (Pagenated)</returns>
        public async Task<Paginated<Work>> SearchWorksAsync(string query, int page = 1, int perPage = 30, string mode = "text", string period = "all", string order = "desc", string sort = "date", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/search/works.json";

            var param = new Dictionary<string, string>
            {
                { "q", query } ,
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "period", period } ,
                { "order", order } ,
                { "sort", sort } ,
                { "mode", mode } ,

                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<Work>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>Works. (Pagenated)</returns>
        public async Task<Paginated<Work>> GetLatestWorksAsync(int page = 1, int perPage = 30, bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/works.json";

            var param = new Dictionary<string, string>
            {
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,

                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<Work>>(MethodType.GET, url, param);
        }

        #endregion

        #region New funcions

        /// <summary>
        /// Get user detail
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns> UserDetail </returns>
        public async Task<UserDetail> GetUserDetail(long authorId)
        {
            var url = "https://app-api.pixiv.net/v1/user/detail";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() },
                { "filter", "for_ios" }
            };

            

            return await this.AccessApiAsync<UserDetail>(MethodType.GET, url, param, isAppAPI : true);
        }

        /// <summary>
        /// List ALL illusts data and next page URL
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="illustType"> [illust, manga] </param>
        /// <returns></returns>
        public async Task<IllustsList> GetUserIllusts(long authorId, string illustType = "illust")
        {
            var url = "https://app-api.pixiv.net/v1/user/illusts";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() },
                { "filter", "for_ios" },
                { "type", illustType }
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// List ALL illusts data(bookmarks) and next page URL
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="publicity"> [public, private] </param>
        /// <param name="max_bookmark_id"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetUserBookmarkIllusts(long authorId, string publicity = "public")
        {
            var url = "https://app-api.pixiv.net/v1/user/bookmarks/illust";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() },
                { "filter", "for_ios" },
                { "restrict", publicity }
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Illusts from my following people 
        /// </summary>
        /// <param name="publicity"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetMyFollowIllust(string publicity = "public", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v2/illust/follow";

            var param = new Dictionary<string, string>
            {
                { "restrict", publicity }
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Detail from the specific illust 
        /// </summary>
        /// <param name="illustId"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustDetail(long illustId)
        {
            var url = "https://app-api.pixiv.net/v1/illust/detail";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString() }
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get comments from the specific illust 
        /// </summary>
        /// <param name="illustId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IllustsComments> GetIllustComment(long illustId, int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/illust/comments";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString() }
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsComments>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Related illusts from the spefic illust
        /// </summary>
        /// <param name="illustId"></param>
        /// <param name="seed_illust_ids"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustRelated(long illustId, List<long> seed_illust_ids = null)
        {
            var url = "https://app-api.pixiv.net/v2/illust/related";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString() },
                { "filter", "for_ios"}
            };

            if (seed_illust_ids != null)
                param.Add("seed_illust_ids[]", seed_illust_ids.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Recommended illusts for me
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustRecommended(int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/illust/recommended";

            var param = new Dictionary<string, string>
            {
                { "include_ranking_label", "1"},
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Recommended illusts for me
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetMangaRecommended(int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/manga/recommended";

            var param = new Dictionary<string, string>
            {
                { "include_ranking_label", "1"},
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }


        /// <summary>
        /// Get Recommended illusts for me
        /// </summary>
        /// <param name="illustType"> [illust, manga] </param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustNewRecommended(string illustType = "illust", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/illust/new";

            var param = new Dictionary<string, string>
            {
                { "content_type", illustType },
                { "include_ranking_label", "1"},
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get Ranking illusts for me
        /// </summary>
        /// <param name="dayMode"> [day, week, month, day_male, day_female, week_original
        ///                         , week_rookie,day_r18, day_male_r18, day_female_r18, week_r18, week_r18g] </param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustRanking(string dayMode = "day", int offset = 0, String date = null)
        {
            var url = "https://app-api.pixiv.net/v1/illust/ranking";

            var param = new Dictionary<string, string>
            {
                { "mode", dayMode },
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            if (date != null)
                param.Add("date", date);

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get trend tags and illusts
        /// </summary>
        /// <returns></returns>
        public async Task<TrendTagsList> GetTrendingTagsIllusts()
        {
            var url = "https://app-api.pixiv.net/v1/trending-tags/illust";

            var param = new Dictionary<string, string>
            {
                { "filter", "for_ios"}
            };

            return await this.AccessApiAsync<TrendTagsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get searching illusts by keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="search_target"> [partial_match_for_tags, exact_match_for_tags, title_and_caption] </param>
        /// <param name="sort"> [date_desc, date_asc] </param>
        /// <returns></returns>
        public async Task<IllustsList> SearchIllustsByKeyword(string keyword, string search_target = "partial_match_for_tags", string sort = "date_desc", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/search/illust";

            var param = new Dictionary<string, string>
            {
                { "word", keyword},
                { "search_target", search_target},
                { "sort", sort},
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get searching illusts by keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="search_target"> [partial_match_for_tags, exact_match_for_tags, title_and_caption] </param>
        /// <returns></returns>
        public async Task<IllustsList> SearchIllustsPopularByKeyword(string keyword, string search_target = "partial_match_for_tags", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/search/popular-preview/illust";

            var param = new Dictionary<string, string>
            {
                { "word", keyword},
                { "search_target", search_target},
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get bookmark detail of the illust
        /// </summary>
        /// <param name="illustId"></param>
        /// <returns></returns>
        public async Task<IllustsList> GetIllustBookmarkDetail(long illustId)
        {
            var url = "https://app-api.pixiv.net/v2/illust/bookmark/detail";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString()}
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// [POST] Add the illust to favorite ---> bookmarked
        /// </summary>
        /// <param name="illustId"></param>
        /// <param name="publicity"></param>
        /// <returns></returns>
        public async Task<IllustsList> AddIllustToBookmark(long illustId, string publicity = "public")
        {
            var url = "https://app-api.pixiv.net/v2/illust/bookmark/add";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString()},
                { "restrict", publicity}
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.POST, url, param, isAppAPI: true);
        }

        /// <summary>
        /// [POST] Add the illust from favorite ---> unbookmarked
        /// </summary>
        /// <param name="illustId"></param>
        /// <returns></returns>
        public async Task<IllustsList> DeleteIllustFromBookmarked(long illustId)
        {
            var url = "https://app-api.pixiv.net/v1/illust/bookmark/delete";

            var param = new Dictionary<string, string>
            {
                { "illust_id", illustId.ToString()}
            };

            return await this.AccessApiAsync<IllustsList>(MethodType.POST, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get my bookmark tags
        /// </summary>
        /// <param name="publicity"></param>
        /// <returns></returns>
        public async Task<BookmarkTagsList> GetBookmarkTags(string publicity = "public", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/user/bookmark-tags/illust";

            var param = new Dictionary<string, string>
            {
                { "restrict", publicity }
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<BookmarkTagsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get user following
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="publicity"></param>
        /// <returns></returns>
        public async Task<UserPreviewsList> GetUserFollowing(long authorId, string publicity = "public", int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/user/following";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() },
                { "restrict", publicity }
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<UserPreviewsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get user follower
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<UserPreviewsList> GetUserFollower(long authorId, int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/user/follower";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() }
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<UserPreviewsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get user Pixiv Friends
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<UserPreviewsList> GetUserPixivFriends(long authorId, int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/user/mypixiv";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() }
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<UserPreviewsList>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// Get black list of the user
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<User> GetUserBlcakList(long authorId, int offset = 0)
        {
            var url = "https://app-api.pixiv.net/v1/user/mypixiv";

            var param = new Dictionary<string, string>
            {
                { "user_id", authorId.ToString() },
                { "filter", "for_ios"}
            };

            if (offset != 0)
                param.Add("offset", offset.ToString());

            return await this.AccessApiAsync<User>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// GEt serial frames of the Ugoira which is somthing like GIF.
        /// What is Ugoira : https://www.pixiv.help/hc/en-us/articles/235584628-What-is-Ugoira
        /// </summary>
        /// <param name="illustId"></param>
        /// <returns></returns>
        public async Task<Ugoira> GetUgoiraMetadata(long illustId)
        {
            var url = "https://app-api.pixiv.net/v1/ugoira/metadata";

            var param = new Dictionary<string, string>
            {
                { "user_id", illustId.ToString() }
            };

            return await this.AccessApiAsync<Ugoira>(MethodType.GET, url, param, isAppAPI: true);
        }

        /// <summary>
        /// [POST] Add User To Following
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<UserFollow> AddUserToFollowing(long authorId)
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite-users.json";

            var param = new Dictionary<string, string>
            {
                { "publicity", "public" },
                { "target_user_id", authorId.ToString() }

            };

            return await this.AccessApiAsync<UserFollow>(MethodType.POST, url, param, isAppAPI: true);
        }

        /// <summary>
        /// [DEL] Delete User From Following
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public async Task<UserFollow> DeleteUserFromFollowing(long authorId)
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite-users.json";

            var param = new Dictionary<string, string>
            {
                { "delete_ids", authorId.ToString() },
                { "publicity", "public" }
            };

            return await this.AccessApiAsync<UserFollow>(MethodType.DELETE, url, param, isAppAPI: true);
        }

        #endregion

        #region Not using or Not working

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>int</c> page (optional)</para>
        /// <para>- <c>int</c> perPage (optional)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// <para>- <c>bool</c> includeSanityLevel (optional)</para>
        /// </summary>
        /// <returns>UsersFavoriteWorks. (Pagenated)</returns>
        public async Task<Paginated<UsersFavoriteWork>> GetMyFavoriteWorksAsync(int page = 1, int perPage = 30, string publicity = "public", bool includeSanityLevel = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite_works.json";

            var param = new Dictionary<string, string>
            {
                { "page", page.ToString() } ,
                { "per_page", perPage.ToString() } ,
                { "publicity", publicity } ,
                { "include_stats", "1" } ,
                { "include_sanity_level", Convert.ToInt32(includeSanityLevel).ToString() } ,
                { "image_sizes", "px_128x128,small,medium,large,px_480mw" } ,
                { "profile_image_sizes", "px_170x170,px_50x50" } ,
            };

            return await this.AccessApiAsync<Paginated<UsersFavoriteWork>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> maxId (optional)</para>
        /// <para>- <c>bool</c> showR18 (optional)</para>
        /// </summary>
        /// <returns>Feeds.</returns>
        public async Task<List<Feed>> GetMyFeedsAsync(long maxId = 0, bool showR18 = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/feeds.json";

            var param = new Dictionary<string, string>
            {
                { "relation", "all" } ,
                { "type", "touch_nottext" } ,
                { "show_r18", Convert.ToInt32(showR18).ToString() } ,
            };

            if (maxId != 0)
                param.Add("max_id", maxId.ToString());

            return await this.AccessApiAsync<List<Feed>>(MethodType.GET, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> workID (required)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// </summary>
        /// <returns>UsersWorks. (Pagenated)</returns>
        public async Task<List<UsersFavoriteWork>> AddMyFavoriteWorksAsync(long workId, string comment = "", IEnumerable<string> tags = null, string publicity = "public")
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite_works.json";

            var param = new Dictionary<string, string>
            {
                { "work_id", workId.ToString() } ,
                { "publicity", publicity } ,
                { "comment", comment } ,
            };

            if (tags != null)
                param.Add("tags", string.Join(",", tags));

            return await this.AccessApiAsync<List<UsersFavoriteWork>>(MethodType.POST, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>IEnumerable</c> workIds (required)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// </summary>
        /// <returns>UsersWorks. (Pagenated)</returns>
        public async Task<List<UsersFavoriteWork>> DeleteMyFavoriteWorksAsync(IEnumerable<long> workIds, string publicity = "public")
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite_works.json";

            var param = new Dictionary<string, string>
            {
                { "work_id", string.Join(",", workIds.Select(x => x.ToString())) } ,
                { "publicity", publicity } ,
            };

            return await this.AccessApiAsync<List<UsersFavoriteWork>>(MethodType.DELETE, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> workId (required)</para>
        /// <para>- <c>string</c> publicity (optional) [ public, private ]</para>
        /// </summary>
        /// <returns>UsersWorks. (Pagenated)</returns>
        public async Task<Paginated<UsersFavoriteWork>> DeleteMyFavoriteWorksAsync(long workId, string publicity = "public")
        {
            var url = "https://public-api.secure.pixiv.net/v1/me/favorite_works.json";

            var param = new Dictionary<string, string>
            {
                { "work_id", workId.ToString() } ,
                { "publicity", publicity } ,
            };

            return await this.AccessApiAsync<Paginated<UsersFavoriteWork>>(MethodType.DELETE, url, param);
        }

        /// <summary>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> maxId (optional)</para>
        /// <para>- <c>bool</c> showR18 (optional)</para>
        /// </summary>
        /// <returns>Feed.</returns>
        public async Task<List<Feed>> GetUsersFeedsAsync(long authorId, long maxId = 0, bool showR18 = true)
        {
            var url = "https://public-api.secure.pixiv.net/v1/users/" + authorId.ToString() + "/feeds.json";

            var param = new Dictionary<string, string>
            {
                { "relation", "all" } ,
                { "type", "touch_nottext" } ,
                { "show_r18", Convert.ToInt32(showR18).ToString() } ,
            };

            if (maxId != 0)
                param.Add("max_id", maxId.ToString());

            return await this.AccessApiAsync<List<Feed>>(MethodType.GET, url, param);
        }


        #endregion 

    }

    public class MD5
    {
        public static String MD5code(String str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            Byte[] data = md5Hasher.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(str));
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}
