
using System.Windows;
using MahApps.Metro.Controls;

namespace agvProject.Views
{
    /// <summary>
    /// NewMissionDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewMissionDialog : MetroWindow
    {
        public string name => NameBox.Text?.Trim();
        public string num => NumBox.Text?.Trim();

        public NewMissionDialog()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }


    }
}
