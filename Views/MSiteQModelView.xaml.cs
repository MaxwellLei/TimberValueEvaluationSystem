using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimberValueEvaluationSystem.ViewModels;

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// MSiteQModelView.xaml 的交互逻辑
    /// </summary>
    public partial class MSiteQModelView : Page
    {
        private static MSiteQModelView mSiteQModelPage = null;
        private MSiteQModelView()
        {
            InitializeComponent();
            this.DataContext = new MSiteQModelViewModel(singleItem, importCSV, databaseImport);
        }
        public static Page GetPage()
        {
            if (mSiteQModelPage == null)
            {
                mSiteQModelPage = new MSiteQModelView();
            }
            return mSiteQModelPage;
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new ImageBrowser(new Uri("pack://application:,,,/Resources/PythonModel/SiteQuality_DecisionTree/model_graph.jpg")).Show();
        }

        private void Image_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            new ImageBrowser(new Uri("pack://application:,,,/Resources/PythonModel/SiteQuality_DecisionTree/Classificationmodel.png")).Show();
        }
    }
}
