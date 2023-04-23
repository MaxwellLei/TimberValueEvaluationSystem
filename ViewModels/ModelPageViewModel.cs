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
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
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

            // 加载 ONNX 模型
            var session = new InferenceSession(@"Resources/PythonModel/SiteQuality_DecisionTree/model.onnx");

            // 准备输入数据
            float[] inputData = { 3, 3, 3, 3 };
            var tensor = new DenseTensor<float>(inputData, new[] { 1, 4 });


            // 将输入数据包装在 NamedOnnxValue 对象中
            var namedOnnxValues = new[] { NamedOnnxValue.CreateFromTensor("input", tensor) };

            //运行模型
            var results = session.Run(namedOnnxValues);

            // 获取预测结果
            var predictionResult = results.First().AsTensor<Int64>();
            var predictedLabel = predictionResult.ToArray()[0];

            var b = results.Last();
            var a = results.Last().Value;
            var c = a.ToString();

            MessageBox.Show(predictedLabel+ ";" + a);
        }
    }

}
