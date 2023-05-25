using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Models
{
    //返回值类
    public class DialogResults
    {
        //存储字典
        public Dictionary<string, object> Values { get; }

        public DialogResults()
        {
            Values = new Dictionary<string, object>();
        }

        //写入数据
        public void AddValue(string key, object value)
        {
            Values.Add(key, value);
        }

        //获取数据
        public T GetValue<T>(string key)
        {
            if (Values.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default(T);
        }
    }

}
