using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimberValueEvaluationSystem.Services;

namespace TimberValueEvaluationSystem.ViewModels
{
    public class SExteriorPageViewModel:ViewModelBase
    {
        //封面流
        private CoverFlow coverFlowMain;
        public CoverFlow CoverFlowMain
        {
            get { return coverFlowMain; }
            set { Set(ref coverFlowMain, value); }
        }

        //搜索引擎
        private int searchEngine;
        public int SearchEngine
        {
            get { return searchEngine; }
            set { Set(ref searchEngine, value); }
        }

        //选中图层出现方式
        private int layerPopMode;
        public int LayerPopMode
        {
            get { return layerPopMode; }
            set { Set(ref layerPopMode, value); }
        }

        public RelayCommand LayerPopModeCommand { get; private set; }   //更改图层出现方式命令
        public RelayCommand AddStartImgCommand { get; private set; }    //添加启动图
        public RelayCommand SearchEngineChangedCommand { get; private set; }    //添加启动图

        public SExteriorPageViewModel()
        {
            LayerPopModeCommand = new RelayCommand(ExecuteLayerPopModeCommand);
            AddStartImgCommand = new RelayCommand(ExecuteAddStartImgCommand);
            SearchEngineChangedCommand = new RelayCommand(ExecuteSearchEngineChangedCommand);

        }

        //搜索引擎切换
        private void ExecuteSearchEngineChangedCommand()
        {
            Services.ConfigHelper.SetConfig("home_search_engine", ConverterHelper.SerachEngineConverter(SearchEngine));
            MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
        }

        //添加启动图
        private void ExecuteAddStartImgCommand()
        {
            // 源文件路径
            string sourceFilePath = FileHelper.GetFilePath("ImageFile(*.jpg,*.png)|*.jpg;*.png");
            if(sourceFilePath != null)
            {
                string sourceFileName = Path.GetFileName(sourceFilePath);
                // 目标文件路径
                string destinationFolderPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Resources", "Image", "StartPic"); ;
                // 创建目标文件路径
                string destinationFilePath = Path.Combine(destinationFolderPath, sourceFileName);
                // 尝试复制文件
                try
                {
                    // 如果目标文件已存在，将overwrite参数设置为true以覆盖
                    File.Copy(sourceFilePath, destinationFilePath, true);
                    //刷新封面流
                    Init();
                    MessageHelper.Success((string)Application.Current.Resources["StartImageCopySuccessfully"]);
                }
                catch (Exception ex)
                {
                    MessageHelper.Warning((string)Application.Current.Resources["StartImageCopyError"] + ex.Message);
                }
            }
        }

        //初始化配置读取
        public void Init()
        {
            //获取图层弹出方式
            LayerPopMode = ConverterHelper.Anti_LayerPopModeConverter(Services.ConfigHelper.GetConfig("layer_pop_mode"));

            //获取启动封面
            string folderPath = System.IO.Path.Combine(Environment.CurrentDirectory,
                "Resources", "Image", "StartPic");     // 指定文件夹路径
            string[] files = Directory.GetFiles(folderPath); // 获取文件夹中的所有文件
            foreach (var file in files)
            {
                CoverFlowMain.Add(file);
            }

            //读取搜索引擎设置
            SearchEngine = ConverterHelper.Anti_SerachEngineConverter(Services.ConfigHelper.GetConfig("home_search_engine"));
        }

        //修改图层出现方式
        private void ExecuteLayerPopModeCommand()
        {
            Services.ConfigHelper.SetConfig("layer_pop_mode", ConverterHelper.LayerPopModeConverter(LayerPopMode));
            MessageHelper.Success((string)Application.Current.Resources["SuccessfullyModified"]);
        }
    }
}
