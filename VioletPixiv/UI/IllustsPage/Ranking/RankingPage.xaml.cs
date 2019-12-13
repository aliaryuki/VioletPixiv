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
using static VioletPixiv.UIObject.EnumType;

namespace VioletPixiv
{

    public partial class RankingPage : IllustsPageTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RankingType"> IllustsRankingType </param>
        /// <param name="RankingDate"> Date String in yyyy-MM-dd </param>
        public RankingPage( IllustsRankingType RankingType = IllustsRankingType.day, String RankingDate = null )
        {
            InitializeComponent();

            // Set ViewModel
            this.DataContext = new IllustsRankingViewModel( RankingType, RankingDate );

            // init
            this.InitLoading();
            
        }

    }
}
