using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using LiveChartsCore.Geo;
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
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using TimberValueEvaluationSystem.Services;
using TimberValueEvaluationSystem.ViewsPopUp.DataPage;
using System.IO;
using System.Printing;
using System.Windows.Controls;

namespace TimberValueEvaluationSystem.ViewModels
{
    //地图页的 ViewModel
    class MapPageViewModel : ViewModelBase
    {
        //鼠标的x,y位置
        private double position_x = 0;
        private double position_y = 0;

        //选中的图层
        private int _selectedLayer;
        public int SelectedLayer
        {
            get { return _selectedLayer; }
            set { Set(ref _selectedLayer, value); ExecuteTestCommand(); }
        }

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
            set {Set(ref _map, value);}
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
        private ObservableCollection<Layer> _mapLayers;
        public ObservableCollection<Layer> MapLayers
        {
            get { return _mapLayers; }
            set { Set(() => MapLayers, ref _mapLayers, value); }
        }

        //面数据
        private ObservableCollection<string> _featureAttributes = new ObservableCollection<string>();
        public ObservableCollection<string> FeatureAttributes
        {
            get { return _featureAttributes; }
            set { Set(ref _featureAttributes, value); }
        }


        public RelayCommand LayerSidebarCommand { get; private set; }   //弹出图层侧边栏
        public RelayCommand SelectShpFileCommand { get; private set; }   //导入Shp文件命令
        public RelayCommand ScreenshotCommand { get; private set; }   //截图命令
        public RelayCommand FocusLayersCommand { get; private set; }   //聚焦图层命令
        public RelayCommand<GeoViewInputEventArgs> ClickSurfaceCommand { get; private set; }   //点击面命令
        public RelayCommand<MouseEventArgs> GetMapInfoCommand { get; private set; }   //移动鼠标命令
        public RelayCommand WheelChangedCommand { get; private set; }   //鼠标滚轮命令
        public RelayCommand EditSurfaceCommand { get; private set; }   //编辑绘制点线面命令
        public RelayCommand PrintCommand { get; private set; }   //打印命令
        public RelayCommand FocusSelectedLayerCommand { get; private set; }   //聚焦图层命令
        public RelayCommand UnloadLayerCommand { get; private set; }   //卸载图层命令
        public RelayCommand MoveLayerUpCommand { get; private set; }   //上移图层命令
        public RelayCommand MoveLayerDownCommand { get;private set; }  //下移图层命令
        public RelayCommand LayerOnTopCommand { get; private set; }  //置顶图层命令
        public RelayCommand LayerBottomCommand { get; private set; }  //置顶图层命令
        public RelayCommand ClearLayerCommand { get; private set; }   //清空图层命令
        public RelayCommand TestCommand { get; private set; }   //测试命令


        public MapPageViewModel()
        {
            // 创建一个世界地图类型的地图
            Map = new Map(Basemap.CreateStreets());
            //初始化命令
            LayerSidebarCommand = new RelayCommand(ExecuteLayerSidebarCommand);
            SelectShpFileCommand = new RelayCommand(ExecuteSelectShpFileCommand);
            ScreenshotCommand = new RelayCommand(ExecuteScreenshotCommand);
            FocusLayersCommand = new RelayCommand(ExecuteFocusLayersCommandAsync);
            ClickSurfaceCommand = new RelayCommand<GeoViewInputEventArgs>(MyMapView_GeoViewTapped);
            GetMapInfoCommand = new RelayCommand<MouseEventArgs>(GetInfoCommand);
            WheelChangedCommand = new RelayCommand(ExecuteWheelChangedCommand);
            PrintCommand = new RelayCommand(ExecutePrintCommand);
            EditSurfaceCommand = new RelayCommand(ExecuteEditSurfaceCommand);
            FocusSelectedLayerCommand = new RelayCommand(ExecuteFocusSelectedLayerCommand);
            UnloadLayerCommand = new RelayCommand(ExecuteUnloadLayerCommand);
            MoveLayerUpCommand = new RelayCommand(ExecuteMoveLayerUpCommand);
            MoveLayerDownCommand = new RelayCommand(ExecuteMoveLayerDownCommand);
            LayerOnTopCommand = new RelayCommand(ExecuteLayerOnTopCommand);
            LayerBottomCommand = new RelayCommand(ExecuteLayerBottomCommand);
            ClearLayerCommand = new RelayCommand(ExecuteClearLayerCommand);
            TestCommand = new RelayCommand(ExecuteTestCommand);
        }
        //测试
        private void ExecuteTestCommand()
        {
            MessageHelper.Info($"选中的：{SelectedLayer}");
        }

        //打印命令
        private void ExecutePrintCommand()
        {
            // 导出地图视图为图像
            RenderTargetBitmap mapViewImage = ExportMapViewImage(MyMapView);

            if (mapViewImage != null)
            {
                // 转换成PNG图像
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(mapViewImage));
                MemoryStream imageStream = new MemoryStream();
                encoder.Save(imageStream);

                // 使用System.Windows.Controls.Image显示图像
                System.Windows.Controls.Image printImage = new System.Windows.Controls.Image
                {
                    Source = BitmapFrame.Create(imageStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad),
                    Stretch = Stretch.None
                };

                // 使用PrintDialog打印图像
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // 使用PrintTicket调整打印参数，例如纸张大小、方向等
                    printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                    printDialog.PrintVisual(printImage, "打印MapView");
                }
                MessageHelper.Success(MessageHelper.GetString("PrintedSuccessfully"));
            }
        }

        //图像生成
        private RenderTargetBitmap ExportMapViewImage(MapView mapView)
        {
            if (mapView == null)
            {
                return null;
            }
            int width = (int)mapView.ActualWidth;
            int height = (int)mapView.ActualHeight;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(mapView);
                context.DrawRectangle(visualBrush, null, new Rect(new Point(), new Size(width, height)));
            }
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }


        //聚焦到选定图层
        private void ExecuteFocusSelectedLayerCommand()
        {
            if (SelectedLayer != -1)
            {
                //获取图层
                Layer layer = MapLayers[SelectedLayer];
                FocusOnLayer(layer);
            }
        }

        //置顶图层
        private void ExecuteLayerOnTopCommand()
        {
            if (SelectedLayer != -1)
            {
                //获取图层
                Layer layer = MapLayers[SelectedLayer];
                // 移除所选图层
                MapLayers.Remove(layer);
                // 将图层插入到集合的开头
                MapLayers.Insert(MapLayers.Count, layer);
            }
        }

        //图层置底
        private void ExecuteLayerBottomCommand()
        {
            if (SelectedLayer != -1)
            {
                //获取图层
                Layer layer = MapLayers[SelectedLayer];
                // 移除所选图层
                MapLayers.Remove(layer);
                // 将图层插入到集合的开头
                MapLayers.Insert(0, layer);
            }
        }

        //下移图层
        private void ExecuteMoveLayerDownCommand()
        {
            //移动图层
            MoveLayer(false);
        }

        //上移图层
        private void ExecuteMoveLayerUpCommand()
        {
            //移动图层
            MoveLayer(true);
        }

        //移动图层
        public void MoveLayer(bool moveUp)
        {
            var countNum = MapLayers.Count;
            int selectedLayerIndex = (SelectedLayer - 1 + countNum) % countNum;

            if (selectedLayerIndex >= 0)
            {
                int newIndex = moveUp ? selectedLayerIndex - 1 : selectedLayerIndex + 1;
                if (newIndex >= 0 && newIndex < MapLayers.Count)
                {
                    //在 MapLayers 调整顺序
                    var temp = SelectedLayer;
                    Layer layer = MapLayers[temp];
                    MapLayers.RemoveAt(temp);
                    int tempNewIndex;
                    if (temp == countNum - 1)
                    {
                        tempNewIndex = moveUp ? temp : temp - 1;
                    }else if(temp == 0)
                    {
                        tempNewIndex = moveUp ? temp + 1 : temp;
                    }
                    else
                    {
                        tempNewIndex = moveUp ? temp + 1 : temp - 1;
                    }
                    MapLayers.Insert(tempNewIndex, layer);
                }
            }
        }



        //清空图层
        private void ExecuteClearLayerCommand()
        {
            // 访问地图中的图层集合
            LayerCollection mapLayers = MyMapView.Map.OperationalLayers;
            // 清空图层集合
            mapLayers.Clear();
            //清空图层列表
            MapLayers.Clear();
        }

        //卸载图层
        private void ExecuteUnloadLayerCommand()
        {
            // 确保地图有图层
            if (Map.OperationalLayers.Count > 0)
            {
                //获取点击图层
                Layer selectLayer = MyMapView.Map.OperationalLayers[SelectedLayer];
                // 从地图的OperationalLayers集合中移除图层
                Map.OperationalLayers.Remove(selectLayer);
                //从图层列表除图层
                MapLayers.Remove(selectLayer);
            }
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
        private void ExecuteFocusLayersCommandAsync()
        {
            if (Map.OperationalLayers.Count > 0)
            {
                Layer layer;
                if (SelectedLayer == -1)
                {
                    layer = MapLayers[MapLayers.Count];
                }
                else
                {
                    layer = MapLayers[SelectedLayer];
                }
                FocusOnLayer(layer);
            }
        }

        //聚焦图层
        public async void FocusOnLayer(Layer layer)
        {
            // 获取图层的范围（Extent）
            Envelope layerExtent = layer.FullExtent;
            if (layerExtent != null)
            {
                // 使用图层的范围创建一个新的 Viewpoint
                Viewpoint layerViewpoint = new Viewpoint(layerExtent);

                // 将地图视图的 Viewpoint 设置为新的 Viewpoint
                await MyMapView.SetViewpointAsync(layerViewpoint);
            }
        }

        //截图命令
        private async void ExecuteScreenshotCommand()
        {
            var path = FileHelper.GetFilePath();
            await SaveMapViewToImageAsync(MyMapView,path);
        }

        //截图
        public async Task SaveMapViewToImageAsync(MapView mapView, string outputPath)
        {
            if (mapView == null || string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            int width = (int)mapView.ActualWidth;
            int height = (int)mapView.ActualHeight;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(mapView);
                context.DrawRectangle(visualBrush, null, new Rect(new Point(), new Size(width, height)));
            }

            renderTarget.Render(drawingVisual);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (FileStream file = File.Create(outputPath))
            {
                encoder.Save(file);
            }
        }

        //导入Shp文件
        private async void ExecuteSelectShpFileCommand()
        {
            ShpFilePath = FileHelper.GetFilePath("Shapefiles(*.shp) | *.shp");  //读取文件路径
            if(ShpFilePath != null)
            {
                await ReadShpFileAsync(ShpFilePath);  //打开Shp文件
                MapLayers = new ObservableCollection<Layer>(Map.OperationalLayers);     //更新图层数据
                //ExecuteWheelChangedCommand();   //更新比例尺
                MessageHelper.Success(MessageHelper.GetString("FileReadSuccessfully"));
            }
        }

        //高亮显示面
        private async void MyMapView_GeoViewTapped(GeoViewInputEventArgs e)
        {
            FeatureAttributes.Clear();
            //如果有图层，才可以点击
            if (MapLayers !=null && MapLayers.Count != 0)
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
                // 初始化要高亮的要素和图层
                FeatureLayer highestFeatureLayer = null;
                Feature highestFeature = null;

                // 遍历识别结果，找到最高图层的要素和图层
                foreach (IdentifyLayerResult layerResult in identifyResults)
                {
                    FeatureLayer featureLayer = layerResult.LayerContent as FeatureLayer;
                    if (featureLayer != null)
                    {
                        foreach (GeoElement geoElement in layerResult.GeoElements)
                        {
                            Feature feature = geoElement as Feature;
                            if (feature != null)
                            {
                                // 如果找到的要素的图层比上一个找到的要素的图层更靠上，更新最高图层的要素和图层
                                if (highestFeatureLayer == null || MapLayers.IndexOf(featureLayer) > MapLayers.IndexOf(highestFeatureLayer))
                                {
                                    highestFeatureLayer = featureLayer;
                                    highestFeature = feature;
                                }
                            }
                        }
                    }
                }

                // 清除所有图层的选择
                foreach (FeatureLayer layer in MapLayers)
                {
                    layer.ClearSelection();
                }

                // 如果找到最高图层的要素，将其设为选中状态
                if (highestFeatureLayer != null && highestFeature != null)
                {
                    highestFeatureLayer.SelectFeature(highestFeature);

                    // 获取并处理属性数据（DBF数据）
                    var attributes = highestFeature.Attributes;
                    foreach (var attribute in attributes)
                    {
                        //MessageHelper.Info($"Key: {attribute.Key}, Value: {attribute.Value}");
                        FeatureAttributes.Add($"{attribute.Key}: {attribute.Value}");
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
