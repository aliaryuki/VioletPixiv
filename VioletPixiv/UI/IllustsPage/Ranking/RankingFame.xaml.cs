using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using VioletPixiv.UIObject;
using VioletPixiv.UIObject.UserControls;
using VioletPixiv.ViewModel;
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class RankingFame
    {
        private RankingFrameViewModel _TargetViewModel;
        public RankingFrameViewModel TargetViewModel
        {
            get
            {
                if (this._TargetViewModel == null)
                {
                    this._TargetViewModel = this.DataContext as RankingFrameViewModel;
                }
                return this._TargetViewModel;
            }
        }

        public IllustsRankingType RankingType = IllustsRankingType.day;
        public String DateStringFormat;

        public RankingFame()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Change RankingType and Navigate
        /// </summary>
        private void TypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var TargetComboBox = sender as ComboBox;

            switch ((TargetComboBox.SelectedItem as ComboBoxItem).Content as String)
            {
                case "デイリー":
                    RankingType = IllustsRankingType.day;
                    break;

                case "ウィークリー":
                    RankingType = IllustsRankingType.week;
                    break;

                case "マンスリー":
                    RankingType = IllustsRankingType.month;
                    break;

                case "ルーキー":
                    RankingType = IllustsRankingType.week_rookie;
                    break;

                case "オリジナル":
                    RankingType = IllustsRankingType.week_original;
                    break;

                case "男子に人気":
                    RankingType = IllustsRankingType.day_male;
                    break;

                case "女子に人気":
                    RankingType = IllustsRankingType.day_female;
                    break;
            }

            // Go Navigate
            this.NavigateNewRankingPage(GetSelectedDate());

        }

        /// <summary>
        /// Get Date and Navigate
        /// </summary>
        private void DateSearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Go Navigate
            this.NavigateNewRankingPage(GetSelectedDate());
        }

        private String GetSelectedDate()
        {
            var TargetTemplate = this.Template;
            var ControlGrid =  TargetTemplate.FindName("MainToggleBar", this) as TopToggleBar;
            var TargetVM = ControlGrid.DataContext as RankingFrameViewModel;

            return TargetVM.SelectedDateStringFormat;
        }

        private String ChangeToDateStringFormat(String Year, String Month, String Day)
        {
            return Year + "-" + Month + "-" + Day;
        }

        // Navigate
        private void NavigateNewRankingPage(String DateFormatString)
        {
            if (DateFormatString == null) return;

            if (DateFormatString == DateTime.Now.Date.ToString("yyyy-MM-dd"))
            {
                this.Navigate(new RankingPage(RankingType: this.RankingType));
            }
            else
                this.Navigate(new RankingPage(RankingType: this.RankingType, RankingDate: DateFormatString));

        }

    }
}
