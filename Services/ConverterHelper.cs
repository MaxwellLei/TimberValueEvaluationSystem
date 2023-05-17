using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    public class ConverterHelper
    {
        //语言转换器
        public static string LanguageConverter(int value)
        {
            switch (value)
            {
                case 0: return "zh-CN";
                case 1: return "en-US";
                default: return null;
            }
        }
        //立地质量转换器
        public static string SiteQualityModelConverter(long value)
        {
            switch (value)
            {
                case 0: return "立地质量—差";
                case 1: return "立地质量—中";
                case 2: return "立地质量—好";
                default: return null;
            }
        }
    }
}
