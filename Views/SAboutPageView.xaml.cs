using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.Views
{
    /// <summary>
    /// SAboutPageView.xaml 的交互逻辑
    /// </summary>
    public partial class SAboutPageView : Page
    {
        private static SAboutPageView aboutPage = null;
        public SAboutPageView()
        {
            InitializeComponent();
        }
        public static Page GetPage()
        {
            if (aboutPage == null)
            {
                aboutPage = new SAboutPageView();
            }
            return aboutPage;
        }

        //播放Seraphine语音
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer player;
            if (Services.ConfigHelper.GetConfig("language") == "0")
            {
                player = new SoundPlayer(@"Resources/Audio/Seraphine_Quote_zh-CN.wav");
            }
            else
            {
                player = new SoundPlayer(@"Resources/Audio/Seraphine_Quote_en-US.wav");
            }
            player.Play();
        }
    }
}
