using HandyControl.Controls;
using HandyControl.Interactivity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
using static SkiaSharp.HarfBuzz.SKShaper;

namespace TimberValueEvaluationSystem.ViewsPopUp.DataPage
{
    public partial class NewCardView
    {

        //回调委托
        private readonly Action<DialogResults> _callback;

        public NewCardView(Action<DialogResults> callback)
        {
            InitializeComponent();
            _callback = callback;
        }
        //点击确认后
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmpty())
            {
                if (IsNameRepeat())
                {
                    //创建返回消息
                    var resultData = new DialogResults();
                    resultData.AddValue("Icon", (object)_cardImage.Uri.ToString());
                    resultData.AddValue("Name", (object)_cardName.Text);
                    resultData.AddValue("Describe", (object)_cardDescribe.Text);
                    resultData.AddValue("Link", (object)_cardLink.Text);

                    errorMessage.Text = "";
                    //执行回调
                    _callback?.Invoke(resultData);
                    //关闭弹窗
                    ControlCommands.Close.Execute(this, this);
                }
            }
            else
            {
                errorMessage.Text = "存在空内容";
            }
            
        }
        //点击取消后
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            //消息提醒
            Growl.Warning((string)Application.Current.Resources["CancelCreation"]);
            //关闭弹窗
            ControlCommands.Close.Execute(this, this);
        }

        //属性验证
        private bool IsEmpty()
        {
            if (_cardImage.Uri == null) return true;
            if (_cardName.Text == "") return true;
            if (_cardDescribe.Text == "") return true;
            if (_cardLink.Text == "") return true;
            return false;
        }

        //名称检查
        private bool IsNameRepeat()
        {
            string filePath = ConfigHelper.GetConfig("json_location_path"); // 您的 JSON 文件路径
            // 读取 JSON 文件
            string json = File.ReadAllText(filePath);
            // 解析 JSON 数据为 JArray
            JArray jsonArray = JArray.Parse(json);
            // 遍历 JSON 对象，并根据指定的 "name" 值删除对象
            for (int i = jsonArray.Count - 1; i >= 0; i--)
            {
                if (jsonArray[i]["Text"]?.ToString() == _cardName.Text)
                {
                    jsonArray.RemoveAt(i);
                    errorMessage.Text = "名称重复";
                    return false;
                    break;
                }
            }
            return true;
        }
    }
}
