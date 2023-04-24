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
        public static string GetFilePath(string filter, string initialDir = null)
        {
            string shpFilePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == true)
            {
                shpFilePath = openFileDialog.FileName;
            }
            return shpFilePath;
        }
    }
}
