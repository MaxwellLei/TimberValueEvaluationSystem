using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using HandyControl.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    public class FileHelper
    {
        //获取文件夹路径
        public static string GetFolderPath()
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;     //用户正确选择了路径
            }
            else
            {
                return null;    //用户直接关闭了窗口
            }
        }

        //获取文件路径——不带格式限制
        public static string GetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;     //用户正确选择了路径
            }
            else
            {
                return null;    //用户直接关闭了窗口
            }
        }

        //获取文件路径——带有格式限制
        public static string GetFilePath(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;     //用户正确选择了路径
            }
            else
            {
                return null;    //用户直接关闭了窗口
            }
        }

        //打开文件所在路径
        public static bool Openxplorer(string path)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
