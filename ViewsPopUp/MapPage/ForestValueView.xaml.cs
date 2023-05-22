using HandyControl.Controls;
using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class ForestValueView
    {
        public bool IsEnd;    //这个窗口是否结束
        public string DatabaseName = null;

        public ForestValueView()
        {
            InitializeComponent();
            IsEnd = false;
        }
        //点击确认后
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsEnd = true;
            Growl.Warning("取消创建");
            ControlCommands.Close.Execute(this, this);
        }
    }
}
