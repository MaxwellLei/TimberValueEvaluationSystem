using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HarfBuzzSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.ViewsPopUp.DataPage;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
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

        private ObservableCollection<IconDefaultControlViewModel> _userControlViewModels;
        public ObservableCollection<IconDefaultControlViewModel> UserControlViewModels
        {
            get { return _userControlViewModels; }
            set { Set(ref _userControlViewModels, value); }
        }

        public ICommand AddCardCommand { get; private set; }    //创建卡片命令
        public ICommand RefreshCommand { get; private set; }     //删除卡片命令
        public RelayCommand ShowOrHideCardCommand { get; private set; }   //显示/隐藏命令
        public RelayCommand SearchStartedCommand { get; private set; }   //搜索开始命令


        public HomePageViewModel()
        {
            AddCardCommand = new RelayCommand(ExecuteAddCardCommand);
            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
            ShowOrHideCardCommand = new RelayCommand(ExecuteShowOrHideCardCommand);
            SearchStartedCommand = new RelayCommand(ExecuteSearchStartedCommand);
            Init();     //初始化
        }

        //显示和隐藏卡片
        private void ExecuteShowOrHideCardCommand()
        {
            CardsStatus = !CardsStatus;
        }

        //刷新
        private void ExecuteRefreshCommand()
        {
            ReadUserControlViewModels();
            MessageHelper.Info((string)Application.Current.Resources["Refreshsuccessfully"]);
        }


        //创建卡片
        private void ExecuteAddCardCommand()
        {
            //弹窗
            var newCardView = new NewCardView(ConfirmCallback);
            Dialog.Show(newCardView);
        }

        //弹出确认结束回调函数
        private void ConfirmCallback(DialogResults result)
        {
            IconDefaultControlViewModel newUserControlVM =
                new IconDefaultControlViewModel(ReadUserControlViewModels,
                                                result.GetValue<string>("Icon"),
                                                result.GetValue<string>("Name"),
                                                result.GetValue<string>("Describe"),
                                                result.GetValue<string>("Link"),
                                                1,
                                                1);
            UserControlViewModels.Add(newUserControlVM);
            SaveUserControlViewModels();
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
            //更新时间
            Task.Run(async () => await UpdatedTimeAsync());

            //获取一言
            //GetWordsAsync();

            //展示卡片
            CardsStatus = true;

            //初始化Json文件
            if (ConfigHelper.GetConfig("json_location_path") == "")
            {
                //如果AppData文件夹不存在，则创建文件夹
                FileHelper.CreateFolder(Directory.GetCurrentDirectory() + "\\Resources\\AppData");
                //设置Json路径
                ConfigHelper.SetConfig("json_location_path", Directory.GetCurrentDirectory()+ "\\Resources\\AppData\\List.json");
            }
            //刷新列表
            ReadUserControlViewModels();

        }

        //保存json文件
        public void SaveUserControlViewModels()
        {
            string json = JsonConvert.SerializeObject(UserControlViewModels);
            File.WriteAllText(ConfigHelper.GetConfig("data_location_path"), json);
        }

        //读取json文件
        public void ReadUserControlViewModels()
        {
            UserControlViewModels = new ObservableCollection<IconDefaultControlViewModel>();
            if (File.Exists(ConfigHelper.GetConfig("json_location_path")))
            {
                string json = File.ReadAllText(ConfigHelper.GetConfig("json_location_path"));
                foreach(var item in JsonConvert.DeserializeObject<ObservableCollection<IconDefaultControlViewModel>>(json))
                {
                    item._callback = ReadUserControlViewModels;
                    UserControlViewModels.Add(item);
                }
            }
        }

        //更新时间
        private async Task UpdatedTimeAsync()
        {
            // 初始化计时器
            while (true)
            {
                // 按照 小时：分钟 格式显示
                CurrentTime = DateTime.Now.ToString("HH:mm");
                // 按照 年份：月：日 星期 显示
                CurrentDate = DateTime.Now.ToString("yyyy年 MM月 dd日 dddd");
                await Task.Delay(1000);
            }
        }

        //获取一言
        private async Task GetWordsAsync()
        {
            string url = "https://v1.hitokoto.cn/?c=f&encode=text";

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
