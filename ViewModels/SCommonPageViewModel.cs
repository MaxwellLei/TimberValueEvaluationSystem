using GalaSoft.MvvmLight;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.ViewModels
{
    //常规设置页面的ViewModel
    class SCommonPageViewModel: ViewModelBase
    {
        private bool changeConfig = false;

        private string dbLocationPath;  //数据库路径
        private string wpLocationPath;  //数据库路径

        private int dbLocation;     //数据库位置设置(0是默认位置，即文档；1是自定义位置)
        public int DbLocation
        {
            get { return dbLocation; }
            set { Set(ref dbLocation, value); 
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private int wpLocation;     //工作区位置设置(0是默认位置，即文档；1是自定义位置)
        public int WpLocation
        {
            get { return wpLocation; }
            set { Set(ref wpLocation, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private int language;     //语言设置(0是中文;1是英文)
        public int Language
        {
            get { return language; }
            set { Set(ref language, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private int font;     //字体设置(0是微软雅黑;1是添加字体)
        public int Font
        {
            get { return font; }
            set { Set(ref font, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
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
            set { Set(ref autoOffTime, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
        }

        private bool boot;  //开机自动启动
        public bool Boot
        {
            get { return boot; }
            set { Set(ref boot, value);
                if (changeConfig)
                {
                    Growl.Info("修改成功");
                    SaveConfig();
                }
            }
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


        //初始化
        public SCommonPageViewModel()
        {
            ReadConfig();   //读取配置文件
            changeConfig = true;
        }

        //读取配置文件
        void ReadConfig()
        {
            //读取数据文件设置
            DbLocation = int.Parse(Services.ConfigHelper.GetConfig("database_location"));
            if (DbLocation == 1)
            {
                Growl.Info("自定义路径");
            }

            //读取工作区设置
            WpLocation = int.Parse(Services.ConfigHelper.GetConfig("workspace_location"));
            if (WpLocation == 1)
            {
                Growl.Info("自定义路径");
            }

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
            //保存数据库位置设置
            if (DbLocation == 0)
            {
                Services.ConfigHelper.SetConfig("database_location", null);  //如果是默认设置，则删除配置文件中的数据库位置
            }
            else
            {
                Services.ConfigHelper.SetConfig("database_location", dbLocationPath);  //如果是自定义设置，则保存配置文件中的数据库位置
            }
            //保存工作区位置设置
            if (WpLocation == 0)
            {
                Services.ConfigHelper.SetConfig("workspace_location", null);  //如果是默认设置，则删除配置文件中的工作区位置
            }
            else
            {
                Services.ConfigHelper.SetConfig("workspace_location", wpLocationPath);  //如果是自定义设置，则保存配置文件中的工作区位置
            }
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
