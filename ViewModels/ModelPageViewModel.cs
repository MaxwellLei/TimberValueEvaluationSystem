using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using TimberValueEvaluationSystem.Views;
using System.Windows.Controls;

namespace TimberValueEvaluationSystem.ViewModels
{
    class ModelPageViewModel: ViewModelBase
    {
        private Frame Nav;  //导航对象
        public RelayCommand SiteQualityPageCommand { get; private set; }   //立地质量模型页面
        public RelayCommand SiteQualityPredictionCommand { get; private set; }   //立地质量模型预测
        public RelayCommand ImportModelCommand { get; private set; }   //立地质量模型预测




        public ModelPageViewModel(Frame frame)
        {
            Nav = frame;
            SiteQualityPredictionCommand = new RelayCommand(ExecuteSiteQualityPredictionCommand);
            SiteQualityPageCommand = new RelayCommand(ExecuteSiteQualityPageCommand);
            Nav.Navigate(MSiteQModelView.GetPage());
        }

        //切换立地质量模型页面
        private void ExecuteImportModelCommand()
        {

        }

        //切换立地质量模型页面
        private void ExecuteSiteQualityPageCommand()
        {
            Nav.Navigate(MSiteQModelView.GetPage());
        }

        //立地质量模型预测
        private void ExecuteSiteQualityPredictionCommand()
        {

        }
    }

}
