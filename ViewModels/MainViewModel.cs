using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;
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
        public RelayCommand SettingPage { get; private set; }  //切换模型命令

        //初始化
        public MainViewModel(Frame nav)
        {
            Nav = nav;
            MapPage = new RelayCommand(ExecuteMapPage);
            HomePage = new RelayCommand(ExecuteHomePage);
            ModelPage = new RelayCommand(ExecuteModelPage);
            SettingPage = new RelayCommand(ExecuteSettingPage);
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

        //切换主页命令
        private void ExecuteModelPage()
        {
            Nav.Navigate(ModelPageView.GetPage());
            ModelPageView.RefeshAn();
        }

        //切换设置命令
        private void ExecuteSettingPage()
        {
            Nav.Navigate(SettingPageView.GetPage());
            SettingPageView.RefeshAn();
        }
    }
}
