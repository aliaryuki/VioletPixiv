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

namespace VioletPixiv.ViewModel
{
    public class RankingFrameViewModel : NotifyImplementClass
    {

        #region OnPropertyChanged Variable

        private DateTime _TodayDate;
        public DateTime TodayDate
        {
            get
            {
                return _TodayDate;
            }
            set
            {
                _TodayDate = value;

                // List 30 Years from now
                var YearsTmp = new ObservableCollection<int>();
                for (int i = value.Year; i > value.Year - 30; i--)
                {
                    YearsTmp.Add(i);
                }
                this.Years = YearsTmp;

                this.SelectedMonth = value.Month;
                this.SelectedYear = value.Year;
                this.SelectedDay = value.Day;

            }
        }


        private ObservableCollection<int> _Days = new ObservableCollection<int>();

        public ObservableCollection<int> Days
        {
            get { return _Days; }
            set
            {
                _Days = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<int> _Years = new ObservableCollection<int>();

        public ObservableCollection<int> Years
        {
            get { return _Years; }
            set
            {
                _Years = value;
                RaisePropertyChanged();
            }
        }


        public String SelectedDateStringFormat
        {
            get
            {
                return  (SelectedDay.ToString() == null) ? null : 
                                                           SelectedYear.ToString() + "-" + SelectedMonth.ToString() + "-" + SelectedDay.ToString();
            }
        }


        private int _SelectedYear;

        public int SelectedYear
        {
            get
            {
                return _SelectedYear;
            }
            set
            {
                _SelectedYear = value;
                this.UpdateDaysInMonth();
                RaisePropertyChanged();
            }
        }


        private int _SelectedMonth;

        public int SelectedMonth
        {
            get
            {
                return _SelectedMonth;
            }
            set
            {
                _SelectedMonth = value;
                this.UpdateDaysInMonth();
                RaisePropertyChanged();
            }
        }


        private int _SelectedDay;

        public int SelectedDay
        {
            get
            {
                return _SelectedDay;
            }
            set
            {
                _SelectedDay = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private void UpdateDaysInMonth()
        {
            try{
                var tmpDays = new ObservableCollection<int>();
                for (int i = 1; i < DateTime.DaysInMonth(this._SelectedYear, this._SelectedMonth) + 1; i++)
                {
                    tmpDays.Add(i);
                }
                Days = tmpDays;
            }
            catch {}
        }

        public RankingFrameViewModel()
        {
            this.TodayDate = DateTime.Today.ToLocalTime();
        }

    }

}
