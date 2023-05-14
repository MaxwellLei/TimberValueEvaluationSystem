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
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class DeleteDatabaseView
    {
        public int IsEnd = 0;     //这个窗口是否结束

        public DeleteDatabaseView()
        {
            InitializeComponent();
            DatabaseName.Text = CommunicationChannel.text;
        }
        //点击确认后
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsEnd = 1;
            ControlCommands.Close.Execute(this,this);
        }
        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsEnd = -1;
            Growl.Warning("取消删除");
            ControlCommands.Close.Execute(this, this);
        }
    }
}
