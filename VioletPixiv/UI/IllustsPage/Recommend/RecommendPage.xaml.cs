using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public partial class RecommendPage : IllustsPageTemplate
    {
        
        public RecommendPage(IllustsRecommendedType ArtistsType = IllustsRecommendedType.illust)
        {
            InitializeComponent();

            // Set ViewModel
            this.DataContext = new IllustsRecommendedViewModel(ArtistsType);

            // init
            this.InitLoading();
            
        }

    }
}
