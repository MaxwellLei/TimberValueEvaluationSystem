using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        //当前时间
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    RaisePropertyChanged("CurrentTime");
                }
            }
        }
        //当前日期
        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    RaisePropertyChanged("CurrentDate");
                }
            }
        }

        //计时器对象
        private DispatcherTimer _timer;

        public HomePageViewModel()
        {
            //初始化计时器
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                //按照 小时：分钟 格式显示
                CurrentTime = DateTime.Now.ToString("hh:mm");
                //按照 年份：月：日 星期 显示
                CurrentDate = DateTime.Now.ToString("yyyy年 MM月 dd日 dddd");
            };
            _timer.Start();
        }
    }
}
