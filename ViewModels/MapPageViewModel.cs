using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    //地图页的 ViewModel
    class MapPageViewModel : ViewModelBase
    {
        //shp文件路径
        private string _shpFilePath;
        public string ShpFilePath
        {
            get { return _shpFilePath; }
            set { Set(ref _shpFilePath, value); }
        }

        //侧边栏双向数据绑定
        private bool isOpenSidebar;
        public bool IsOpenSidebar
        {
            get { return isOpenSidebar; }
            set { Set(ref isOpenSidebar, value); }
        }

        //Gis图层
        private Map _map;
        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                RaisePropertyChanged(() => Map);
            }
        }

        public RelayCommand LayerSidebarCommand { get; private set; }   //弹出图层侧边栏
        public RelayCommand SelectShpFileCommand { get; private set; }   //导入Shp文件命令
        
        public MapPageViewModel()
        {
            // 创建一个世界地图类型的地图
            Map = new Map(Basemap.CreateStreets());
            //初始化命令
            LayerSidebarCommand = new RelayCommand(ExecuteLayerSidebarCommand);
            SelectShpFileCommand = new RelayCommand(ExecuteSelectShpFileCommand);
        }

        //导入Shp文件
        private void ExecuteSelectShpFileCommand()
        {
            string filePath = FileHelper.GetFilePath("Shapefile (*.shp)|*.shp");
            if (!string.IsNullOrEmpty(filePath))
            {
                ShpFilePath = filePath;
            }

        }

        //弹出图层侧边栏
        private void ExecuteLayerSidebarCommand()
        {
            IsOpenSidebar = true;
        }
    }
}
