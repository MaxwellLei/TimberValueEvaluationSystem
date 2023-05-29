using HandyControl.Controls;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class ForestValueView
    {
        //林班属性
        public ObservableCollection<string> FeatureAttributes { get; set; }

        //回调委托
        private readonly Action _callback;

        public ForestValueView(ObservableCollection<string> featureAttributes,Action action)
        {
            InitializeComponent();
            FeatureAttributes = featureAttributes;
            _callback = action;

            itemscontrol.ItemsSource = FeatureAttributes;
        }

        //点击评估后
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDataEmpty())
            {
                var temp = new double[Convert.ToInt32(_AverageAge.Value)];
                temp[0] = Convert.ToDouble(_FirstYearCost.Text);
                temp[1] = Convert.ToDouble(_SecondYearCost.Text);
                temp[2] = Convert.ToDouble(_ThirdYearCost.Text);
                temp[3] = Convert.ToDouble(_AnnualCost.Text);
                temp[4] = Convert.ToDouble(_AnnualCost.Text);
                _Result.Text = (Math.Truncate(CalculateE_n(Convert.ToDouble(_AdjustmentCoefficient.Text),
                    Convert.ToDouble(_ReturnOnInvestment.Text), temp, Convert.ToInt32(_AverageAge.Value)) * 100) / 100).ToString();
            }
        }

        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _callback?.Invoke();
            ControlCommands.Close.Execute(this, this);
        }

        //导出报告
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var path = FileHelper.GetFolderPath() + "导出报告.png";
            ExportGridAsImage(TopBorder, path);
            MessageHelper.Success(MessageHelper.GetString("ExportSucceeded"));
        }

        //成本法计算
        public static double CalculateE_n(double K, double p, double[] CiValues, int n)
        {
            double sum = 0;
            for (int i = 1; i <= n; i++)
            {
                sum += CiValues[i - 1] * Math.Pow(1 + p, n - i + 1);
            }

            return K * sum;
        }

        //验证数据是否为空
        private bool IsDataEmpty()
        {
            if(_AverageAge.Value == 0)
            {
                return true;
            }
            if(_FirstYearCost.Text == "")
            {
                return true;
            }
            if(_SecondYearCost.Text == "")
            {
                return true;
            }
            if(_ThirdYearCost.Text == "")
            {
                return true;
            }
            if(_AnnualCost.Text == "")
            {
                return true;
            }
            if(_AdjustmentCoefficient.Text == "")
            {
                return true;
            }
            if(_ReturnOnInvestment.Text == "")
            {
                return true;
            }
            return false;
        }

        //导出Border为图片
        private void ExportGridAsImage(Border border, string filePath)
        {
            // 对 Border 进行测量和排列
            border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            border.Arrange(new Rect(0, 0, border.DesiredSize.Width, border.DesiredSize.Height));

            // 创建一个渲染目标位图并渲染 Border
            var renderTargetBitmap = new RenderTargetBitmap((int)border.DesiredSize.Width, (int)border.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(border);

            // 将位图保存到文件
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(fileStream);
            }
        }

        //清空数据
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _AverageAge.Value = 0;
            _FirstYearCost.Text = "";
            _SecondYearCost.Text = "";
            _ThirdYearCost.Text = "";
            _AnnualCost.Text = "";
            _AdjustmentCoefficient.Text = "";
            _ReturnOnInvestment.Text = "";
            _Result.Text = "";
        }
    }
}
