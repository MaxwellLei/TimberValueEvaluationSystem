using Esri.ArcGISRuntime.Location;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    //常规设置页面的ViewModel
    class SCommonPageViewModel: ViewModelBase
    {
        private bool changeConfig = false;  //是否第一次进入设置页面
        private bool isInsideChange = false;    //是否是代码层面改变设置的值

        private string dbLocationPath;  //数据库路径
        private string wpLocationPath;  //数据库路径

        private int dbLocation;     //数据库位置设置(0是默认位置，即文档；1是自定义位置)
        public int DbLocation
        {
            get { return dbLocation; }
            set { Set(ref dbLocation, value);}
        }

        private int wpLocation;     //工作区位置设置(0是默认位置，即文档；1是自定义位置)
        public int WpLocation
        {
            get { return wpLocation; }
            set { Set(ref wpLocation, value);}
        }

        private int language;     //语言设置(0是中文;1是英文)
        public int Language
        {
            get { return language; }
            set { Set(ref language, value);}
        }

        private int font;     //字体设置(0是微软雅黑;1是添加字体)
        public int Font
        {
            get { return font; }
            set { Set(ref font, value);}
        }

        private int fontSize;     //字体大小设置(0是12;1是13;2是14;3是15;4是16)
        public int FontSize
        {
            get { return fontSize; }
            set { Set(ref fontSize, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private int autoOffTime;    //通知自动关闭时间(0是5秒;1是4秒;2是3秒;3是2秒)
        public int AutoOffTime
        {
            get { return autoOffTime; }
            set { Set(ref autoOffTime, value);}
        }

        private bool boot;  //开机自动启动
        public bool Boot
        {
            get { return boot; }
            set { Set(ref boot, value);}
        }

        private bool autoCheck;  //自动检查更新
        public bool AutoCheck
        {
            get { return autoCheck; }
            set { Set(ref autoCheck, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private int exitMode;    //退出方式(0是最小化到托盘;1是退出程序)
        public int ExitMode
        {
            get { return exitMode; }
            set { Set(ref exitMode, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        public RelayCommand OpenDbFolderCommand { get; private set; }   //打开数据库文件夹命令
        public RelayCommand OpenWsFolderCommand { get; private set; }   //打开工作区文件夹命令
        public RelayCommand DbLocationChangedCommand { get; private set; }   //修改数据库位置命令
        public RelayCommand WsLocationChangedCommand { get; private set; }   //修改工作区位置命令
        public RelayCommand AutoOffTimeChangedCommand { get; private set; }   //修改通知自动关闭时间命令
        public RelayCommand LanguageChangedCommand { get; private set; }   //修改语言命令
        public RelayCommand BootCommand { get; private set; }   //开机启动命令

        //初始化
        public SCommonPageViewModel()
        {
            OpenDbFolderCommand = new RelayCommand(ExecuteOpenDbFolderCommand);
            OpenWsFolderCommand = new RelayCommand(ExecuteOpenWsFolderCommand);
            DbLocationChangedCommand = new RelayCommand(ExecuteDbLocationChangedCommand);
            WsLocationChangedCommand = new RelayCommand(ExecuteWsLocationChangedCommand);
            LanguageChangedCommand = new RelayCommand(ExecuteLanguageChangedCommand);
            AutoOffTimeChangedCommand = new RelayCommand(ExecuteAutoOffTimeChangedCommand);
            BootCommand = new RelayCommand(ExecuteBootCommand);
            ReadConfig();   //读取配置文件
            changeConfig = true;
        }

        //开机自动启动
        private void ExecuteBootCommand()
        {
            BootHelper.SetAutoRun(Boot);
            MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
        }

        //修改消息通知时间
        private void ExecuteAutoOffTimeChangedCommand()
        {
            switch (AutoOffTime)
            {
                case 0:
                    MessageHelper.waitTime = 5;
                    break;
                case 1:
                    MessageHelper.waitTime = 4;
                    break;
                case 2:
                    MessageHelper.waitTime = 3;
                    break;
                case 3:
                    MessageHelper.waitTime = 2;
                    break;
            }
            MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
        }

        //修改语言
        private void ExecuteLanguageChangedCommand()
        {
            if (Language == 0)
            {
                LanguageHelper.ChangeLanguage("zh-CN");
                ConfigHelper.SetConfig("language", "0");
            }
            else
            {
                LanguageHelper.ChangeLanguage("en-US");
                ConfigHelper.SetConfig("language", "1");
            }
            MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
        }

        //修改数据库位置
        private void ExecuteDbLocationChangedCommand()
        {
            //保存数据库位置设置
            if (DbLocation == 0)
            {
                if (!isInsideChange)
                {
                    Services.ConfigHelper.SetConfig("database_location", "0");
                    Services.ConfigHelper.SetConfig("database_location_path", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));  //如果是默认设置，则删除配置文件中的数据库位置
                    MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
                }
                isInsideChange = false;
            }
            else
            {
                dbLocationPath = FileHelper.GetFolderPath();  //获取文件夹路径
                if (dbLocationPath != null)
                {
                    Services.ConfigHelper.SetConfig("database_location", "1");
                    Services.ConfigHelper.SetConfig("database_location_path", dbLocationPath);  //如果是自定义设置，则保存配置文件中的数据库位置
                    MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
                }
                else
                {
                    isInsideChange = true;
                    DbLocation = 0;
                    MessageHelper.Warning((string)Application.Current.Resources["CancelModified"]);
                }
            }
        }

        //修改工作区的位置
        //打开指定文件夹
        private void ExecuteWsLocationChangedCommand()
        {
            //保存工作区位置设置
            if (WpLocation == 0)
            {
                if (!isInsideChange)
                {
                    ConfigHelper.SetConfig("workspace_location", "0");
                    ConfigHelper.SetConfig("workspace_location_path", 
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));  //如果是默认设置，则删除配置文件中的工作区位置
                    MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
                }
                isInsideChange = false;
            }
            else
            {
                wpLocationPath = FileHelper.GetFolderPath();  //获取文件夹路径
                if (wpLocationPath != null)
                {
                    Services.ConfigHelper.SetConfig("workspace_location", "1");
                    Services.ConfigHelper.SetConfig("workspace_location_path", wpLocationPath);  //如果是自定义设置，则保存配置文件中的工作区位置
                    MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
                }
                else
                {
                    isInsideChange = true;
                    WpLocation = 0;
                    MessageHelper.Warning((string)Application.Current.Resources["CancelModified"]);
                }

            }
        }



        //打开指定数据库文件夹
        private void ExecuteOpenDbFolderCommand()
        {
            FileHelper.Openxplorer(ConfigHelper.GetConfig("database_location_path"));
            MessageHelper.Success((string)Application.Current.Resources["OpenFolderSuccessfully"]);
        }

        //打开指定工作区文件夹
        private void ExecuteOpenWsFolderCommand()
        {
            FileHelper.Openxplorer(ConfigHelper.GetConfig("database_location_path"));
            MessageHelper.Success((string)Application.Current.Resources["OpenFolderSuccessfully"]);
        }

        //读取配置文件
        void ReadConfig()
        {
            if(ConfigHelper.GetConfig("database_location_path") == "")
            {
                Services.ConfigHelper.SetConfig("database_location_path", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));  //如果是默认设置，则删除配置文件中的数据库位置
            }

            if (ConfigHelper.GetConfig("workspace_location_path") == "")
            {
                Services.ConfigHelper.SetConfig("workspace_location_path", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));  //如果是默认设置，则删除配置文件中的数据库位置
            }

            DbLocation = int.Parse(Services.ConfigHelper.GetConfig("database_location"));   //读取数据文件设置
            WpLocation = int.Parse(Services.ConfigHelper.GetConfig("workspace_location"));  //读取工作区设置
            Language = int.Parse(Services.ConfigHelper.GetConfig("language"));  //读取语言
            Font = int.Parse(Services.ConfigHelper.GetConfig("font"));  //读取字体
            FontSize = int.Parse(Services.ConfigHelper.GetConfig("font_size"));  //读取字体大小
            AutoOffTime = int.Parse(Services.ConfigHelper.GetConfig("auto_off_time"));    //读取消息通知时间
            Boot = bool.Parse(Services.ConfigHelper.GetConfig("boot"));     //读取是否自动开机
            AutoCheck = bool.Parse(Services.ConfigHelper.GetConfig("auto_check_update"));   //读取是否自动检查更新
            ExitMode = int.Parse(Services.ConfigHelper.GetConfig("exit_program_mode"));  //读取退出方式
        }

        //保存修改后的配置文件
        void SaveConfig()
        {
            Services.ConfigHelper.SetConfig("language", Language.ToString());  //保存语言设置
            Services.ConfigHelper.SetConfig("font", Font.ToString());  //保存字体设置
            Services.ConfigHelper.SetConfig("font_size", FontSize.ToString());  //保存字体大小设置
            Services.ConfigHelper.SetConfig("auto_off_time", AutoOffTime.ToString());  //保存消息通知时间设置
            Services.ConfigHelper.SetConfig("boot", Boot.ToString());  //保存是否自动开机设置
            Services.ConfigHelper.SetConfig("auto_check_update", AutoCheck.ToString());  //保存是否自动检查更新设置
            Services.ConfigHelper.SetConfig("exit_program_mode", ExitMode.ToString());  //保存退出方式设置
        }


    }
}
