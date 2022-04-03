namespace Lotlab.PluginCommon.Updater
{
    public struct FileInfo
    {
        public string FileName;
        public string Url;
        public string Hash;

        public FileInfo(string name, string url, string hash)
        {
            FileName = name;
            Url = url;
            Hash = hash;
        }
    }
}
