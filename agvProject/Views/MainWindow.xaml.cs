using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace agvProject.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new agvProject.ViewModels.MainViewModel(DialogCoordinator.Instance);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // DB 연결
            // 데이터 그리드에 데이터를 읽어오기
            LoadedControlFromDb();
        }

        private void LoadedControlFromDb()
        {
            string conntionString = "=localhost;Initial Catalog=agvProject;Integrated Security=True";
        }
    }
}
