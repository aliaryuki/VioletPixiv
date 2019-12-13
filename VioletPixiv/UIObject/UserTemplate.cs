using System;
using System.Collections.Generic;
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
    public class UserTemplate<T> : NeedToLoadImages, INotifyPropertyChanged
        where T : HasUser
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

        protected BitmapImage _IllustHeader;
        public BitmapImage IllustHeader
        {

            get { return this._IllustHeader; }
            private set
            {
                this._IllustHeader = value;
                OnPropertyChanged("IllustHeader");
            }

        }

        #endregion

        public T TargetUserDetail { get; private set; }
        
        // Constructor
        public UserTemplate(T targetUserDetail, bool isLarge = false, bool InitGetImage = false)
        {
            this.TargetUserDetail = targetUserDetail;
            Task GetAllImageTask = this.GetAllImage();
        }

        // [override] NeedToLoadImages
        public override async Task GetAllImage()
        {
            await Task.Run(async () => {
                await this.GetImageHeader();
            });
        }


        // Load IllustHeader
        protected async Task GetImageHeader()
        {
            this.IllustHeader = await GetImages.GetImage(this.TargetUserDetail.User.ProfileImageUrls.Medium);
        }


    }

}
