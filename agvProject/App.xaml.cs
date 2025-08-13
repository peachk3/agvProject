using agvProject.Views;
using agvProject.Views.Auth;
using System.Windows;

namespace agvProject
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 로그인 창을 열고, 로그인 성공 시 메인 윈도우를 열도록 설정
            while(true)
            {
                var loginWindow = new LoginWindow();
                bool? result = loginWindow.ShowDialog();

                if(Application.Current.MainWindow != null && Application.Current.MainWindow is Views.MainWindow)
                {
                    return; // 앱 계속 실행

                }

                // 로그인 실패나 취소시 앱 종료
                if(result != true)
                {
                    Application.Current.Shutdown(); // 앱 종료
                    return;

                }
            }
        }
    }
}