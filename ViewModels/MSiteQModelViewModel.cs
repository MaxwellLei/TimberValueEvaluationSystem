using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using HandyControl.Controls;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    class MSiteQModelViewModel: ViewModelBase
    {
        //辅助变量
        private float[] testdata;

        //图表绑定
        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new LineSeries<int>
            {
                Values = new int[] { 4, 6, 5, 3, -3, -1, 2 }
            },
            new ColumnSeries<double>
            {
                Values = new double[] { 2, 5, 4, -2, 4, -3, 5 }
            }
        }; //图表

        public ISeries[] pieSeries;
        public ISeries[] PieSeries
        {
            get { return pieSeries; }
            set { Set(ref pieSeries, value); }
        }

        //方法命令绑定
        public RelayCommand SingleItemCommand { get; private set; }   //切换单项预测
        public RelayCommand ImportCSVCommand { get; private set; }   //切换导入CSV
        public RelayCommand DatabaseImportCommand { get; private set; }   //切换数据库导入
        public RelayCommand PrevCommand { get; private set; }   //下一步命令
        public RelayCommand NextCommand { get; private set; }   //下一步命令

        //模型预测结果反馈前端
        private string modelResult;
        public string ModelResult
        {
            get { return modelResult; }
            set { Set(ref modelResult, value); }
        }

        //获取Combox的数据
        private int soilIndex;  //土壤厚度
        public int SoilIndex
        {
            get { return soilIndex; }
            set { Set(ref soilIndex, value); }
        }
        private int slopeIndex;     //坡位
        public int SlopeIndex
        {
            get { return slopeIndex; }
            set { Set(ref slopeIndex, value); }
        }
        private int aspectIndex;     //坡向
        public int AspectIndex
        {
            get { return aspectIndex; }
            set { Set(ref aspectIndex, value); }
        }
        private int gradientIndex;     //坡度
        public int GradientIndex
        {
            get { return gradientIndex; }
            set { Set(ref gradientIndex, value); }
        }

        //步骤模块前端双向数据
        private bool isShowStackPannl1;
        public bool IsShowStackPannl1
        {
            get { return isShowStackPannl1; }
            set { Set(ref isShowStackPannl1, value); }
        }
        private bool isShowStackPannl2;
        public bool IsShowStackPannl2
        {
            get { return isShowStackPannl2; }
            set { Set(ref isShowStackPannl2, value); }
        }

        //步骤条
        private int indexCount = 0;
        public int IndexCount
        {
            get { return indexCount; }
            set { Set(ref indexCount, value); }
        }

        private StackPanel _stackPanel1, _stackPanel2, _stackPanel3;

        //初始化
        public MSiteQModelViewModel(StackPanel stackPanel1,StackPanel stackPanel2,StackPanel stackPanel3) {

            GradientIndex = 0;
            AspectIndex = 0;
            SlopeIndex = 0;
            SoilIndex = 0;
            IsShowStackPannl1 = true;
            _stackPanel1 = stackPanel1;
            _stackPanel2 = stackPanel2;
            _stackPanel3 = stackPanel3;
            SingleItemCommand = new RelayCommand(ExecuteImportModelCommand);
            ImportCSVCommand = new RelayCommand(ExecuteImportCSVCommand);
            DatabaseImportCommand = new RelayCommand(ExecuteDatabaseImportCommand);
            NextCommand = new RelayCommand(ExecuteNextCommand);
            PrevCommand = new RelayCommand(ExecutePrevCommand);

        }

        //下一步命令
        private void ExecutePrevCommand()
        {
            ShowStackPanel(-1);
        }

        //下一步命令
        private void ExecuteNextCommand()
        {
            ShowStackPanel(1);
        }

        private void ShowStackPanel(int value)
        {
            if (IndexCount == 0)
            {
                if (value>0)
                {
                    IsShowStackPannl1 = false;
                    IsShowStackPannl2 = true;
                    IndexCount += value;
                }
            }
            else if (IndexCount == 1)
            {
                if(value < 0)
                {
                    //返回第一步
                    IsShowStackPannl2 = false;
                    IsShowStackPannl1 = true;
                }
                else
                {
                    //第三步
                    IsShowStackPannl2 = false;
                    InputData();
                    ModelResult = ConverterHelper.SiteQualityModelConverter(SiteQualityModel(testdata));
                    PieChartInit();
                    MessageHelper.Success("预测成功");
                }
                IndexCount += value;
            }
            else
            {
                if (value < 0)
                {
                    //返回第一步
                    IsShowStackPannl2 = true;
                    IndexCount += value;
                    RefreshSiteQModel();
                }
            }
        }
        
        //调用模型预测方法
        private long SiteQualityModel(float[] data)
        {
            // 加载 ONNX 模型
            var session = new InferenceSession(@"Resources/PythonModel/SiteQuality_DecisionTree/model.onnx");

            // 准备输入数据
            float[] inputData = data;
            var tensor = new DenseTensor<float>(inputData, new[] { 1, 4 });


            // 将输入数据包装在 NamedOnnxValue 对象中
            var namedOnnxValues = new[] { NamedOnnxValue.CreateFromTensor("input", tensor) };

            //运行模型
            var results = session.Run(namedOnnxValues);

            // 获取预测结果
            var predictionResult = results.First().AsTensor<Int64>();
            var predictedLabel = predictionResult.ToArray()[0];

            return predictedLabel;
        }

        //重置预测模块
        private void RefreshSiteQModel()
        {
            PieSeries = null;   //清空饼图
            ModelResult = null;     //清空结果文字
        }

        //填写预测数据
        private void InputData()
        {
            testdata = new float[4];
            testdata[0] = SoilIndex + 1;
            testdata[1] = SlopeIndex + 1;
            testdata[2] = AspectIndex + 1;
            testdata[3] = GradientIndex + 1;
        }

        //初始化饼图
        private void PieChartInit()
        {
            PieSeries = new ISeries[]
            {
                new PieSeries<double> { Name="Right", Values = new double[] { 99 } },
                new PieSeries<double> { Name="Wrong", Values = new double[] { 1 } }
            };
        }

        //切换单项预测
        private void ExecuteImportModelCommand()
        {
            _stackPanel1.Visibility = System.Windows.Visibility.Visible;
            _stackPanel2.Visibility = System.Windows.Visibility.Hidden;
            _stackPanel3.Visibility = System.Windows.Visibility.Hidden;
        }

        //切换导入CSV
        private void ExecuteImportCSVCommand()
        {
            _stackPanel1.Visibility = System.Windows.Visibility.Hidden;
            _stackPanel2.Visibility = System.Windows.Visibility.Visible;
            _stackPanel3.Visibility = System.Windows.Visibility.Hidden;
        }

        //切换数据库导入
        private void ExecuteDatabaseImportCommand()
        {
            _stackPanel1.Visibility = System.Windows.Visibility.Hidden;
            _stackPanel2.Visibility = System.Windows.Visibility.Hidden;
            _stackPanel3.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
