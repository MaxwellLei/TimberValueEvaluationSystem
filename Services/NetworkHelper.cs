using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    public static class NetworkHelper
    {
        //打开浏览器指定链接
        public static void OpenBrowser(string url)
        {
            //System.Diagnostics.Process.Start(url);
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
