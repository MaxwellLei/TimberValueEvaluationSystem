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
using TimberValueEvaluationSystem.ViewModels;

namespace TimberValueEvaluationSystem.Views
{
    //地图页
    public partial class MapPageView : Page
    {
        private static MapPageView mapPage = null;
        private MapPageView()
        {
            InitializeComponent();
            this.DataContext = new MapPageViewModel();
        }
        public static Page GetPage()
        {
            if (mapPage == null)
            {
                mapPage = new MapPageView();
            }
            return mapPage;
        }
    }
}
