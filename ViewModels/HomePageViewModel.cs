using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HarfBuzzSharp;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {

        
        private DispatcherTimer _timer;     //计时器对象

        //搜索内容
        private SearchBar _searchString;
        public SearchBar SearchString
        {
            get { return _searchString; }
            set { Set(ref _searchString, value); }
        }
        //当前时间
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set { Set(ref _currentTime, value); }
        }
        //当前日期
        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set { Set(ref _currentDate, value); }
        }



        public RelayCommand SearchStartedCommand { get; private set; }   //搜索开始命令

        public HomePageViewModel()
        {
            SearchStartedCommand = new RelayCommand(ExecuteSearchStartedCommand);

            Init();
        }

        //执行搜索命令
        private void ExecuteSearchStartedCommand()
        {
            if(SearchString.Text != "")
            {
                //获取搜索内容
                string temp = ConfigHelper.GetConfig("home_search_engine");
                string url;
                if (temp == "BaiDu")
                {
                    url = "https://www.baidu.com/s?wd=" + SearchString.Text;
                }
                else if (temp == "Bing")
                {
                    url = "https://www.bing.com/search?q=" + SearchString.Text;
                }
                else
                {
                    url = "https://www.google.com/search?q=" + SearchString.Text;
                }
                //拉起浏览器
                try
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                catch (Exception ex)
                {
                    MessageHelper.Warning((string)Application.Current.Resources["OpenBrowserError"] +ex.Message);
                }
            }
        }

        //初始化
        private void Init()
        {
            //初始化计时器
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, e) =>
            {
                //按照 小时：分钟 格式显示
                CurrentTime = DateTime.Now.ToString("HH:mm");
                //按照 年份：月：日 星期 显示
                CurrentDate = DateTime.Now.ToString("yyyy年 MM月 dd日 dddd");
            };
            _timer.Start();
        }
    }
}
