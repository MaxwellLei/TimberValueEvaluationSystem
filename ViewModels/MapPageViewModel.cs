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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.ViewsPopUp.DataPage;

namespace TimberValueEvaluationSystem.ViewModels
{
    //地图页的 ViewModel
    class MapPageViewModel : ViewModelBase
    {
        //鼠标的x,y位置
        private double position_x = 0;
        private double position_y = 0;

        //shp文件路径
        private string _shpFilePath;
        public string ShpFilePath
        {
            get { return _shpFilePath; }
            set { Set(ref _shpFilePath, value); }
        }

        //图层侧边栏出现方式
        private string _drawerMode;
        public string DrawerMode
        {
            get { return _drawerMode; }
            set { Set(ref _drawerMode, value); }
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

        //鼠标位置坐标
        private string _mousePosition;
        public string MousePosition
        {
            get { return _mousePosition; }
            set { Set(ref _mousePosition, value); }
        }

        //比例尺数据
        private string _zoomScale;
        public string ZoomScale
        {
            get { return _zoomScale; }
            set { Set(ref _zoomScale, value); }
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
        public RelayCommand<GeoViewInputEventArgs> ClickSurfaceCommand { get; private set; }   //点击面命令
        public RelayCommand<MouseEventArgs> GetMapInfoCommand { get; private set; }   //移动鼠标命令
        public RelayCommand WheelChangedCommand { get; private set; }   //鼠标滚轮命令
        public RelayCommand EditSurfaceCommand { get; private set; }   //编辑绘制点线面命令
        public RelayCommand TestCommand { get; private set; }   //测试命令


        public MapPageViewModel()
        {
            // 创建一个世界地图类型的地图
            Map = new Map(Basemap.CreateStreets());
            //初始化命令
            LayerSidebarCommand = new RelayCommand(ExecuteLayerSidebarCommand);
            SelectShpFileCommand = new RelayCommand(ExecuteSelectShpFileCommand);
            ScreenshotCommand = new RelayCommand(ExecuteScreenshotCommand);
            FocusLayersCommand = new RelayCommand(ExecuteFocusLayersCommand);
            ClickSurfaceCommand = new RelayCommand<GeoViewInputEventArgs>(MyMapView_GeoViewTapped);
            GetMapInfoCommand = new RelayCommand<MouseEventArgs>(GetInfoCommand);
            WheelChangedCommand = new RelayCommand(ExecuteWheelChangedCommand);
            EditSurfaceCommand = new RelayCommand(ExecuteEditSurfaceCommand);
            TestCommand = new RelayCommand(ExecuteTestCommand);
        }
        //测试
        private void ExecuteTestCommand()
        {
            var forestValueView = new ForestValueView();
            Dialog.Show(forestValueView);
        }

        //鼠标滚轮命令
        private void ExecuteWheelChangedCommand()
        {
            //获取缩放倍数
            ZoomScale = (string)Application.Current.Resources["Scale"] + "1" + ":" + Math.Floor(MyMapView.MapScale);
        }

        //获取前端的信息
        private void GetInfoCommand(MouseEventArgs e)
        {
            position_x = e.GetPosition(MyMapView).X;
            position_y = e.GetPosition(MyMapView).Y;

            //获取鼠标点击的地图坐标
            MapPoint mousePosition = MyMapView.ScreenToLocation(new Point(position_x, position_y));
            if(mousePosition != null)
            {
                //改变数据通知前端
                MousePosition = (string)Application.Current.Resources["Coordinate"] + Math.Floor(mousePosition.X) + " m" + "," + Math.Floor(mousePosition.Y) + " m";
            }

            //SpatialReference spatialRef = MyMapView.Map.SpatialReference;
            //var wkid = spatialRef.Wkid; // 获取地图使用的空间参考系的 WKID
            //var wkt = spatialRef.WkText; // 获取地图使用的空间参考系的 WKT
            //var unit = spatialRef.Unit; // 获取地图使用的空间参考系的单位

        }

        //编辑绘制点线面命令
        private void ExecuteEditSurfaceCommand()
        {
            //编辑绘制点线面
            Growl.Info("编辑绘制点线面");
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
            if(ShpFilePath != null)
            {
                ReadShpFileAsync(ShpFilePath);  //打开Shp文件
                MapLayers = new ObservableCollection<Layer>(Map.OperationalLayers);     //更新图层数据
                //ExecuteWheelChangedCommand();   //更新比例尺
                MessageHelper.Success((string)Application.Current.Resources["FileReadSuccessfully"]);
            }
        }

        //高亮显示面
        private async void MyMapView_GeoViewTapped(GeoViewInputEventArgs e)
        {
            //如果有图层，才可以点击
            if (MapLayers.Count != 0)
            {
                // 获取点击位置的地图坐标
                MapPoint mapPoint = e.Location;

                // 设置查询参数
                double tolerance = 10; // 选择容差
                int maxResults = 1; // 最大选择结果数

                // 执行识别操作
                IReadOnlyList<IdentifyLayerResult> identifyResults =
                     await MyMapView.IdentifyLayersAsync(new Point(e.Position.X, e.Position.Y),
                                                         tolerance,
                                                         false,
                                                         maxResults,
                                                         CancellationToken.None);

                // 遍历识别结果
                foreach (IdentifyLayerResult layerResult in identifyResults)
                {
                    // 高亮选中的要素
                    FeatureLayer featureLayer = layerResult.LayerContent as FeatureLayer;
                    if (featureLayer != null)
                    {
                        featureLayer.ClearSelection();
                        foreach (GeoElement geoElement in layerResult.GeoElements)
                        {
                            Feature feature = geoElement as Feature;
                            if (feature != null)
                            {
                                featureLayer.SelectFeature(feature);
                            }
                        }
                    }
                }
            }
        }

        //异步读取shp文件
        private async Task ReadShpFileAsync(string shapefilePath)
        {
            // 创建一个ShapefileFeatureTable，用于读取SHP文件
            ShapefileFeatureTable shapefileFeatureTable = new ShapefileFeatureTable(shapefilePath);

            // 加载ShapefileFeatureTable
            await shapefileFeatureTable.LoadAsync();

            // 创建一个FeatureLayer，用于显示SHP文件中的要素
            FeatureLayer featureLayer = new FeatureLayer(shapefileFeatureTable);

            // 将FeatureLayer添加到地图的操作图层中
            MyMapView.Map.OperationalLayers.Add(featureLayer);

            // 确保地图显示SHP文件的范围
            await MyMapView.SetViewpointGeometryAsync(featureLayer.FullExtent);
        }

        //弹出图层侧边栏
        private void ExecuteLayerSidebarCommand()
        {
            DrawerMode = ConfigHelper.GetConfig("layer_pop_mode");
            IsOpenSidebar = true;
        }
    }
}
