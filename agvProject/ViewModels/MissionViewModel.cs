using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using agvProject.Models;
using MahApps.Metro.Controls.Dialogs;

namespace agvProject.ViewModels
{
    public class MissionViewModel : INotifyPropertyChanged
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Mission> _missions;
        private string _selectedAgv;
        private string _selectedRobotArm;
        private DateTime _selectedDate;
        private string _selectedMissionLocation;
        private string _searchText;

        public MissionViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            _selectedDate = DateTime.Today;
            
            // 명령어 초기화
            SearchCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
            AddMissionCommand = new RelayCommand(ExecuteAddMission);
            CancelMissionCommand = new RelayCommand(ExecuteCancelMission, CanExecuteCancelMission);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            SelectedAllCommand = new RelayCommand(ExecuteSelectedAll);
            UnSelectedAllCommand = new RelayCommand(ExecuteUnSelectedAll);
            ExportCommand = new RelayCommand(ExecuteExport, CanExecuteExport);

            // 초기 데이터 로드
            LoadInitialData();
        }

        #region Properties

        public ObservableCollection<Mission> Missions
        {
            get => _missions;
            set
            {
                _missions = value;
                OnPropertyChanged(nameof(Missions));
            }
        }

        public string SelectedAgv
        {
            get => _selectedAgv;
            set
            {
                _selectedAgv = value;
                OnPropertyChanged(nameof(SelectedAgv));
            }
        }

        public string SelectedRobotArm
        {
            get => _selectedRobotArm;
            set
            {
                _selectedRobotArm = value;
                OnPropertyChanged(nameof(SelectedRobotArm));
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public string SelectedMissionLocation
        {
            get => _selectedMissionLocation;
            set
            {
                _selectedMissionLocation = value;
                OnPropertyChanged(nameof(SelectedMissionLocation));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        #endregion

        #region Commands

        public ICommand SearchCommand { get; }
        public ICommand AddMissionCommand { get; }
        public ICommand CancelMissionCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SelectedAllCommand { get; }
        public ICommand UnSelectedAllCommand { get; }
        public ICommand ExportCommand { get; }

        #endregion

        #region Command Methods

        private void ExecuteSearch()
        {
            // 검색 로직 구현
            LoadMissions();
        }

        private bool CanExecuteSearch()
        {
            return !string.IsNullOrEmpty(SearchText) || 
                   !string.IsNullOrEmpty(SelectedAgv) || 
                   !string.IsNullOrEmpty(SelectedRobotArm);
        }

        private async void ExecuteAddMission()
        {
            try
            {
                // 미션 추가 다이얼로그 표시
                var result = await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "미션 추가", 
                    "새로운 미션을 추가하시겠습니까?", 
                    MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    // 미션 추가 로직 구현
                    await _dialogCoordinator.ShowMessageAsync(
                        this, 
                        "성공", 
                        "미션이 추가되었습니다.");
                    
                    LoadMissions(); // 목록 새로고침
                }
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "오류", 
                    $"미션 추가 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private async void ExecuteCancelMission()
        {
            try
            {
                var selectedMissions = Missions.Where(m => m.IsSelected).ToList();
                
                if (!selectedMissions.Any())
                {
                    await _dialogCoordinator.ShowMessageAsync(
                        this, 
                        "알림", 
                        "취소할 미션을 선택해주세요.");
                    return;
                }

                var result = await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "미션 취소", 
                    $"선택된 {selectedMissions.Count}개의 미션을 취소하시겠습니까?", 
                    MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    // 미션 취소 로직 구현
                    foreach (var mission in selectedMissions)
                    {
                        mission.Status = MissionStatus.Canceled;
                    }
                    
                    await _dialogCoordinator.ShowMessageAsync(
                        this, 
                        "성공", 
                        "선택된 미션이 취소되었습니다.");
                }
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "오류", 
                    $"미션 취소 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private bool CanExecuteCancelMission()
        {
            return Missions?.Any(m => m.IsSelected) == true;
        }

        private void ExecuteRefresh()
        {
            LoadMissions();
        }

        private void ExecuteSelectedAll()
        {
            if (Missions != null)
            {
                foreach (var mission in Missions)
                {
                    mission.IsSelected = true;
                }
            }
        }

        private void ExecuteUnSelectedAll()
        {
            if (Missions != null)
            {
                foreach (var mission in Missions)
                {
                    mission.IsSelected = false;
                }
            }
        }

        private async void ExecuteExport()
        {
            try
            {
                var selectedMissions = Missions?.Where(m => m.IsSelected).ToList();
                
                if (selectedMissions == null || !selectedMissions.Any())
                {
                    await _dialogCoordinator.ShowMessageAsync(
                        this, 
                        "알림", 
                        "내보낼 미션을 선택해주세요.");
                    return;
                }

                // 내보내기 로직 구현
                await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "성공", 
                    $"선택된 {selectedMissions.Count}개의 미션이 내보내기되었습니다.");
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    "오류", 
                    $"내보내기 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private bool CanExecuteExport()
        {
            return Missions?.Any(m => m.IsSelected) == true;
        }

        #endregion

        #region Private Methods

        private void LoadInitialData()
        {
            // 샘플 데이터 로드
            Missions = new ObservableCollection<Mission>
            {
                new Mission
                {
                    MissionId = "M001",
                    AddMissionTime = DateTime.Now.AddHours(-2),
                    ProcessName = "AGV-01",
                    StartLocation = "충전소",
                    Total = 0,
                    MissionStartTime = DateTime.Now.AddHours(-1),
                    FinalLocation = "토출구",
                    Status = MissionStatus.Processing,
                    Duration = TimeSpan.FromMinutes(45),
                    AddInfo = "충전소에서 AGV-01가 토출구로 이동중"
                },
                new Mission
                {
                    MissionId = "M002",
                    AddMissionTime = DateTime.Now.AddHours(-3),
                    ProcessName = "AGV-01",
                    StartLocation = "토출구",
                    Total = 100,
                    FinalLocation = "분류장",
                    Status = MissionStatus.Standby,
                    AddInfo = "AGV-01가 토출구에서 분류장으로 이동"
                },
                new Mission
                {
                    MissionId = "M003",
                    AddMissionTime = DateTime.Now.AddHours(-4),
                    ProcessName = "AGV-01",
                    StartLocation = "분류장",
                    Total = 75,
                    MissionStartTime = DateTime.Now.AddHours(-3),
                    MissionEndTime = DateTime.Now.AddHours(-2),
                    FinalLocation = "토출구",
                    Status = MissionStatus.Standby,
                    Duration = TimeSpan.FromMinutes(60),
                    AddInfo = "AGV-01가 분류장에서 토출구로 이동"
                }
            };
        }

        private void LoadMissions()
        {
            // 실제 데이터베이스나 서비스에서 미션 데이터를 로드하는 로직
            // 현재는 샘플 데이터를 사용
            LoadInitialData();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
