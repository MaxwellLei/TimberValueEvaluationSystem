using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.ViewModels
{
    //地图页的 ViewModel
    class MapPageViewModel : ViewModelBase
    {
        private Map _map;

        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                RaisePropertyChanged(() => Map); // 使用ViewModelBase类提供的属性更改通知
            }
        }

        public MapPageViewModel()
        {
            // 创建一个世界地图类型的地图
            Map = new Map(Basemap.CreateStreets());

        }
    }
}
