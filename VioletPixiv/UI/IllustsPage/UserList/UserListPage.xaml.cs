using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

    public partial class UserListPage : UsersPageTemplate
    {
        public UserListPage(long userID)
        {
            InitializeComponent();
            this.PerLoadingImages = 70;
            this.DataContext = new UsersViewModel(userID);
            this.Title = "UserID:" + userID;

            this.InitLoading();

        }

    }
}
