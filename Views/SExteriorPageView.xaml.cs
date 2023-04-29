using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// SExteriorPageView.xaml 的交互逻辑
    /// </summary>
    public partial class SExteriorPageView : Page
    {
        private static SExteriorPageView ExteriorPage = null;
        public SExteriorPageView()
        {
            InitializeComponent();
            CoverFlowMain.AddRange(new[]
            {
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/1.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/2.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/3.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/4.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/5.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/6.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/7.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/8.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/9.jpg"),
                new Uri(@"pack://application:,,,/Resources/Image/StartPic/10.jpg")
            });
        }
        public static Page GetPage()
        {
            if (ExteriorPage == null)
            {
                ExteriorPage = new SExteriorPageView();
            }
            return ExteriorPage;
        }
    }
}
