using System.ComponentModel;
using System.Windows.Input;

namespace agvProject.ViewModels
// 테스트용 VM
{
    // MainMonitoringViewModel: 수동 제어 모드 및 토출 버튼 로직 처리
    public class MainMonitoringViewModel : INotifyPropertyChanged
    {
        private bool _isManualMode; // 수동 제어 모드 상태 저장 (ON/OFF)

        // 하단 버튼 활성화 여부
        // true: 하단 버튼 활성화, false: 비활성화
        public bool IsManualMode
        {
            get => _isManualMode; // 수동 제어 모드 상태 저장 (ON/OFF)
            set
            {
                if (_isManualMode != value)
                {
                    _isManualMode = value;
                    // UI 갱신
                    OnPropertyChanged(nameof(IsManualMode));
                    // 상단 버튼 글자 갱신
                    OnPropertyChanged(nameof(ToggleButtonText));
                }
            }
        }

        // 상단 버튼 글자
        // 수동 모드 ON이면 "OFF" 표시, OFF이면 "ON" 표시
        public string ToggleButtonText => IsManualMode ? "수동 제어 모드 OFF" : "수동 제어 모드 ON";

        // ------------------------------
        // ICommand 속성 (버튼 바인딩용)
        // ------------------------------

        // 상단 버튼: 수동 모드 토글
        public ICommand ToggleManualCommand { get; }

        // 하단 버튼: 적재 시설 토출
        // CanExecute 조건: IsManualMode가 true일 때만 활성화
        public ICommand EjectCargoCommand { get; }

        // ------------------------------
        // 생성자
        // ------------------------------
        public MainMonitoringViewModel()
        {
            // 상단 버튼 클릭 시 ToggleManual() 실행
            ToggleManualCommand = new MyRelayCommand(ToggleManual);

            // 하단 버튼 클릭 시 EjectCargo() 실행
            // CanExecute 조건으로 IsManualMode 사용: 수동 모드가 켜져야 버튼 클릭 가능
            EjectCargoCommand = new MyRelayCommand(EjectCargo, () => IsManualMode);
        }

        // ------------------------------
        // 메서드
        // ------------------------------

        // 수동 제어 버튼 클릭
        private void ToggleManual()
        {
            // 수동 모드 ON/OFF 토글
            IsManualMode = !IsManualMode;
            // 하단 버튼 CanExecute 상태 갱신
            // 이 호출이 있어야 하단 버튼 활성/비활성 UI가 즉시 반영됨
            (EjectCargoCommand as MyRelayCommand)?.RaiseCanExecuteChanged();
        }

        // 적재 시설 수동 제어 버튼 (토출)
        private void EjectCargo()
        {
            // 실제 토출 동작 처리 (예: 시리얼/통신으로 AGV 서보 모터 제어)
            // 나중에 AGV 컨트롤 코드, 모터 PWM 신호, 혹은 UI 애니메이션 등 연결 가능
            Console.WriteLine("토출구가 열리면서 물건이 쏟아집니다!");
        }

        // ------------------------------
        // INotifyPropertyChanged 구현
        // ------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            // PropertyChanged 이벤트 발생
            // UI 바인딩 속성 갱신
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ------------------------------
    // RelayCommand 구현 (ViewModel용)
    // ------------------------------
    public class MyRelayCommand : ICommand
    {
        private readonly Action _execute;   // 실행할 메서드
        private readonly Func<bool> _canExecute;    // 실행 가능 여부 판단 메서드 (옵션)

        // 생성자
        public MyRelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        // 버튼 활성화/비활성화 여부
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        // 버튼 클릭 시 실행
        public void Execute(object parameter) => _execute();

        // UI 갱신 이벤트
        public event EventHandler CanExecuteChanged;

        // CanExecute 상태가 바뀌면 UI 갱신
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
