using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Models
{
    public class DatabaseTreeItem
    {
        public string IconType { get; set; } // 图标类型
        public string Path { get; set; } // 表示是文件还是文件夹
        public string FName { get; set; } // 文件的名称
        //public ObservableCollection<DatabaseTreeItem> Children { get; set; } // 文件夹中的子文件夹或文件
    }
}
