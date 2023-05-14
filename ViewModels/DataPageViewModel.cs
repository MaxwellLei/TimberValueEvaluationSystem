using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.Views;
using TimberValueEvaluationSystem.ViewsPopUp.DataPage;

namespace TimberValueEvaluationSystem.ViewModels
{
    class DataPageViewModel: ViewModelBase
    {
        public RelayCommand SetDatabaseLocationCommand { get; private set; }   //设定数据库位置
        public RelayCommand RefreshDbCommand { get; private set; }   //刷新数据库
        public RelayCommand NewDBTableCommand { get; private set; }   //新建数据库
        public RelayCommand DeleteDBTableCommand { get; private set; }   //删除数据库
        public RelayCommand OpenDbFolderCommand { get; private set; }   //打开数据库位置
        public RelayCommand PageChangedCommand { get; private set; }   //选择&跳转页数


        //选中的本地数据库列表项目
        private DatabaseTree _selectedDbItem;
        public DatabaseTree SelectedDbItem
        {
            get { return _selectedDbItem; }
            set { Set(ref _selectedDbItem, value); OnSelectedDbItemChanged(); }
        }

        //选中的本地数据表列表项目
        private DatabaseTable _selectedTableItem;
        public DatabaseTable SelectedTableItem
        {
            get { return _selectedTableItem; }
            set { Set(ref _selectedTableItem, value); OnSelectedTableItemChanged(0); }
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

        //立地质量模型表
        List<SiteQModel> siteQModels;
        public List<SiteQModel> SiteQModels
        {
            get { return siteQModels; }
            set { Set(ref siteQModels, value);}
        }

        //通用数据表
        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set { Set(ref _dataTable, value); }
        }

        //数据表是否为空
        private bool _isTableDataEmpty;
        public bool IsTableDataEmpty
        {
            get { return _isTableDataEmpty; }
            set { Set(ref _isTableDataEmpty, value); }
        }

        //跳转页数
        private string _pageCount;
        public string PageCount
        {
            get { return _pageCount; }
            set { Set(ref _pageCount, value); }
        }

        //最大页数
        private string _maxPageCount;
        public string MaxPageCount
        {
            get { return _maxPageCount; }
            set { Set(ref _maxPageCount, value); }
        }


        //初始化
        public DataPageViewModel()
        {
            //命令绑定
            SetDatabaseLocationCommand = new RelayCommand(ExecuteSetDatabaseLocationCommand);
            NewDBTableCommand = new RelayCommand(ExecuteNewDBCommandAsync);
            DeleteDBTableCommand = new RelayCommand(ExecuteDeleteDBCommand);
            RefreshDbCommand = new RelayCommand(ExecuteRefreshDbCommand);
            OpenDbFolderCommand = new RelayCommand(ExecuteOpenDbFolderCommand);
            PageChangedCommand = new RelayCommand(ExecutePageChangedCommand);
            SiteQModels = new List<SiteQModel>();

            InitList();     //初始化数据库
            IsTableListEmpty = true;    //初始化数据表提示显示
            IsTableDataEmpty = false;   //初始化数据表是否为空
            MaxPageCount = "10";    //初始化最大导航页数
        }

        //最大页码显示页数计算
        private void MaxPageCountCalculate()
        {
            int temp = DatabaseHelper.GetTableCount(SelectedDbItem.Path, SelectedTableItem.TName);
            if(temp != 0)
            {
                if(temp % 10 == 0)
                {
                    MaxPageCount = (temp / 10).ToString();
                }
                else
                {
                    MaxPageCount = (temp / 10 + 1).ToString();
                }
            }
            else
            {
                MaxPageCount = "1";
                IsTableDataEmpty = true;
            }
        }

        //选择&跳转页数
        private void ExecutePageChangedCommand()
        {
            OnSelectedTableItemChanged((Convert.ToInt32(PageCount)-1)*10);
            //Growl.Info($"选中的页数：{PageCount}");
        }

        //选中数据库触发
        private void OnSelectedDbItemChanged()
        {
            int temp = 0;
            if (SelectedDbItem != null)
            {
                temp = DatabaseHelper.FindAllTablesCount(SelectedDbItem.Path);
            }
            if (temp != 0 && temp !=null)
            {
                InitTableList();
            }
            else
            {
                DatabaseTable = null;
                IsTableListEmpty = true;
            }
        }

        //选中数据表触发
        private void OnSelectedTableItemChanged(int countLocation)
        {
            DataTable = DatabaseHelper.GetDataTable(SelectedDbItem.Path, SelectedTableItem.TName,countLocation);
            MaxPageCountCalculate();
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
            if (SelectedDbItem != null)
            {
                List<string> strings = DatabaseHelper.FindAllTables(SelectedDbItem.Path);
                DatabaseTable = new ObservableCollection<DatabaseTable>();
                foreach(var table in strings)
                {
                    if (!string.IsNullOrEmpty(table))
                    {
                        DatabaseTable.Add(new DatabaseTable()
                        {
                            TName = table
                        });
                        IsTableListEmpty = false;
                    }else
                    {
                        IsTableListEmpty = true;
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

        //新建数据库
        private async void ExecuteNewDBCommandAsync()
        {
            //获取异步任务
            CancellationTokenSource cts = cts = new CancellationTokenSource();
            //创建获取信息窗口
            var newDatabaseView = new NewDatabaseView();
            Dialog.Show(newDatabaseView);
            Task task = Task.Run(() =>
            {
                while (!cts.IsCancellationRequested && !newDatabaseView.IsEnd)
                {
                    if (newDatabaseView.DatabaseName != null)
                    {
                        // 新建数据库
                        DatabaseHelper.CreateDatabase(ConfigHelper.GetConfig("database_location_path"), newDatabaseView.DatabaseName);
                        // 刷新列表
                        InitList();
                        cts.Cancel(); // 成功执行后立刻取消任务
                    }
                }
            }, cts.Token);

            await task;
            //DatabaseHelper.CreateMSQMTable();
            //Growl.Info(Convert.ToString(SelectedDbItem));
            //Growl.Info("创建表成功");
        }

        //删除数据库
        private async void ExecuteDeleteDBCommand()
        {
            //管道通信传递数据
            CommunicationChannel.text = SelectedDbItem.FName;
            //获取异步任务
            CancellationTokenSource cts = new CancellationTokenSource(); 
            //创建获取信息窗口
            var deleteDatabaseView = new DeleteDatabaseView();
            Dialog.Show(deleteDatabaseView);
            Task task = Task.Run(() =>
            {
                while (deleteDatabaseView.IsEnd >= 0)
                {
                    if (deleteDatabaseView.IsEnd > 0)
                    {
                        //删除数据库
                        if (FileHelper.DeleteFile(SelectedDbItem.Path) == true)
                        {
                            //刷新列表
                            InitList();
                            deleteDatabaseView.IsEnd -= 2;
                            cts.Cancel();
                        }
                    }
                }
                CommunicationChannel.text = null;
            }, cts.Token);
            await task;
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
