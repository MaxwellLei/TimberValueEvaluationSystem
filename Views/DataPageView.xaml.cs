using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.ViewModels;

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// DataPageView.xaml 的交互逻辑
    /// </summary>
    public partial class DataPageView : Page
    {
        private static DataPageView databasePage = null;
        public DataPageView()
        {
            InitializeComponent();
            this.DataContext = new DataPageViewModel();
        }
        public static Page GetPage()
        {
            if (databasePage == null)
            {
                databasePage = new DataPageView();
            }
            return databasePage;
        }


    }
}
