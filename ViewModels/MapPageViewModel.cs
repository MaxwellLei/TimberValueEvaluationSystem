using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        //Gis图地图模型
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

        //地图视图
        private MapView _myMapView;
        public MapView MyMapView
        {
            get { return _myMapView; }
            set { _myMapView = value; }
        }

        //图层数据
        private ObservableCollection<Layer> _mapLayers = new ObservableCollection<Layer>();
        public ObservableCollection<Layer> MapLayers
        {
            get { return _mapLayers; }
            set { Set(() => MapLayers, ref _mapLayers, value); }
        }


        public RelayCommand LayerSidebarCommand { get; private set; }   //弹出图层侧边栏
        public RelayCommand SelectShpFileCommand { get; private set; }   //导入Shp文件命令
        public RelayCommand ScreenshotCommand { get; private set; }   //截图命令
        public RelayCommand FocusLayersCommand { get; private set; }   //聚焦图层命令
        public RelayCommand ClickSurfaceCommand { get; private set; }   //点击面命令



        public MapPageViewModel()
        {
            // 创建一个世界地图类型的地图
            Map = new Map(Basemap.CreateStreets());
            //初始化命令
            LayerSidebarCommand = new RelayCommand(ExecuteLayerSidebarCommand);
            SelectShpFileCommand = new RelayCommand(ExecuteSelectShpFileCommand);
            ScreenshotCommand = new RelayCommand(ExecuteScreenshotCommand);
            FocusLayersCommand = new RelayCommand(ExecuteFocusLayersCommand);
            ClickSurfaceCommand = new RelayCommand(ExecuteClickSurfaceCommand);

        }

        //点击面
        private void ExecuteClickSurfaceCommand()
        {
            Growl.Info("点击成功");
        }

        //聚焦图层
        private void ExecuteFocusLayersCommand()
        {
            //聚焦ArcGis图层
            Growl.Error("这是一个错误提醒");
        }

        //截图命令
        private void ExecuteScreenshotCommand()
        {
            new Screenshot().Start();

        }

        //导入Shp文件
        private void ExecuteSelectShpFileCommand()
        {
            ShpFilePath = FileHelper.GetFilePath("Shapefiles(*.shp) | *.shp");  //读取文件路径
            ReadShpFileAsync(ShpFilePath);  //打开Shp文件
            MapLayers = new ObservableCollection<Layer>(Map.OperationalLayers);     //更新图层数据
            Growl.Success("文件读取成功");
        }



        //异步读取shp文件
        private async Task ReadShpFileAsync(string shapefilePath)
        {
            // Create the feature table
            ShapefileFeatureTable myFeatureTable = await ShapefileFeatureTable.OpenAsync(shapefilePath);

            // Create the layer from the feature table
            FeatureLayer myFeatureLayer = new FeatureLayer(myFeatureTable);

            // Add the layer to the map
            Map.OperationalLayers.Add(myFeatureLayer);

            await MyMapView.SetViewpointAsync(new Viewpoint(myFeatureTable.Extent));
        }

        //弹出图层侧边栏
        private void ExecuteLayerSidebarCommand()
        {
            IsOpenSidebar = true;
        }
    }
}
