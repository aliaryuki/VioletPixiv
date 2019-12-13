using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VioletPixiv.ViewModel;

namespace VioletPixiv
{
    public partial class UserDataFrame : IllustsPageTemplate
    {

        public UserDataFrame(long userID)
        {

            InitializeComponent();

            // Set ViewModel
            this.DataContext = new IllustsUserViewModel(userID);
            this.Title = "ID：" + userID;

            // Update Artists
            this.InitLoading();

            // Update UserData
            this.UpdateUserData(userID);

        }

        private async void UpdateUserData(long userID)
        {
            // Get UserData
            UserDetail TargetUser = await MainWindow.PixivWindow.AuthToken.GetUserDetail(userID);

            // Update UserDataSource and UserDataProfile
            (CollectionViewModel as IllustsUserViewModel).GetUserDataSource(TargetUser);
            (CollectionViewModel as IllustsUserViewModel).GetUserProfileSource(TargetUser);
            

        }

    }
}
