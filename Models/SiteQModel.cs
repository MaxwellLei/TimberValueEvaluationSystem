using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Models
{
    public class SiteQModel
    {
        public long Index { get; set; }   //ID
        public string SoilThickness { get; set; }   //土壤厚度
        public string Slope { get; set; }   //坡位
        public string Aspect { get; set; }      //坡向
        public string Gradient { get; set; }    //坡度
        public string? SiteQuality { get; set; }    //立地质量
    }
}
