using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    public class ConverterHelper
    {
        //搜索引擎转换器
        public static string SerachEngineConverter(int value)
        {
            switch (value)
            {
                case 0: return "BaiDu";
                case 1: return "Bing";
                case 2: return "Google";
                default: return null;
            }
        }
        //搜索引擎转换器
        public static int Anti_SerachEngineConverter(string value)
        {
            switch (value)
            {
                case "BaiDu": return 0;
                case "Bing": return 1;
                case "Google": return 2;
                default: return -1;
            }
        }
        //图层弹出方式转换器
        public static int Anti_LayerPopModeConverter(string value)
        {
            switch (value)
            {
                case "Push": return 0;
                case "Press": return 1;
                case "Cover": return 2;
                default: return -1;
            }
        }
        //图层弹出方式转换器
        public static string LayerPopModeConverter(int value)
        {
            switch (value)
            {
                case 0: return "Push";
                case 1: return "Press";
                case 2: return "Cover";
                default: return null;
            }
        }
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
