using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TimberValueEvaluationSystem.Views;

namespace TimberValueEvaluationSystem.ViewModels
{
    class SettingPageViewModel
    {
        private Frame Nav;  //导航对象
        public RelayCommand CommonPage { get; private set; }   //切换常规设置命令

        public SettingPageViewModel(Frame nav)
        {
            Nav = nav;
            CommonPage = new RelayCommand(ExecuteCommonPage);
            Nav.Navigate(SCommonPageView.GetPage());
        }

        //切换常规设置命令
        private void ExecuteCommonPage()
        {
            Nav.Navigate(SCommonPageView.GetPage());
        }
    }
}
