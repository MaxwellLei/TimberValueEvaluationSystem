using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    class DataPageViewModel: ViewModelBase
    {
        public RelayCommand SetDatabaseLocationCommand { get; private set; }   //设定数据库位置
        public RelayCommand RefreshDbCommand { get; private set; }   //刷新数据库
        public RelayCommand NewModelDBTableCommand { get; private set; }   //新建数据库
        public RelayCommand OpenDbFolderCommand { get; private set; }   //打开数据库位置

        //选中的本地数据库列表项目
        private DatabaseTree _selectedItem;
        public DatabaseTree SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); OnSelectedItemChanged(); }
        }

        //左侧数据库列表
        private ObservableCollection<DatabaseTree> databaseTree;
        public ObservableCollection<DatabaseTree> DatabaseTree
        {
            get { return databaseTree; }
            set { Set(ref databaseTree, value); }
        }

        //左侧数据表列表
        private ObservableCollection<DatabaseTable> _databaseTable;
        public ObservableCollection<DatabaseTable> DatabaseTable
        {
            get { return _databaseTable; }
            set { Set(ref _databaseTable, value); }
        }


        //左侧数据库是否为空
        private bool _isListEmpty;
        public bool IsListEmpty
        {
            get { return _isListEmpty; }
            set { Set(ref _isListEmpty, value); }
        }

        //左侧数据表是否为空
        private bool _isTableListEmpty;
        public bool IsTableListEmpty
        {
            get { return _isTableListEmpty; }
            set { Set(ref _isTableListEmpty, value); }
        }

        //模型表
        List<SiteQModel> siteQModels;
        public List<SiteQModel> SiteQModels
        {
            get { return siteQModels; }
            set { Set(ref siteQModels, value);}
        }


        //初始化
        public DataPageViewModel()
        {
            SetDatabaseLocationCommand = new RelayCommand(ExecuteSetDatabaseLocationCommand);
            NewModelDBTableCommand = new RelayCommand(ExecuteNewModelDBTableCommand);
            RefreshDbCommand = new RelayCommand(ExecuteRefreshDbCommand);
            OpenDbFolderCommand = new RelayCommand(ExecuteOpenDbFolderCommand);
            SiteQModels = new List<SiteQModel>();

            InitList();
        }

        //选中数据库触发
        private void OnSelectedItemChanged()
        {
            //if (SelectedItem != null)
            //{
            //    // 在这里执行你需要的操作，例如显示一个MessageBox
            //    MessageBox.Show($"选中的项目是: {SelectedItem.FName}{SelectedItem.Path}");
            //}
            InitTableList();
        }

        //初始化左侧列表&刷新数据库
        private void InitList()
        {
            DatabaseTree = new ObservableCollection<DatabaseTree>();
            string dbPath;   //数据库路径
            //获取文件夹文件
            if (ConfigHelper.GetConfig("database_location") == "0")
            {
                //获取C盘用户文档路径
                dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                dbPath = ConfigHelper.GetConfig("database_location_path");
            }
            var files = Directory.GetFiles(dbPath);
            //遍历文件传入列表
            foreach (var file in files)
            {
                //判断列表是否为空
                if (File.Exists(file))
                {
                    //如果是数据库文件
                    if (file.Contains(".db"))
                    {
                        DatabaseTree.Add(new DatabaseTree()
                        {
                            IconType = null,
                            Path = file,
                            FName = Path.GetFileNameWithoutExtension(file)
                        });
                        IsListEmpty = false;
                    }
                    else
                    {
                        IsListEmpty = true;
                    }
                }
                else
                {
                    IsListEmpty = true;
                }
            }
        }

        //初始化表&刷新表
        private void InitTableList()
        {
            //如果选中了数据库
            if(SelectedItem != null)
            {
                List<string> strings = DatabaseHelper.FindAllTables(SelectedItem.Path);
                DatabaseTable = new ObservableCollection<DatabaseTable>();
                foreach(var table in strings)
                {
                    if (!string.IsNullOrEmpty(table))
                    {
                        DatabaseTable.Add(new DatabaseTable()
                        {
                            TName = table
                        });
                    }
                }
            }
        }

        //刷新数据库列表
        private void ExecuteRefreshDbCommand()
        {
            InitList();
            Growl.Success("刷新数据库成功");
        }

        //设置数据库位置
        private void ExecuteSetDatabaseLocationCommand()
        {
            //获取选定的数据库位置
            string dbPath = FileHelper.GetFolderPath();
            //如果不为空
            if(dbPath != null)
            {
                ConfigHelper.SetConfig("database_location", "1");
                ConfigHelper.SetConfig("database_location_path", dbPath);
                InitList();
                Growl.Success("设置数据库位置成功");
            }
            else
            {
                Growl.Warning("取消设置数据库位置");
            }

        }

        //新建模型表
        private void ExecuteNewModelDBTableCommand()
        {
            //DatabaseHelper.CreateMSQMTable();
            Growl.Info(Convert.ToString(SelectedItem));
            //Growl.Info("创建表成功");
        }

        //打开数据库文件夹
        private void ExecuteOpenDbFolderCommand()
        {
            if (ConfigHelper.GetConfig("database_location") == "0")
            {
                FileHelper.Openxplorer(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            else
            {
                FileHelper.Openxplorer(ConfigHelper.GetConfig("database_location_path"));
            }
            Growl.Success("打开文件位置成功");
        }

        //插入数据
        private void ExecuteInsertDataCommand()
        {
            DatabaseHelper.InsertDatabase("INSERT INTO YourTableName(SoilThickness, Slope, Aspect, Gradient, SiteQuality) VALUES('测试', '张三', '男', '这也是测试', '')");
            UpateGridDate();
            Growl.Info("插入数据成功");
        }

        //更新数据
        private void UpateGridDate()
        {
            string sql = "SELECT * FROM YourTableName LIMIT 14;";
            var result = DatabaseHelper.QueryDatabase(sql);
            while(result.Read())
            {
                SiteQModels.Add(new SiteQModel()
                {
                    Index = Convert.ToInt64(result["ID"]),
                    SoilThickness = result["SoilThickness"].ToString(),
                    Slope = result["Slope"].ToString(),
                    Aspect = result["Aspect"].ToString(),
                    Gradient = result["Gradient"].ToString(),
                    SiteQuality = result["SiteQuality"].ToString()
                });
            }
        }
    }
}
