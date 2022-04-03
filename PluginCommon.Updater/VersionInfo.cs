using System;

namespace Lotlab.PluginCommon.Updater
{
    public struct VersionInfo
    {
        public Int64 Time;
        public string Version;
        public string Url;
        public string ChangeLog;

        public VersionInfo(Int64 time, string version, string url, string changeLog = "")
        {
            Time = time;
            Version = version;
            Url = url;
            ChangeLog = changeLog;
        }
    }
}
