using System;

namespace Lotlab.PluginCommon.Updater
{
    [Obsolete]
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

    public struct VersionInfoV2
    {
        public Int64 Time;
        public string Version;
        public string ChangeLog;
        public FileInfo[] Files;

        public VersionInfoV2(Int64 time, string version, FileInfo[] files, string changeLog = "")
        {
            Time = time;
            Version = version;
            ChangeLog = changeLog;
            Files = files;
        }
    }
}
