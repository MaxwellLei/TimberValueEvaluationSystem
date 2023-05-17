using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.Views;

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// StartView.xaml 的交互逻辑
    /// </summary>
    public partial class StartView : Window
    {
        private bool isDragging = false;                      // 是否正在拖动窗体
        private Point startPoint;                             // 当前鼠标按下的位置

        public StartView()
        {
            InitializeComponent();
            LanguageHelper.InitializeLanguage();
            // 异步执行启动过程
            Task.Run(() =>
            {
                // 模拟启动过程，更新进度条的进度
                for (int i = 0; i <= 100; i++)
                {
                    // 更新进度条的进度
                    this.Dispatcher.Invoke(() =>
                    {
                        progressBar.Value = i;
                    });

                    // 模拟一段耗时的操作
                    Thread.Sleep(20);
                }

                // 启动过程完成，关闭窗口并打开主界面
                this.Dispatcher.Invoke(() =>
                {
                    Window mainWindow = new MainView();
                    mainWindow.Show();
                    this.Close();
                });
            });

            // 为进度条添加动画效果
            DoubleAnimation animation = new()
            {
                From = 0,
                To = 100,
                Duration = new Duration(TimeSpan.FromSeconds(5))
            };
            progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);

            //启动窗体的背景图片
            int pic = new Random().Next(1, 11); 
            Uri uri = new(@"Resources/Image/StartPic/"+pic+".jpg", UriKind.Relative);
            ImageBrush imageBrush = new()
            {
                ImageSource = new BitmapImage(uri)
            };
            Basemap.Background = imageBrush;
        }

        //窗体启动动画
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            };
            this.BeginAnimation(UIElement.OpacityProperty, animation);
        }
        //鼠标按下
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // 获取当前鼠标按下的位置
                startPoint = e.GetPosition(this);

                // 设置拖动标记为 true
                isDragging = true;
            }
        }
        //鼠标移动
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                // 获取窗体当前的位置
                double left = this.Left;
                double top = this.Top;

                // 获取鼠标在窗体内的移动量，并计算出窗体应该移动到的位置
                Point currentPoint = e.GetPosition(this);
                double newLeft = left + currentPoint.X - startPoint.X;
                double newTop = top + currentPoint.Y - startPoint.Y;

                // 设置窗体的位置
                this.Left = newLeft;
                this.Top = newTop;
            }
        }
        //松开鼠标
        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // 重置拖动标记为 false
                isDragging = false;
            }
        }
    }
}
