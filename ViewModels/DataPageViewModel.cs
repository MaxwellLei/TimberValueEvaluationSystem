using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    class DataPageViewModel: ViewModelBase
    {
        
        public RelayCommand NewDatabaseCommand { get; private set; }   //新建数据库

        public DataPageViewModel()
        {
            NewDatabaseCommand = new RelayCommand(ExecuteNewDatabaseCommand);
        }

        //新建数据库
        private void ExecuteNewDatabaseCommand()
        {
            DatabaseHelper.CreateDatabase();
        }
    }
}
