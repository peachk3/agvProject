using agvProject.Models;
using agvProject.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;

namespace agvProject.ViewModels
{
    // ViewModelBase 대신 ObservableObject를 상속받습니다.
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialog;

        public ObservableCollection<TabItemViewModel> TabItems { get; }

        private TabItemViewModel _selectedTabItem;
        public TabItemViewModel SelectedTabItem
        {
            get => _selectedTabItem;
            set => SetProperty(ref _selectedTabItem, value);
        }

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            dialog = dialogCoordinator;
            TabItems = new()
        {
            new TabItemViewModel { Header = "메인 모니터링", Content = new MainMonitoringViewModel(dialogCoordinator) },
            new TabItemViewModel { Header = "미션 관리",     Content = new MissionQueryViewModel() },
            new TabItemViewModel { Header = "이벤트 로그",   Content = new EventLogViewModel() }
        };
            SelectedTabItem = TabItems[0];
        }


    }
}