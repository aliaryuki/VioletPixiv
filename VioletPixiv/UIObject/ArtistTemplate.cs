using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Pixeez.Objects;

namespace VioletPixiv
{
    public abstract class NeedToLoadImages
    {
        #pragma warning disable CS1998
        public virtual async Task GetAllImage()
        {
            throw new NotImplementedException();
        }
        #pragma warning restore CS1998
    }

    public class ArtistTemplate : NeedToLoadImages, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region OnPropertyChanged Variable

        // IllustHeader
        private BitmapImage _IllustHeader = null;
        public BitmapImage IllustHeader
        {

            get { return this._IllustHeader; }
            private set
            {
                this._IllustHeader = value;
                OnPropertyChanged("IllustHeader");
            }

        }

        // One IllustImage
        private BitmapImage _IllustImage = null;
        public BitmapImage IllustImage
        {

            get { return this._IllustImage; }
            private set
            {
                this._IllustImage = value;
                OnPropertyChanged("IllustImage");
            }

        }

        // Multiple IllustImages
        private ObservableCollection<BitmapImage> _IllustImageCollection = new ObservableCollection<BitmapImage>();
        public ObservableCollection<BitmapImage> IllustImageCollection
        {

            get { return this._IllustImageCollection; }
            private set
            {
                this._IllustImageCollection = value;
                OnPropertyChanged("IllustImageCollection");
            }

        }

        #endregion

        public Illust TargetIllust { get; private set; }
        private bool IsLarge = false;

        // Costructor
        public ArtistTemplate(Illust illust, bool isLarge = false, bool InitGetImage = false)
        {
            this.IsLarge = isLarge;
            this.TargetIllust = illust;
            if (InitGetImage) { Task GetAllImageTask = this.GetAllImage(); }
        }

        
        // [override] NeedToLoadImages
        public override async Task GetAllImage()
        {
            await Task.Run(async () => {
                
                await this.GetImageHeader();
                await this.GetImageIllust();
            });
        }

        // Load IllustHeader
        private async Task GetImageHeader()
        {
            this.IllustHeader = await GetImages.GetImage(TargetIllust.User.ProfileImageUrls.Medium);
        }
        // Load IllustImage
        public async Task GetImageIllust()
        {
            switch (this.IsLarge)
            {
                case false:
                    this.IllustImage = await GetImages.GetImage(TargetIllust.ImageUrls.SquareMedium);
                    break;

                case true:

                    if (this.TargetIllust.MetaSinglePage.OriginalImageUrl != null)
                    {
                        var TagetImage = await GetImages.GetImage(this.TargetIllust.MetaSinglePage.OriginalImageUrl);

                        // [No Wait] UI Update
                        _ = Application.Current.Dispatcher.BeginInvoke(new Action(() => {

                            this.IllustImageCollection.Add(TagetImage);

                        }));

                        break;
                    }
                    else
                    {
                        var IllustNum = this.TargetIllust.MetaPages.Count();

                        // [No Wait] UI Update
                        _ = Application.Current.Dispatcher.BeginInvoke(new Action(async() => {
                            foreach (var i in this.TargetIllust.MetaPages)
                            {
                                var TagetImage = await GetImages.GetImage(i.ImageUrls.Original);
                                this.IllustImageCollection.Add(TagetImage);
                            }
                        }));
                        break;
                    }

            }
        }

    }

}
