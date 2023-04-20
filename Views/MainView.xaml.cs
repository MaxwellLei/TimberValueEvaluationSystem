using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TimberValueEvaluationSystem
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        //窗体加载完成后的按钮动画
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var marginAnim = new ThicknessAnimation()
            {
                From = new Thickness(-200, 0, 0, 0),
                To = new Thickness(0, 0, 0, 0),
                EasingFunction = new QuadraticEase()
            };
            for (int i = 1; i < 6; i++)
            {
                Storyboard.SetTargetName(marginAnim, "RadioButton" + i);
                Storyboard.SetTargetProperty(marginAnim, new PropertyPath(MarginProperty));

                marginAnim.Duration = TimeSpan.FromSeconds(0.5 + i*0.2);

                var sb = new Storyboard();
                sb.Children.Add(marginAnim);
                sb.Begin(this);
            }
        }
    }
}
