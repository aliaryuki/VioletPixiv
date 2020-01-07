using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MetroRadiance.UI;
using Pixeez;
using Pixeez.Objects;
using System.Windows.Threading;
using System.Threading;
using System.Collections;
using VioletPixiv.ViewModel;
using VioletPixiv.Animation;
using System.Windows.Input;

namespace VioletPixiv
{

    public partial class MainWindow
    {

        #region variable

        // Global MainWindow
        public static MainWindow PixivWindow;

        // AuthToken
        public Tokens AuthToken = null;

        // Container
        private Grid _LeftTabGrid;
        public Grid LeftTabGrid {
            get
            {
                if (_LeftTabGrid == null)
                {
                    _LeftTabGrid = (Grid)LeftTab.Template.FindName("LeftTabGrid", LeftTab);
                }
                return _LeftTabGrid;
            }

        }
        private Grid _InnerGrid;
        public Grid InnerGrid {
            get
            {
                if(_InnerGrid == null)
                {
                    _InnerGrid = (Grid)LeftTab.Template.FindName("InnerGrid", LeftTab);
                }
                return _InnerGrid;
            }

        }

        // UserSettingFrame
        private SubPageFrame UserSettingFrame = null;

        // LeftTab Collapse(True) and Expand(False)  
        private bool IsLeftBarCollapse = false;

        #endregion

        /// <summary>
        /// Initialize
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            PixivWindow = this;

            // Set ViewModel
            this.DataContext = new MainViewModel();

            // Start Login
            Task Login = LoginProcess();
        }

        public async Task LoginProcess()
        {

            // If Default account or passwd is null
            if (String.IsNullOrEmpty(Properties.Settings.Default.passwd) || String.IsNullOrEmpty(Properties.Settings.Default.account)) { }
            else
            {
                try
                {
                    // If Default account and passwd are valid
                    // Login and Init
                    AuthToken = await Pixeez.Auth.AuthorizeAsync(Properties.Settings.Default.account, Properties.Settings.Default.passwd);
                    Init();
                    return;
                }
                #pragma warning disable 168
                catch (InvalidOperationException e){}
                #pragma warning disable 168
            }

            // Open LoginPage
            new SubPageFrame(MainGridR1, new LoginPage());

        }

        /// <summary>
        /// Init After Login
        /// </summary>
        public void Init()
        {

            // Get my account data
            UpdateUserDetail();

            // Show F0
            LeftTab.SelectedIndex = 0;
            IllustsPage(F0, 0);

            // F3 is Searching Page, which has searching history.
            while (F3.CanGoBack) F3.RemoveBackEntry();
        }

        private async void UpdateUserDetail()
        {
            // Get UserDetail
            var TheTargetUser = await AuthToken.GetUserDetail(AuthToken.Auth.User.Id);
            // Update UserData(Image) To View
            (this.DataContext as MainViewModel).GetUserDataSource(TheTargetUser);
        }

        #region button click event

        /// <summary>
        /// TabControl Animation Controller
        /// </summary>
        public void TabControlToggle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton).IsChecked ?? false || (sender as ToggleButton).IsChecked == false)
            {
                LeftTabToggleAnimateToggle(true);  // Expand
                UserSettingFrame = new SubPageFrame(InnerGrid, new UserConfig());
            }
            else
            {
                LeftTabToggleAnimateToggle(false); // Collapse
                UserSettingFrame?.FrameOut();
            }
        }

        private void LeftTabToggleAnimateToggle(bool ToggleStatus)
        {
            IsLeftBarCollapse = LeftTabToggleAnimation.Toggle(LeftTabGrid,
                                                               LeftTabGrid.ActualWidth * 0.75 + (MainGrid.ActualWidth - LeftTab.ActualWidth),
                                                               (MainGrid.ActualWidth - LeftTab.ActualWidth),
                                                               ToggleStatus);
        }

        /// <summary>
        /// Refresh now selected Frame
        /// </summary>
        private void FrameRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }

        private void Refresh()
        {
            var SelectedItemFrame = (LeftTab.SelectedItem as TabItem).Content as System.Windows.Controls.Frame;

            (SelectedItemFrame.Content as ICollectionPageTemplate).InitLoading();
        }

        /// <summary>
        /// Init and Logout 
        /// </summary>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {

            foreach (TabItem element in UIAccess.FindChildrenByType<TabItem>(LeftTab))
            {
                var TargetFrame = (element.Content as System.Windows.Controls.Frame);

                Task.Run(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TargetFrame.ClearAll();
                    }));
                });
            }

            AuthToken = null;
            Properties.Settings.Default.account = null;
            Properties.Settings.Default.passwd = null;
            Properties.Settings.Default.Save();

            Task Login = LoginProcess();
        }

        #endregion

        /// <summary>
        /// TabItem Selection Changed
        /// </summary>
        private void SelectionChanged(object sender, EventArgs e)
        {
            var SelectedIndex = (sender as TabControl).SelectedIndex;
            var SelectedItem = (sender as TabControl).SelectedItem as TabItem;
            var SelectedItemFrame = SelectedItem.Content as System.Windows.Controls.Frame;
            
            if (SelectedItemFrame == null || AuthToken == null) return;

            if (SelectedItem.IsSelected && SelectedItemFrame.Content == null)
            {
                IllustsPage(SelectedItemFrame, SelectedIndex);
            }
        }

        /// <summary>
        /// Frame New Pages
        /// </summary>
        private void IllustsPage(System.Windows.Controls.Frame TargetFrame, int index)
        {
            switch (index)
            {
                case 0:
                    TargetFrame.Navigate(new RecommendPage());
                    break;
                case 1:
                    TargetFrame.Navigate(new RankingPage());
                    break;
                case 2:
                    TargetFrame.Navigate(new BookMarkPage(AuthToken.Auth.User.Id));
                    break;
                case 3: // Do not change this index     ε٩(๑> ₃ <)۶з	
                    break;
                case 4:
                    TargetFrame.Navigate(new UserListPage(AuthToken.Auth.User.Id));
                    break;
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                this.Refresh();
            }
        }

    }
}
