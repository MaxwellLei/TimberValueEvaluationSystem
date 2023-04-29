using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
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



        //模型表
        List<SiteQModel> siteQModels;
        public List<SiteQModel> SiteQModels
        {
            get { return siteQModels; }
            set
            {
                siteQModels = value;
                RaisePropertyChanged(() => SiteQModels);
            }
        }


        public DataPageViewModel()
        {
            NewDatabaseCommand = new RelayCommand(ExecuteNewDatabaseCommand);
            NewModelDBTableCommand = new RelayCommand(ExecuteNewModelDBTableCommand);
            InsertDataCommand = new RelayCommand(ExecuteInsertDataCommand);
            SiteQModels = new List<SiteQModel>();
        }

        //新建数据库
        public void ExecuteNewDatabaseCommand()
        {
            DatabaseHelper.CreateDatabase();
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
