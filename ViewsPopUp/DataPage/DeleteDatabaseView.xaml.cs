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
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class DeleteDatabaseView
    {
        //回调委托
        private readonly Action _callback;

        public DeleteDatabaseView(string databasename,Action callback)
        {
            InitializeComponent();
            DatabaseName.Text = databasename;
            _callback = callback;
        }
        //点击确认后
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //执行回调
            _callback?.Invoke();
            ControlCommands.Close.Execute(this,this);
        }
        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Growl.Warning("取消删除");
            ControlCommands.Close.Execute(this, this);
        }
    }
}
