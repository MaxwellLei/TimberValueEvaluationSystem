using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace TimberValueEvaluationSystem.Services
{
    public static class BootHelper
    {
        //开机自动启动
        public static void SetAutoRun(bool isAutoRun)
        {
            try
            {
                string name = "TimberValueEvaluationSystem";
                Microsoft.Win32.RegistryKey reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                {
                    reg.SetValue(name, System.Windows.Forms.Application.ExecutablePath);
                }
                else
                {
                    reg.DeleteValue(name);
                }
                reg.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
