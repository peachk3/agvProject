using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace agvProject.ViewModels
{
    // ViewModelBase 대신 ObservableObject를 상속받습니다.
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<TabItemViewModel> TabItems { get; }
        private TabItemViewModel _selectedTabItem;
        public TabItemViewModel SelectedTabItem
        {
            get => _selectedTabItem;
            set => SetProperty(ref _selectedTabItem, value);
        }

        public MainViewModel()
        {
            TabItems = new()
        {
            new TabItemViewModel { Header = "메인 모니터링", Content = new MainMonitoringViewModel() },
            new TabItemViewModel { Header = "미션 관리",     Content = new MissionQueryViewModel() },
            new TabItemViewModel { Header = "이벤트 로그",   Content = new EventLogViewModel() }
        };
            SelectedTabItem = TabItems[0];
        }
    }
}