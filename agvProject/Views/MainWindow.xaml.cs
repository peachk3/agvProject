using MahApps.Metro.Controls;
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
        }

        private void NewMission_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewMissionDialog();
            dialog.Owner = this;    // 메인창 위에 뜨게
            bool? result = dialog.ShowDialog();
            
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
