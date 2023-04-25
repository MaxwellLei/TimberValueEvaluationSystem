using HandyControl.Controls;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimberValueEvaluationSystem.ViewModels;

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// ModelPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ModelPageView : Page
    {
        private static ModelPageView mapPage = null;
        private ModelPageView()
        {
            InitializeComponent();
            this.DataContext = new ModelPageViewModel(ModelNav);
        }
        public static Page GetPage()
        {
            if (mapPage == null)
            {
                mapPage = new ModelPageView();
            }
            return mapPage;
        }
        //动画重新播放
        public static void RefeshAn()
        {
            //创建动画过程
            var marginAnim = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, -300, 0),
                To = new Thickness(0, 0, 0, 0),
                EasingFunction = new QuadraticEase()
            };
            for (int i = 1; i < 3; i++)
            {
                Storyboard.SetTargetName(marginAnim, "RadioButton" + i);
                Storyboard.SetTargetProperty(marginAnim, new PropertyPath(MarginProperty));

                //延迟动画时间
                marginAnim.Duration = TimeSpan.FromSeconds(0.5 + i * 0.05); 

                //创建动画版播放动画
                var sb = new Storyboard();
                sb.Children.Add(marginAnim);
                sb.Begin(mapPage);
            }
        }
    }
}
