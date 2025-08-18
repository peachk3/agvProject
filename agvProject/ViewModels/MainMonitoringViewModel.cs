// ViewModels/MainMonitoringViewModel.cs
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using agvProject.Models;
using agvProject.Views;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace agvProject.ViewModels
{
    public partial class MainMonitoringViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialog;

        // 콤보 데이터 소스
        private ObservableCollection<MissionAddTest> _missions = new();
        public ObservableCollection<MissionAddTest> Missions
        {
            get => _missions;
            set => SetProperty(ref _missions, value);
        }

        // 선택 항목
        private MissionAddTest? _selectedMission;
        public MissionAddTest? SelectedMission
        {
            get => _selectedMission;
            set => SetProperty(ref _selectedMission, value);
        }

        public MainMonitoringViewModel(IDialogCoordinator dialogCoordinator = null)
        {
            dialog = dialogCoordinator ?? DialogCoordinator.Instance; // fallback

            // 화면 뜨자마자 목록 로드
            _ = LoadMissionsAsync();
        }

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
                // 기본 MessageBox로 알림 이거 수정 해야함
                MessageBox.Show(ex.Message, "조회 오류");
            }
        }

        /// <summary>
        /// 다이얼로그 띄우고 값 받기 → DB INSERT → 콤보박스 등록
        /// </summary>
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
                MessageBox.Show("num은 정수로 입력하세요.", "입력 오류");
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

                // 등록 직후 콤보 박스 갱신
                await LoadMissionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DB 오류");
            }
        }

        // 미션 시작 test
        [RelayCommand]
        public void StartMission()
        {
            if (SelectedMission is null)
            {
                MessageBox.Show("미션을 선택하세요.", "알림");
                return;
            }
            MessageBox.Show($"[{SelectedMission.Id}] {SelectedMission.Name} 시작합니다.", "미션 시작");
        }
    }
}
