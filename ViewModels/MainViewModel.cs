using GalaSoft.MvvmLight.CommandWpf;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Themes;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.Views;


namespace TimberValueEvaluationSystem.ViewModels
{
    //主窗口的ViewModel
    class MainViewModel
    {
        private Frame Nav;  //导航对象
        public RelayCommand MapPage { get; private set; }   //切换地图命令
        public RelayCommand HomePage { get; private set; }  //切换主页命令
        public RelayCommand ModelPage { get; private set; }  //切换模型命令
        public RelayCommand DataPage { get; private set; }  //切换数据命令
        public RelayCommand SettingPage { get; private set; }  //切换设置命令
        public RelayCommand ThemeModeChange { get; private set; }  //切换设置命令

        //初始化
        public MainViewModel(Frame nav)
        {
            Nav = nav;
            MapPage = new RelayCommand(ExecuteMapPage);
            HomePage = new RelayCommand(ExecuteHomePage);
            ModelPage = new RelayCommand(ExecuteModelPage);
            DataPage = new RelayCommand(ExecuteDataPage);
            SettingPage = new RelayCommand(ExecuteSettingPage);
            //ThemeModeChange = new RelayCommand(ExecuteThemeModeChange);

            FunInit();
        }


        //切换地图命令
        private void ExecuteMapPage()
        {
            Nav.Navigate(MapPageView.GetPage());
        }

        //切换主页命令
        private void ExecuteHomePage()
        {
            Nav.Navigate(HomePageView.GetPage());
        }

        //切换模型命令
        private void ExecuteModelPage()
        {
            Nav.Navigate(ModelPageView.GetPage());
            ModelPageView.RefeshAn();
        }

        //切换地图命令
        private void ExecuteDataPage()
        {
            Nav.Navigate(DataPageView.GetPage());
        }

        //切换设置命令
        private void ExecuteSettingPage()
        {
            Nav.Navigate(SettingPageView.GetPage());
            SettingPageView.RefeshAn();
        }

        //功能初始化
        private void FunInit()
        {
            //初始化自动关闭时间
            MessageHelper.waitTime = Convert.ToInt32(
                ConfigHelper.GetConfig("auto_off_time"));
            //自动更新
            if(ConfigHelper.GetConfig("auto_check_update") == "True")
            {
                UpdateHelper.CheckForUpdatesAsync();
            }
        }
    }
}
