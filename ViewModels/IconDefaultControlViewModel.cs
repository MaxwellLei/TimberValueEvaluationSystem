using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimberValueEvaluationSystem.Models;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class IconDefaultControlViewModel: ViewModelBase
    {
        //图标
        public string Icon { get; set; }
        //标题
        public string Text { get; set; }
        //描述
        public string Description { get; set; }
        //描述
        public string Link { get; set; }
        //图标大小
        public int WidthtSize { get; set; }
        public int HeightSize { get; set; }

        //刷新回调委托
        [JsonIgnore]
        public Action _callback;

        [JsonIgnore]
        public RelayCommand ClickCardCommand { get; private set; }  //点击卡片命令
        [JsonIgnore]
        public RelayCommand DeleteCardCommand { get; private set; }  //删除卡片命令

        public IconDefaultControlViewModel(Action action,string icon, string text, string description, string link,int width, int height)
        {
            Icon = icon;
            Text = text;
            Description = description;
            WidthtSize = width * 100;
            HeightSize = height * 100;
            Link = link;
            _callback = action;


            ClickCardCommand = new RelayCommand(() =>
            {
                NetworkHelper.OpenBrowser(Link);
            });

            DeleteCardCommand = new RelayCommand(() =>
            {
                RemoveUserControlViewModels(Text);
                MessageHelper.Success("删除成功");
            });
        }

        //删除json中的对象
        public void RemoveUserControlViewModels(string targetName)
        {
            string filePath = ConfigHelper.GetConfig("json_location_path"); // 您的 JSON 文件路径
            // 读取 JSON 文件
            string json = File.ReadAllText(filePath);
            // 解析 JSON 数据为 JArray
            JArray jsonArray = JArray.Parse(json);
            // 遍历 JSON 对象，并根据指定的 "name" 值删除对象
            for (int i = jsonArray.Count - 1; i >= 0; i--)
            {
                if (jsonArray[i]["Text"]?.ToString() == targetName)
                {
                    jsonArray.RemoveAt(i);
                    break;
                }
            }
            // 将更新后的 JSON 数组保存回文件
            string updatedJson = jsonArray.ToString();
            File.WriteAllText(filePath, updatedJson);

            //执行刷新回调
            _callback?.Invoke();
        }
    }
}
