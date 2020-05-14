using System;
using System.Collections.Generic;
using System.Text;

namespace GLT
{
    public static class _Consts
    {
        public static class Redis
        {
            public const string Key1 = "Redis";

            public const string TableVer = "TableVer";
            public const string TableVer_DefaultValue = "redis01:6379,defaultDatabase=1";
        }
        public static class db
        {
            public const string CoreDB = "GLT_Core";
            public const string CoreDB_R = "CoreDB_R";
            public const string CoreDB_W = "CoreDB_W";
            public const string CoreDB_Default = "Data Source=db01;Initial Catalog=" + CoreDB + ";Persist Security Info=True;User ID=sa;Password=sa";

            public const string MainDB_R = "MainDB_R";
            public const string MainDB_W = "MainDB_W";
            public const string EventLog_R = "EventLog_R";
            public const string EventLog_W = "EventLog_W";
            public const string Reporting_R = "Reporting_R";
            public const string Reporting_W = "Reporting_W";
            public const string UserDB_R = "UserDB_R";
            public const string UserDB_W = "UserDB_W";
            public const string LogDB_R = "LogDB_R";
            public const string LogDB_W = "LogDB_W";
        }
    }
}
