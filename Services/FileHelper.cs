using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using HandyControl.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

        //删除指定路径文件
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                Growl.Success("删除成功");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //检查文件是否被占用
        public static bool IsFileInUse(string path)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                fs.Close();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        //解除文件占用
        public static bool ReleaseFile(string path)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //检查文件是否存在
        public static bool IsFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
