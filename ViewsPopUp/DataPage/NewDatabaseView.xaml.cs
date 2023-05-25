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

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class NewDatabaseView
    {
        //回调委托
        private readonly Action<DialogResults> _callback;

        public NewDatabaseView(Action<DialogResults> callback)
        {
            InitializeComponent();
            _callback = callback;
        }
        //点击确认后
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //创建返回消息
            var resultData = new DialogResults();
            resultData.AddValue("DatabaseName", (object)_databaseName.Text);
            _callback?.Invoke(resultData);
            ControlCommands.Close.Execute(this, this);
        }
        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Growl.Warning("取消创建");
            ControlCommands.Close.Execute(this, this);
        }
    }
}
