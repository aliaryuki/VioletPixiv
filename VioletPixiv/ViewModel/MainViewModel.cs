using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VioletPixiv
{
    /// <summary>
    /// MainWindow
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
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

        private UserTemplate<UserDetail> _UserData;
        public UserTemplate<UserDetail> UserData
        {
            get { return _UserData; }
            set
            {
                _UserData = value;
                OnPropertyChanged("UserData");
            }
        }

        #endregion

        public void GetUserDataSource(UserDetail TheTargetUser)
        {
            UserData = new UserTemplate<UserDetail>(TheTargetUser);

        }

    }

}
