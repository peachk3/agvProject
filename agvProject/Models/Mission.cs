using System;
using System.ComponentModel;

namespace agvProject.Models
{
    public class Mission : INotifyPropertyChanged
    {
        private string _missionId;
        private DateTime _addMissionTime;
        private string _processName;
        private string _startLocation;
        private int _total;
        private DateTime? _missionStartTime;
        private string _finalLocation;
        private MissionStatus _status;
        private DateTime? _missionEndTime;
        private TimeSpan? _duration;
        private string _addInfo;
        private bool _isSelected;

        // 기존 속성들은 그대로 유지...
        public string MissionId
        {
            get => _missionId;
            set
            {
                _missionId = value;
                OnPropertyChanged(nameof(MissionId));
            }
        }

        public DateTime AddMissionTime
        {
            get => _addMissionTime;
            set
            {
                _addMissionTime = value;
                OnPropertyChanged(nameof(AddMissionTime));
            }
        }

        public string ProcessName
        {
            get => _processName;
            set
            {
                _processName = value;
                OnPropertyChanged(nameof(ProcessName));
            }
        }

        public string StartLocation
        {
            get => _startLocation;
            set
            {
                _startLocation = value;
                OnPropertyChanged(nameof(StartLocation));
            }
        }

        public int Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        public DateTime? MissionStartTime
        {
            get => _missionStartTime;
            set
            {
                _missionStartTime = value;
                OnPropertyChanged(nameof(MissionStartTime));
            }
        }

        public string FinalLocation
        {
            get => _finalLocation;
            set
            {
                _finalLocation = value;
                OnPropertyChanged(nameof(FinalLocation));
            }
        }

        public MissionStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public DateTime? MissionEndTime
        {
            get => _missionEndTime;
            set
            {
                _missionEndTime = value;
                OnPropertyChanged(nameof(MissionEndTime));
            }
        }

        public TimeSpan? Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public string AddInfo
        {
            get => _addInfo;
            set
            {
                _addInfo = value;
                OnPropertyChanged(nameof(AddInfo));
            }
        }

        // 🔥 핵심 버그 수정: IsSelected 속성
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));

                    //디버그용
                    System.Diagnostics.Debug.WriteLine($"Mission: {MissionId} : IsSelected = {value}");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum MissionStatus
    {
        Standby,    // 대기
        Processing, // 진행중
        Completed,  // 완료
        Error,      // 오류
        Canceled    // 취소
    }
}