using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Python.Runtime;
using TimberValueEvaluationSystem.Views;

namespace TimberValueEvaluationSystem.ViewModels
{
    class ModelPageViewModel: ViewModelBase
    {
        public RelayCommand PredictByPklModelCommand { get; private set; }   //切换地图命令
        public ModelPageViewModel()
        {
            PredictByPklModelCommand = new RelayCommand(ExecutePredictByPklModelCommand);
        }

        //切换地图命令
        private void ExecutePredictByPklModelCommand()
        {

        }
    }

}
