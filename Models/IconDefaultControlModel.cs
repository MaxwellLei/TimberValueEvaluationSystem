using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class IconDefaultControlModel
    {
        //图标
        public string Icon { get; set; }
        //标题
        public string Text { get; set; }
        //描述
        public string Description { get; set; }

        public IconDefaultControlModel(string icon, string text, string description)
        {
            Icon = icon;
            Text = text;
            Description = description;
        }
    }
}
