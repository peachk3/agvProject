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
            ExportCommand = new RelayCommand(ExecuteExport, CanExecuteExport);
            ToggleSelectAllCommand = new RelayCommand(ExecuteToggleSelectAll);

            LoadInitialData();
        }

        #region Properties

        public ObservableCollection<Mission> Missions
        {
            get => _missions;
            set
            {
                if (_missions != null)
                    _missions.CollectionChanged -= Missions_CollectionChanged;

                _missions = value;

                if (_missions != null)
                    _missions.CollectionChanged += Missions_CollectionChanged;

                OnPropertyChanged(nameof(Missions));
            }
        }

        public string SelectedAgv
        {
            get => _selectedAgv;
            set { _selectedAgv = value; OnPropertyChanged(nameof(SelectedAgv)); }
        }

        public string SelectedRobotArm
        {
            get => _selectedRobotArm;
            set { _selectedRobotArm = value; OnPropertyChanged(nameof(SelectedRobotArm)); }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        public string SelectedMissionLocation
        {
            get => _selectedMissionLocation;
            set { _selectedMissionLocation = value; OnPropertyChanged(nameof(SelectedMissionLocation)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }



        #endregion

        #region Commands

        public ICommand SearchCommand { get; }
        public ICommand AddMissionCommand { get; }
        public ICommand CancelMissionCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ToggleSelectAllCommand { get; }

        #endregion

        #region Command Methods

        private void ExecuteSearch() => LoadMissions();

        private bool CanExecuteSearch()
            => !string.IsNullOrEmpty(SearchText)
               || !string.IsNullOrEmpty(SelectedAgv)
               || !string.IsNullOrEmpty(SelectedRobotArm);

        private async void ExecuteAddMission()
        {
            try
            {
                var result = await _dialogCoordinator.ShowMessageAsync(
                    this, "미션 추가", "새로운 미션을 추가하시겠습니까?",
                    MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "성공", "미션이 추가되었습니다.");
                    LoadMissions();
                }
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "오류", $"미션 추가 중 오류: {ex.Message}");
            }
        }

        private async void ExecuteCancelMission()
        {
            try
            {
                var selectedMissions = Missions.Where(m => m.IsSelected).ToList();
                if (!selectedMissions.Any())
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "알림", "취소할 미션을 선택해주세요.");
                    return;
                }

                var result = await _dialogCoordinator.ShowMessageAsync(
                    this, "미션 취소",
                    $"선택된 {selectedMissions.Count}개의 미션을 취소하시겠습니까?",
                    MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    foreach (var mission in selectedMissions)
                        mission.Status = MissionStatus.Canceled;

                    await _dialogCoordinator.ShowMessageAsync(this, "성공", "선택된 미션이 취소되었습니다.");
                }
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "오류", $"미션 취소 중 오류: {ex.Message}");
            }
        }

        private bool CanExecuteCancelMission() => Missions?.Any(m => m.IsSelected) == true;

        private void ExecuteRefresh() => LoadMissions();

        /// <summary>
        /// 헤더 버튼 클릭 시 전체 선택/해제 토글
        /// </summary>
        private void ExecuteToggleSelectAll()
        {
            if (Missions == null || Missions.Count == 0) return;

            // 현재 모든 미션이 선택되어 있는지 확인
            bool allSelected = Missions.All(m => m.IsSelected);

            if (allSelected)
            {
                // 모두 선택되어 있으면 → 전체 해제
                foreach (var mission in Missions)
                {
                    mission.IsSelected = false;
                }
            }
            else
            {
                // 하나라도 선택 안되어 있으면 → 전체 선택
                foreach (var mission in Missions)
                {
                    mission.IsSelected = true;
                }
            }

            // 토글 완료 (버튼 텍스트 업데이트 불필요)
        }

        private async void ExecuteExport()
        {
            try
            {
                var selected = Missions?.Where(m => m.IsSelected).ToList();
                if (selected == null || !selected.Any())
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "알림", "내보낼 미션을 선택해주세요.");
                    return;
                }
                await _dialogCoordinator.ShowMessageAsync(this, "성공",
                    $"선택된 {selected.Count}개의 미션이 내보내기되었습니다.");
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "오류", $"내보내기 중 오류: {ex.Message}");
            }
        }

        private bool CanExecuteExport() => Missions?.Any(m => m.IsSelected) == true;

        #endregion

        #region Private Methods

        private void LoadInitialData()
        {
            var missions = new ObservableCollection<Mission>
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

            // 각 미션에 PropertyChanged 이벤트 연결
            foreach (var mission in missions)
                mission.PropertyChanged += Mission_PropertyChanged;

            Missions = missions;
        }

        private void Missions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Mission mission in e.NewItems)
                    mission.PropertyChanged += Mission_PropertyChanged;

            if (e.OldItems != null)
                foreach (Mission mission in e.OldItems)
                    mission.PropertyChanged -= Mission_PropertyChanged;

        }

        /// <summary>
        /// 개별 미션의 IsSelected 상태 변경 시 호출
        /// </summary>
        private void Mission_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 필요시 추가 로직 작성
        }

        private void LoadMissions() => LoadInitialData();

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}