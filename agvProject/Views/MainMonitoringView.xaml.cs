using agvProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace agvProject.Views
{
    /// <summary>
    /// MainMonitoringView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainMonitoringView : UserControl
    {
        public MainMonitoringView()
        {
            InitializeComponent();
        }

        //private void NewMission_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new NewMissionDialog();
        //    dialog.Owner = Window.GetWindow(this);      // UserControl에선 바로 Owner = this 못씀
        //    dialog.ShowDialog();                   
        //}
    }
}