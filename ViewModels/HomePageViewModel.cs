﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HarfBuzzSharp;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {

        
        private DispatcherTimer _timer;     //计时器对象

        //是否展示卡片
        private bool _cardsStatus;
        public bool CardsStatus
        {
            get { return _cardsStatus; }
            set { Set(ref _cardsStatus, value); }
        }

        //一言内容
        private string _words;
        public string Words
        {
            get { return _words; }
            set { Set(ref _words, value); }
        }

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

        private ObservableCollection<IconDefaultControlModel> _userControlViewModels;
        public ObservableCollection<IconDefaultControlModel> UserControlViewModels
        {
            get { return _userControlViewModels; }
            set { Set(ref _userControlViewModels, value); }
        }

        public ICommand AddCardCommand { get; private set; }    //创建卡片命令
        public ICommand RemoveCardCommand { get; private set; }     //删除卡片命令
        public RelayCommand ShowOrHideCardCommand { get; private set; }   //搜索开始命令
        public RelayCommand SearchStartedCommand { get; private set; }   //搜索开始命令


        public HomePageViewModel()
        {
            AddCardCommand = new RelayCommand(ExecuteAddCardCommand);
            RemoveCardCommand = new RelayCommand(ExecuteRemoveCardCommand);
            ShowOrHideCardCommand = new RelayCommand(ExecuteShowOrHideCardCommand);
            SearchStartedCommand = new RelayCommand(ExecuteSearchStartedCommand);

            Init();     //初始化
        }

        //显示和隐藏卡片
        private void ExecuteShowOrHideCardCommand()
        {
            CardsStatus = !CardsStatus;
        }

        //删除卡片
        private void ExecuteRemoveCardCommand()
        {

        }

        //创建卡片
        private void ExecuteAddCardCommand()
        {
            // 根据需要创建新的用户控件视图模型实例
            IconDefaultControlModel newUserControlVM = 
                new IconDefaultControlModel("../Resources/Image/Other/Seraphine0.png", "Name","hhhhhh");
            UserControlViewModels.Add(newUserControlVM);
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

            //获取一言
            GetWordsAsync();

            //展示卡片
            CardsStatus = true;

            //初始化队列
            UserControlViewModels = new ObservableCollection<IconDefaultControlModel>();
        }

        //获取一言
        private async Task GetWordsAsync()
        {
            string url = "https://v1.hitokoto.cn/?c=f&encode=text"; // 替换为您的实际网址

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string fetchedText = await httpClient.GetStringAsync(url);
                    Words = fetchedText;
                }
                catch (Exception ex)
                {
                    Words = "你我一起，改变世界";
                }
            }
        }
    }
}
