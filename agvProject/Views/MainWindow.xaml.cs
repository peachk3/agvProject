using MahApps.Metro.Controls;

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
    }
}
