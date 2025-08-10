using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace agvProject.Views.Auth
{
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();

            // 엔터 키 입력시 로그인 
            UserIdTextBox.KeyDown += async (s, e) => { if (e.Key == Key.Enter) await Login(); };
            PasswordBox.KeyDown += async (s, e) => { if (e.Key == Key.Enter) await Login(); };


        }


        private async void LoginWindow_Closing(object? s, System.ComponentModel.CancelEventArgs e)
        {
            // 로그인 성공하면 화면 닫기
            if (this.DialogResult == true) return;

            e.Cancel = true; // 창 닫기 취소 -> 사용자한테 물어봄
            var result = await this.ShowMessageAsync(
                "앱 종료",
                "완전히 종료하시겠습니까?",
                MessageDialogStyle.AffirmativeAndNegative
                );

            if (result == MessageDialogResult.Affirmative)
            {
                // 완전종료
                this.Closing -= LoginWindow_Closing; // 재진입 방지
                Application.Current.Shutdown(); // 앱 종료
            }
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        private async Task Login()
        {
            string userId = UserIdTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // 입력값 확인
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                await this.ShowMessageAsync(
                    "입력 오류",
                    "아이디와 비밀번호 모두 입력해주세요",
                    MessageDialogStyle.Affirmative);
                return;
            }

            // 간단한 로그인 검증 (admin/1234)
            if (userId == "admin" && password == "1234")
            {
                // 로그인 성공 - DialogResult를 true로 설정하여 MainWindow로 이동
                await this.ShowMessageAsync(
                    "로그인 성공",
                    "환영합니다!",
                    MessageDialogStyle.Affirmative);

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                // 로그인 실패 - DialogResult를 false로 설정하여 다시 로그인 창 표시
                await this.ShowMessageAsync(
                    "로그인 실패",
                    "아이디 또는 비밀번호가 올바르지 않습니다",
                    MessageDialogStyle.Affirmative
                );

                // 비밀번호 필드 초기화 및 포커스
                PasswordBox.Password = "";
                UserIdTextBox.Focus();

                // DialogResult를 false로 설정하여 App.xaml.cs에서 다시 로그인 창을 표시하도록 함
                this.DialogResult = false;
            }
        }
    }
}