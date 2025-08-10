using agvProject.Views;
using agvProject.Views.Auth;
using System.Windows;

namespace agvProject
{

    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            while (true)
            {
                var loginWindow = new LoginWindow();
                bool? result = loginWindow.ShowDialog();

                if (result == true) // 로그인성공
                {
                    var mainWindow = new Views.MainWindow();
                    Current.MainWindow = mainWindow; // 메인 창으로 설정
                    mainWindow.Show();
                    return; // 로그인 루프 종료
                }
                else if (result == false) // 로그인실패
                {
                    // 로그인 실패 시 다시 로그인 창을 표시 (while 루프가 계속 실행됨)
                    continue;
                }
                else // result == null (창이 X 버튼으로 닫힌 경우)
                {
                    // 완전 종료
                    Shutdown();
                    return;
                }
            }
        }
    }
}