using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TimberValueEvaluationSystem.ViewModels;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using HandyControl.Controls;
using TimberValueEvaluationSystem.Views;
using System.Windows.Input;
using System.Runtime.Serialization.Json;

namespace TimberValueEvaluationSystem
{
    public partial class MainView : System.Windows.Window
    {
        public MainView()
        {
            InitializeComponent();
            //绑定ViewModel
            this.DataContext = new MainViewModel(Nav);
            Nav.Navigate(HomePageView.GetPage());
        }

        //窗体加载完成后的按钮动画
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //创建动画过程
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

                //延迟动画时间
                marginAnim.Duration = TimeSpan.FromSeconds(0.5 + i*0.25);

                //创建动画版播放动画
                var sb = new Storyboard();
                sb.Children.Add(marginAnim);
                sb.Begin(this);
                
            }

            // 创建 NotifyIcon 对象
            System.Windows.Forms.NotifyIcon notifyIcon = new()
            {
                // 设置托盘图标
                Icon = new Icon(@"Resources/Icon/logo_icon.ico"),

                // 设置鼠标移动时的文本
                Text = "林木价值评价系统",

                // 显示托盘图标
                Visible = true
            };

            // 创建菜单项
            ToolStripMenuItem restoreItem = new("显示");
            restoreItem.Click += RestoreItem_Click;

            ToolStripMenuItem exitItem = new("退出");
            exitItem.Click += ExitItem_Click;

            // 创建右键菜单
            ContextMenuStrip menu = new();
            menu.Items.Add(restoreItem);
            menu.Items.Add(exitItem);

            // 将右键菜单分配给 NotifyIcon 对象
            notifyIcon.ContextMenuStrip = menu;
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(NotifyIcon_MouseClick);

            // 加载自定义的鼠标样式
            System.Windows.Input.Cursor myCursor = new System.Windows.Input.Cursor(@"Resources/Cursors/pointer.cur");
            rootborder.Cursor = myCursor;
        }



        private void RestoreItem_Click(object? sender, EventArgs? e)
        {
            // 显示恢复窗口
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
        }
        private void ExitItem_Click(object? sender, EventArgs? e)
        {
            // 关闭应用程序
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 将窗口隐藏并最小化到托盘
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            this.WindowState = WindowState.Minimized;
        }

        private void NotifyIcon_MouseClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            // 在单击托盘图标时显示窗口并将其前置
            if (e.Button == MouseButtons.Left)
            {
                // 显示窗口并将其置于屏幕的最顶层
                this.Show();
                this.WindowState = WindowState.Normal;
                this.Topmost = true;
                this.Activate();

                // 将置顶属性重置为 false，在窗口获得焦点时再次激活
                Dispatcher.BeginInvoke(new Action(() => { this.Topmost = false; }));
            }
        }
    }
}
