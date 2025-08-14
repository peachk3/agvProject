using System.Windows.Controls;
using agvProject.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace agvProject.Views
{
    /// <summary>
    /// MissionViewControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MissionViewControl : UserControl
    {
        public MissionViewControl()
        {
            InitializeComponent();
            
            // ViewModel을 DataContext로 설정
            this.DataContext = new MissionViewModel(DialogCoordinator.Instance);
        }
    }
}
