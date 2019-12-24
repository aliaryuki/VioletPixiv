using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv.ViewModel
{
    /// <summary>
    /// Template
    /// </summary>
    public class CollectionViewModelTemplate<T1, T2> : NotifyImplementClass
        where T1 : NeedToLoadImages
        where T2 : HasNextUrl
    {
        protected string CollectionNextUrl = "";
        protected Stack<T1> LoadingImagesCollectionStack = new Stack<T1>();

        #region CollectionListPartial PropertyChanged

        protected ObservableCollection<T1> _CollectionListPartial = new ObservableCollection<T1>();

        public ObservableCollection<T1> CollectionListPartial
        {
            get { return this._CollectionListPartial; }
            set {
                this._CollectionListPartial = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Init All Data
        /// </summary>
        public void InitializeData()
        {
            this.CollectionNextUrl = "";
            this.LoadingImagesCollectionStack = new Stack<T1>();
            this.CollectionListPartial = new ObservableCollection<T1>();
        }

        /// <summary>
        /// Update Data to View 
        /// </summary>
        public async Task ShowCollectionTemplate(int NumNeedToShow)
        {
            
            this.LoadingImagesCollectionStack = (this.LoadingImagesCollectionStack.Count == 0)? await this.GetLoadingImagesCollectionStack() : 
                                                                                                      this.LoadingImagesCollectionStack;

            var AlreadyShowNum = this.PopCollectionStackAndPropertyChangeToView(this.LoadingImagesCollectionStack, NumNeedToShow);

            if (AlreadyShowNum < NumNeedToShow) await this.ShowCollectionTemplate(NumNeedToShow - AlreadyShowNum);

        }

        protected int PopCollectionStackAndPropertyChangeToView(Stack<T1> CollectionStack, int NumNeedToShow)
        {
            int NumToShowCounter = 0;
            var CollectionListTmp = CollectionListPartial;

            while (this.LoadingImagesCollectionStack.Count != 0 && NumToShowCounter < NumNeedToShow)
            {
                var CollectionElement = this.LoadingImagesCollectionStack.Pop();
                _ = CollectionElement.GetAllImage();
                CollectionListTmp.Add(CollectionElement);
                NumToShowCounter++;
            }
            // Update To View
            CollectionListPartial = CollectionListTmp;

            return NumToShowCounter;
        }

        protected virtual Task<T2> GetCollectionEnumerable() {
            throw new NotImplementedException();
        }

        protected virtual Task<T2> GetNextCollectionEnumerable(string nextURL) {
            return MainWindow.PixivWindow.AuthToken.AccessApiAsync<T2>(Pixeez.MethodType.GET, nextURL, null, isAppAPI: true);
        }

        protected async Task<Stack<T1>> GetLoadingImagesCollectionStack()
        {
            if (this.CollectionNextUrl == null) return null;

            var LoadingImagesCollectionStack = new Stack<T1>();
            foreach (var PerNotLoaingImagesCollection in (await this.GetNotLoaingImagesCollection()).GetList().AsEnumerable().Reverse())
            {
                LoadingImagesCollectionStack.Push((T1)Activator.CreateInstance(typeof(T1), new object[] { PerNotLoaingImagesCollection, false, false }));
            }
            return LoadingImagesCollectionStack;
        }

        protected async Task<T2> GetNotLoaingImagesCollection()
        {
            var CollectionList = (this.CollectionNextUrl == "") ? await this.GetCollectionEnumerable() : 
                                                                  await this.GetNextCollectionEnumerable(CollectionNextUrl);
            this.CollectionNextUrl = (string)CollectionList.NextUrl;

            return CollectionList;
        }

    }

    /// <summary>
    /// RecommendPage
    /// </summary>
    public class IllustsRecommendedViewModel : CollectionViewModelTemplate<ArtistTemplate, IllustsList>
    {
        private IllustsRecommendedType IllustsRecommendedType;

        public IllustsRecommendedViewModel(IllustsRecommendedType illustsRecommendedType)
        {
            this.IllustsRecommendedType = illustsRecommendedType;
        }

        /// <summary>
        /// [API] Return Recommended Data
        /// </summary>
        /// [override] CollectionViewModelTemplate 
        protected override Task<IllustsList> GetCollectionEnumerable() {

            switch (IllustsRecommendedType)
            {
                case IllustsRecommendedType.illust:
                    return MainWindow.PixivWindow.AuthToken.GetIllustRecommended();
                case IllustsRecommendedType.manga:
                    return MainWindow.PixivWindow.AuthToken.GetMangaRecommended();
                default:
                    return MainWindow.PixivWindow.AuthToken.GetIllustRecommended();
            }
        }
    }

    /// <summary>
    /// RankingPage
    /// </summary>
    public class IllustsRankingViewModel : CollectionViewModelTemplate<ArtistTemplate, IllustsList>
    {
        private IllustsRankingType IllustsRankingType;
        private String DateString;

        public IllustsRankingViewModel(IllustsRankingType illustsRankingType, String dateString)
        {
            this.IllustsRankingType = illustsRankingType;
            this.DateString = dateString;

        }

        /// <summary>
        /// [API] Return Ranking Data
        /// </summary>
        /// [override] CollectionViewModelTemplate 
        protected override Task<IllustsList> GetCollectionEnumerable() { return MainWindow.PixivWindow.AuthToken.GetIllustRanking(dayMode: this.IllustsRankingType.ToString(),
                                                                                                                            date: this.DateString); }
    }

    /// <summary>
    /// BookMarkPage
    /// </summary>
    public class IllustsBookmarksViewModel : CollectionViewModelTemplate<ArtistTemplate, IllustsList>
    {
        private long IllustID;

        public IllustsBookmarksViewModel(long illustID)
        {
            this.IllustID = illustID;
        }

        /// <summary>
        /// [API] Return Bookmark Data
        /// </summary>
        /// [override] CollectionViewModelTemplate 
        protected override Task<IllustsList> GetCollectionEnumerable() { return MainWindow.PixivWindow.AuthToken.GetUserBookmarkIllusts(this.IllustID); }
    }

    /// <summary>
    /// SearchArtistPage
    /// </summary>
    public class IllustsSearchViewModel : CollectionViewModelTemplate<ArtistTemplate, IllustsList>
    {
        private String SearchKeyWord;
        private IllustsSearchSortType IllustsSearchSortType;

        public IllustsSearchViewModel(String searchKeyWord, IllustsSearchSortType illustsSearchSortType)
        {
            this.SearchKeyWord = searchKeyWord;
            this.IllustsSearchSortType = illustsSearchSortType;
        }

        /// <summary>
        /// [API] Return Search Data
        /// </summary>
        /// [override] CollectionViewModelTemplate 
        protected override Task<IllustsList> GetCollectionEnumerable() {

            switch (this.IllustsSearchSortType)
            {
                case IllustsSearchSortType.popular_desc:
                    return MainWindow.PixivWindow.AuthToken.SearchIllustsPopularByKeyword(keyword: this.SearchKeyWord);

                default:
                    return MainWindow.PixivWindow.AuthToken.SearchIllustsByKeyword(keyword: this.SearchKeyWord, sort: this.IllustsSearchSortType.ToString());

            }
        }
    }

    /// <summary>
    /// UserListPage
    /// </summary>
    public class UsersViewModel : CollectionViewModelTemplate<UserTemplate<UserPreviews>, UserPreviewsList>
    {
        private long UserID;

        /// <summary>
        /// [API] Return User Following Data
        /// </summary>
        public UsersViewModel(long userID)
        {
            this.UserID = userID;
        }
        // [override] CollectionViewModelTemplate 
        protected override Task<UserPreviewsList> GetCollectionEnumerable() { return MainWindow.PixivWindow.AuthToken.GetUserFollowing(this.UserID); }
    }

    /// <summary>
    /// UserDataFrame
    /// </summary>
    public class IllustsUserViewModel : CollectionViewModelTemplate<ArtistTemplate, IllustsList>
    {
        private long UserID;
        #region OnPropertyChanged Variable

        private object _UserData;
        public object UserData
        {
            get { return this._UserData; }
            set
            {
                this._UserData = value;
                OnPropertyChanged("UserData");
            }
        }

        private ObservableCollection<object> _UserProfileSource = new ObservableCollection<object>();
        public ObservableCollection<object> UserProfileSource
        {
            get { return this._UserProfileSource; }
            set
            {
                this._UserProfileSource = value;
                OnPropertyChanged("UserProfileSource");
            }
        }

        private ObservableCollection<object> _UserWorkSpaceSource = new ObservableCollection<object>();
        public ObservableCollection<object> UserWorkSpaceSource
        {
            get { return this._UserWorkSpaceSource; }
            set
            {
                this._UserWorkSpaceSource = value;
                OnPropertyChanged("UserWorkSpaceSource");
            }
        }

        #endregion
        
        public IllustsUserViewModel(long userID)
        {
            this.UserID = userID;
        }

        /// <summary>
        /// [API] Return User Data
        /// </summary>
        /// [override] CollectionViewModelTemplate 
        protected override Task<IllustsList> GetCollectionEnumerable() { return MainWindow.PixivWindow.AuthToken.GetUserIllusts(this.UserID); }


        public void GetUserDataSource(UserDetail TheTargetUser)
        {
            UserTemplate<UserDetail> TargetUserTemplate = new UserTemplate<UserDetail>(TheTargetUser);

            var UserDataSource = new
            {
                UserTemplate = TargetUserTemplate,
                IsTwitterVisible = (TargetUserTemplate.TargetUserDetail.Profile.TwitterUrl == null) ? Visibility.Collapsed : Visibility.Visible,
                IsPawooVisible = (TargetUserTemplate.TargetUserDetail.Profile.PawooUrl == null) ? Visibility.Collapsed : Visibility.Visible
            };
            this.UserData = UserDataSource;
        }

        public void GetUserProfileSource(UserDetail TheTargetUser)
        {

            this.UserProfileSource = DictionaryTable(new Dictionary<string, string> {
                    { "ウェブページ", TheTargetUser.Profile.Webpage },
                    { "性別", TheTargetUser.Profile.Gender },
                    { "生まれた年", TheTargetUser.Profile.BirthYear.ToString() },
                    { "誕生日", TheTargetUser.Profile.BirthDay },
                    { "住所", TheTargetUser.Profile.Region },
                    { "職業", TheTargetUser.Profile.Job }
            });
            this.UserWorkSpaceSource = DictionaryTable(new Dictionary<string, string> {
                    { "コンピュータ", TheTargetUser.Workspace.Pc },
                    { "モニター", TheTargetUser.Workspace.Monitor },
                    { "ソフト", TheTargetUser.Workspace.Tool },
                    { "スキャナー", TheTargetUser.Workspace.Scanner },
                    { "タブレット", TheTargetUser.Workspace.Tablet },
                    { "マウス", TheTargetUser.Workspace.Mouse },
                    { "プリンター", TheTargetUser.Workspace.Printer },
                    { "机の上にあるもの", TheTargetUser.Workspace.Desktop },
                    { "絵を描く時に聞く音楽", TheTargetUser.Workspace.Music },
                    { "机", TheTargetUser.Workspace.Desk },
                    { "椅子", TheTargetUser.Workspace.Chair },
                    { "その他", TheTargetUser.Workspace.Comment }
            });

        }

        /// <summary>
        /// Filter null and space
        /// </summary>
        private ObservableCollection<object> DictionaryTable(Dictionary<string, string> DataDic)
        {
            ObservableCollection<object> UserProfile = new ObservableCollection<object>();

            foreach (var Item in DataDic)
            {
                if (Item.Value != null && Item.Value != "")
                {
                    UserProfile.Add(
                            new
                            {
                                TargetKey = Item.Key,
                                TargetValue = Item.Value
                            }
                        );
                }

            }

            return UserProfile;
        }

    }

    /// <summary>
    /// PictureFrame
    /// </summary>
    public class IllustDetailViewModel : NotifyImplementClass
    {
        #region OnPropertyChanged Variable

        private ArtistTemplate _TargetArtistsTemplate;
        public ArtistTemplate TargetArtistsTemplate
        {
            get { return this._TargetArtistsTemplate; }
            set
            {
                this._TargetArtistsTemplate = value;
                OnPropertyChanged("TargetArtistsTemplate");
            }
        }

        #endregion

        public async Task GetTheIllust(Illust illust) {

            // [API] Return IllustDetail Data
            IllustsList RefreshIllusts = await MainWindow.PixivWindow.AuthToken.GetIllustDetail(illust.Id);
            // Loading Images
            this.TargetArtistsTemplate = new ArtistTemplate(RefreshIllusts.Illust, isLarge: true, InitGetImage : true);
           
        }

    }

}
