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

namespace TimberValueEvaluationSystem.ViewModels
{
    class MSiteQModelViewModel: ViewModelBase
    {
        public ISeries[] Series { get; set; }   //图表
        public ISeries[] PieSeries { get; set; }   //饼图表
        public RelayCommand SingleItemCommand { get; private set; }   //切换单项预测
        public RelayCommand ImportCSVCommand { get; private set; }   //切换导入CSV
        public RelayCommand DatabaseImportCommand { get; private set; }   //切换数据库导入
        private StackPanel _stackPanel1,_stackPanel2,_stackPanel3;

        public MSiteQModelViewModel(StackPanel stackPanel1,StackPanel stackPanel2,StackPanel stackPanel3) {

            _stackPanel1 = stackPanel1;
            _stackPanel2 = stackPanel2;
            _stackPanel3 = stackPanel3;
            SingleItemCommand = new RelayCommand(ExecuteImportModelCommand);
            ImportCSVCommand = new RelayCommand(ExecuteImportCSVCommand);
            DatabaseImportCommand = new RelayCommand(ExecuteDatabaseImportCommand);

            Series = new ISeries[]
                {
                    new LineSeries<int>
                    {
                        Values = new int[] { 4, 6, 5, 3, -3, -1, 2 }
                    },
                    new ColumnSeries<double>
                    {
                        Values = new double[] { 2, 5, 4, -2, 4, -3, 5 }
                    }
                };
            PieSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 2 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 1 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 3 } }
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
