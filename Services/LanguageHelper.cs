using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimberValueEvaluationSystem.Services
{
    public static class LanguageHelper
    {
        //初始化语言
        public static void InitializeLanguage()
        {
            string language = ConfigHelper.GetConfig("language");
            if (language != "")
            {
                LanguageHelper.ChangeLanguage(
                    ConverterHelper.LanguageConverter(Convert.ToInt32(language)));
            }
            else
            {
                LanguageHelper.ChangeLanguage("zh-CN");
            }
            
        }
        //切换语言
        public static void ChangeLanguage(string language)
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
            string requestedCulture = @"/TimberValueEvaluationSystem;component/Resources/Language/" +  language + ".xaml";
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedCulture));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

    }
}
