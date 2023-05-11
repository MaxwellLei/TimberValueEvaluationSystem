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
        public RelayCommand NewDatabaseCommand { get; private set; }   //新建数据库
        public RelayCommand NewModelDBTableCommand { get; private set; }   //新建数据库
        public RelayCommand InsertDataCommand { get; private set; }   //新建数据库


        //左侧数据库列表
        private ObservableCollection<DatabaseTree> databaseTree;
        public ObservableCollection<DatabaseTree> DatabaseTree
        {
            get { return databaseTree; }
            set { Set(ref databaseTree, value); }
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
            NewDatabaseCommand = new RelayCommand(ExecuteNewDatabaseCommand);
            NewModelDBTableCommand = new RelayCommand(ExecuteNewModelDBTableCommand);
            InsertDataCommand = new RelayCommand(ExecuteInsertDataCommand);
            SiteQModels = new List<SiteQModel>();

            InitList();
        }

        //初始化左侧列表
        private void InitList()
        {
            DatabaseTree = new ObservableCollection<DatabaseTree>();
            var files = Directory.GetFiles(@"Data/Database");
            foreach (var file in files)
            {
                DatabaseTree.Add(new DatabaseTree()
                {
                    IconType = null,
                    Path = file,
                    FName = Path.GetFileName(file)
                });
            }
        }

        //新建数据库
        public void ExecuteNewDatabaseCommand()
        {
            InitList();
            //DatabaseHelper.CreateDatabase();
        }

        //新建模型表
        private void ExecuteNewModelDBTableCommand()
        {
            DatabaseHelper.CreateMSQMTable();
            Growl.Info("创建表成功");
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
