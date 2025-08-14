using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agvProject.Helpers
{
    class Common
    {
        //NLogs 인스턴스
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static readonly string CONNSTR = "Server=127.0.0.1:3306;Database=Docker MySQL;Uid=root;Pwd=12345;Charset=utf8\"";
    }
}
