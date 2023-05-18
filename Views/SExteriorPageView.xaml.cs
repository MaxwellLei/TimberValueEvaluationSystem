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
using TimberValueEvaluationSystem.ViewModels;

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
            this.DataContext = new SExteriorPageViewModel();
            (this.DataContext as SExteriorPageViewModel).CoverFlowMain = this.CoverFlow;
            (this.DataContext as SExteriorPageViewModel).Init();
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
