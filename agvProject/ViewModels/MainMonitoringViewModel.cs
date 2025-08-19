using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MahApps.Metro.Controls.Dialogs;
using agvProject.Models;
using agvProject.Views;

namespace agvProject.ViewModels
{
    /// <summary>
    /// 메인 모니터링 뷰 모델
    /// - AGV 맵 이동 테스트 완료(오버레이 이동)
    /// - 미션 로드(콤보)/등록(미션페이지로 옮겨야 함)/시작(그리드)
    /// </summary>
    public partial class MainMonitoringViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialog;

        #region === Map / AGV 오버레이 (사이즈 & 위치) ===

        // 맵/AGV 표시 크기 (맵 원본 픽셀 기준)
        public int MapWidth { get; } = 520;
        public int MapHeight { get; } = 390;    // Map 520 * 390
        public int AgvSize { get; } = 80;       // Agv A 80 (대략)

        // AGV 위치. 상세 위치 MoveAgvTo 내부에서 설정.
        [ObservableProperty] private double agvX;
        [ObservableProperty] private double agvY;

        #endregion

        #region === Map RFID 시뮬레이션 테스트(버튼) (포인트 & 이동) ===

        // 테스트용 RFID 포인트 (맵 좌표 기준)
        private readonly List<Point> _rfidPoints = new()
        {
            new Point( 45, 129),  // 대기 및 충전소 (위: A)
            new Point(235,  48),  // 적재 장소
            new Point(450,  56),  // 분류 앞자리 
            new Point( 45, 250),  // 대기 및 충전소 (아래: B)
            new Point(472, 316),  // 분류 끝자리 
            new Point(305, 343),  // 창고 앞
        };
        private int _rfidIndex = 0;

        // 시작 시 첫 포인트로 초기 위치 설정 (A 대기 포인트)
        private void InitAgvAtFirstPoint()
        {
            var p = _rfidPoints[0];
            MoveAgvTo(p.X, p.Y);
        }

        // Agv 주어진 좌표로 이동 (아이콘 중앙 기준)
        private void MoveAgvTo(double cx, double cy)
        {
            AgvX = cx - AgvSize / 2.0;
            AgvY = cy - AgvSize / 2.0;
        }

        // 버튼: 위치 인식하기 (포인트로 이동시키는 기능)
        [RelayCommand]
        private void LocateNow()
        {
            if (_rfidPoints.Count == 0) return;

            var p = _rfidPoints[_rfidIndex];
            MoveAgvTo(p.X, p.Y);

            _rfidIndex = (_rfidIndex + 1) % _rfidPoints.Count;
        }

        #endregion

        #region === 미션 (콤보박스 & 미션 시작 리스트: 임무목록) ===

        // 콤보 데이터 소스
        private ObservableCollection<MissionAddTest> _missions = new();
        public ObservableCollection<MissionAddTest> Missions
        {
            get => _missions;
            set => SetProperty(ref _missions, value);
        }

        // 콤보에서 선택한 항목
        private MissionAddTest? _selectedMission;
        public MissionAddTest? SelectedMission
        {
            get => _selectedMission;
            set => SetProperty(ref _selectedMission, value);
        }

        // 시작된 임무 목록(우측 그리드에 표시: 현재 미션명 빼고는 임의 데이터)
        private ObservableCollection<MissionAddTest> _startedMissions = new();
        public ObservableCollection<MissionAddTest> StartedMissions
        {
            get => _startedMissions;
            set => SetProperty(ref _startedMissions, value);
        }

        #endregion

        #region === MainMonitoringViewModel Constructor ===

        public MainMonitoringViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            dialog = dialogCoordinator ?? DialogCoordinator.Instance;

            // 화면 로드 시 미션 목록 데이터 로드
            _ = LoadMissionsAsync();

            // 화면 로드 시 AGV 처음 위치 설정
            InitAgvAtFirstPoint();
        }

        #endregion

        #region === DB: Load Missions //// 새 미션 등록(다이얼로그) <- 이 부분을 옮기세여!! ===

        [RelayCommand]
        public async Task LoadMissionsAsync()
        {
            try
            {
                await using var db = new AppDbContext();

                var list = await db.MissionAddTest
                    .AsNoTracking()
                    .OrderBy(m => m.Id)
                    .ToListAsync();

                Missions = new ObservableCollection<MissionAddTest>(list);

                if (Missions.Count > 0 && SelectedMission is null)
                    SelectedMission = Missions[0];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "조회 오류");
            }
        }

        // 다이얼로그 오픈 입력 → DB INSERT → 콤보 갱신
        [RelayCommand]
        public async Task NewMissionAsync()
        {
            var dlg = new NewMissionDialog();
            var ok = dlg.ShowDialog();
            if (ok != true) return;

            var name = dlg.name;
            var numText = dlg.num;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("미션 ID를 입력하세요.", "입력 오류");
                return;
            }
            if (!int.TryParse(numText, out var numValue))
            {
                MessageBox.Show("num: 정수로 입력하세요.", "입력 오류");
                return;
            }

            try
            {
                await using var db = new AppDbContext();

                var entity = new MissionAddTest { Name = name, Num = numValue };
                await db.MissionAddTest.AddAsync(entity);

                var affected = await db.SaveChangesAsync();

                if (affected > 0)
                    MessageBox.Show($"저장 성공 (id={entity.Id})", "미션 등록");
                else
                    MessageBox.Show("저장된 행이 없습니다.", "알림");

                // 등록 직후 콤보 갱신
                await LoadMissionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DB 오류");
            }
        }

        #endregion

        #region === 미션 시작 ===

        // 미션 시작 → 우측 임무 목록에 누적 표시
        [RelayCommand]
        public void StartMission()
        {
            if (SelectedMission is null)
            {
                MessageBox.Show("미션을 선택하세요.", "알림");
                return;
            }

            var row = new MissionAddTest
            {
                Id = SelectedMission.Id,
                Name = SelectedMission.Name,
                Num = SelectedMission.Num,
                RowNo = StartedMissions.Count + 1 // 화면용 No. 자동 순번
            };

            StartedMissions.Add(row);

            MessageBox.Show($"[{row.Id}] {row.Name} 시작합니다.", "미션 시작");
        }

        #endregion
    }
}
