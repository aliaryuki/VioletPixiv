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
using VioletPixiv.UIObject.UserControls;
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class RankingFame : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region OnPropertyChanged Variable

        private DateTime _TargetDate;
        public DateTime TargetDate
        {
            get
            {
                return _TargetDate;
            }

            set
            {
                _TargetDate = value;

                var tmpDays = new ObservableCollection<int>();
                for (int i = 1; i < DateTime.DaysInMonth(TargetDate.Year, TargetDate.Month) + 1; i++)
                {
                    tmpDays.Add(i);
                }
                Days = tmpDays;


            }
        }

        private ObservableCollection<int> _Days = new ObservableCollection<int>();
        public ObservableCollection<int> Days
        {
            get { return _Days; }
            set
            {
                _Days = value;
                OnPropertyChanged("Days");
            }
        }

        private ObservableCollection<int> _Years = new ObservableCollection<int>();
        public ObservableCollection<int> Years
        {
            get { return _Years; }
            set
            {
                _Years = value;
            }
        }

        #endregion

        public IllustsRankingType RankingType = IllustsRankingType.day;

        public RankingFame()
        {
            InitializeComponent();

            // Get Now Time
            this.TargetDate = DateTime.Today.ToLocalTime();

            // List 30 Years from now
            for (int i = this.TargetDate.Year; i > this.TargetDate.Year - 30; i--)
            {
                this.Years.Add(i);
            }
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
            this.NavigateNewRankingPage();

        }

        /// <summary>
        /// Get Date and Navigate
        /// </summary>
        private void DateSearchButton_Click(object sender, RoutedEventArgs e)
        {
            var TargetButton = sender as Button;
            var DateSearchStackPanel = UIAccess.FindParentByName<StackPanel>(TargetButton, "DateSearchStackPanel");
            var TargetDay = (DateSearchStackPanel.FindName("DayCombobox") as DarkCombobox).SelectedValue.ToString();

            if (TargetDay == null) return;

            // Go Navigate
            this.NavigateNewRankingPage();
        }

        /// <summary>
        /// When Change Combobox Date, update to TargetDate
        /// </summary>
        private void ChangeDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var TargetComboBox = (ComboBox)sender;
            if (TargetComboBox.Items.Count == 0 || TargetComboBox.SelectedValue == null) return;

            switch (TargetComboBox.Name)
            {
                case "YearCombobox":
                    this.ChangeTime(this.TargetDate, year: (int)TargetComboBox.SelectedValue);
                    break;

                case "MonthCombobox":
                    this.ChangeTime(this.TargetDate, month: (int)TargetComboBox.SelectedValue);
                    break;

                case "DayCombobox":
                    this.ChangeTime(this.TargetDate, day: (int)TargetComboBox.SelectedValue);
                    break;
            }

        }

        /// <summary>
        /// ChangeTime and Call setter
        /// </summary>
        private void ChangeTime(DateTime dateTime, int? year = null, int? month = null, int? day = null)
        {
            // Day can not Exceed that month should be.
            day = day ?? Math.Min(dateTime.Day, DateTime.DaysInMonth(year ?? dateTime.Year, month ?? dateTime.Month));

            // Set TargetDate Value
            this.TargetDate = new DateTime(
                year ?? dateTime.Year,
                month ?? dateTime.Month,
                day ?? dateTime.Day);
        }

        // Navigate
        private void NavigateNewRankingPage()
        {

            if (this.TargetDate.Date == DateTime.Now.Date)
            {
                this.Navigate(new RankingPage(RankingType: this.RankingType));
            }
            else
                this.Navigate(new RankingPage(RankingType: this.RankingType, RankingDate: this.TargetDate.ToString("yyyy-MM-dd")));

        }
    }
}
