using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace agvProject.Helpers
{
    internal static class Common
    {
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static readonly string CONNSTR =
            "Server=localhost;Port=3306;Database=agv_test;Uid=root;Password=12345;CharSet=utf8mb4;SslMode=none";

        public static readonly IDialogCoordinator DIALOGCOORDINATOR = DialogCoordinator.Instance;
    }
}