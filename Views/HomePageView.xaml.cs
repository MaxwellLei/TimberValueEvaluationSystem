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
    //主页
    public partial class HomePageView : Page
    {
        private static HomePageView homePage = null;
        private HomePageView()
        {
            InitializeComponent();
            this.DataContext = new HomePageViewModel();
        }
        public static Page GetPage()
        {
            if (homePage == null)
            {
                homePage = new HomePageView();
            }
            return homePage;
        }
    }
}
