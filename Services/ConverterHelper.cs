using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    public class ConverterHelper
    {
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
