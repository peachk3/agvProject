using MahApps.Metro.Controls;

namespace agvProject.Views.Auth
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();

            // 로그인 버튼 클릭 이벤트 핸들러 등록
            LoginBtn.Click += LoginBtn_Click;

            // 엔터 키 입력 시 로그인 시도
            UserIdTextBox.KeyDown += LoginWindow_KeyDown;
            PasswordBox.KeyDown += LoginWindow_KeyDown;
        }
    }
}
