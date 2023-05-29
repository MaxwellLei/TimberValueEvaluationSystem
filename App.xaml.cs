using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            //渲染交给GPU提升性能
            RenderOptions.ProcessRenderMode = RenderMode.Default;
        }
    }
}
