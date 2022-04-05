using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lotlab.PluginCommon.Updater
{
    public class Updater
    {
        protected string baseUrl { get; }

        public Updater(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public bool CompareVersion(string current, string target)
        {
            return Version.Parse(target) > Version.Parse(current);
        }

        public VersionInfo GetLatest(string path = "/update.json")
        {
            var content = HttpWrapper.Get(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfo>(content);
        }

        public async Task<VersionInfo> GetLatestAsync(string path = "/update.json")
        {
            var content = await HttpWrapper.GetAsync(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfo>(content);
        }

        public FileInfo[] GetFileInfos(string path)
        {
            var content = HttpWrapper.Get(baseUrl + path);
            return JsonConvert.DeserializeObject<FileInfo[]>(content);
        }

        public async Task<FileInfo[]> GetFileInfosAsync(string path)
        {
            var content = await HttpWrapper.GetAsync(baseUrl + path);
            return JsonConvert.DeserializeObject<FileInfo[]>(content);
        }

        /// <summary>
        /// Compute SHA1 hash for specific file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static string ComputeHash(string path)
        {
            using(var fs = File.OpenRead(path))
            {
                var sha1 = SHA1CryptoServiceProvider.Create();
                var result = sha1.ComputeHash(fs);

                var sb = new StringBuilder();
                foreach (var b in result)
                {
                    sb.Append(b.ToString("x02"));
                }
                return sb.ToString();
            }
        }
    }

    public class ExtendedUpdater : Updater
    {
        public string Version { get; set; }
        public string BasePath { get; }

        public ExtendedUpdater(string baseUrl, string basePath) : base(baseUrl)
        {
            BasePath = basePath;
        }

        /// <summary>
        /// Download files
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<string[]> UpdateAsync(VersionInfo info)
        {
            var fileList = await GetFileInfosAsync(info.Url);
            List<string> updatedList = new List<string>();

            foreach (var file in fileList)
            {
                var localFile = Path.Combine(BasePath, file.FileName);
                var downloadFile = localFile + ".new";
                if (File.Exists(localFile))
                {
                    var localHash = ComputeHash(localFile);
                    if (localHash.ToLower() == file.Hash.ToLower())
                    {
                        continue;
                    }
                }
                await HttpWrapper.GetFileAsync(baseUrl + file.Url, downloadFile);

                var downloadedHash = ComputeHash(downloadFile);
                if (downloadedHash.ToLower() != file.Hash.ToLower())
                {
                    throw new InvalidDataException();
                }

                File.Delete(localFile);
                File.Move(downloadFile, localFile);

                updatedList.Add(file.FileName);
            }

            return updatedList.ToArray();
        }

        /// <summary>
        /// Download files in list
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string[] Update(VersionInfo info)
        {
            var fileList = GetFileInfos(info.Url);
            List<string> updatedList = new List<string>();

            foreach (var file in fileList)
            {
                var localFile = Path.Combine(BasePath, file.FileName);
                var downloadFile = localFile + ".new";
                if (File.Exists(localFile))
                {
                    var localHash = ComputeHash(localFile);
                    if (localHash.ToLower() == file.Hash.ToLower())
                    {
                        continue;
                    }
                }
                HttpWrapper.GetFile(baseUrl + file.Url, downloadFile);

                var downloadedHash = ComputeHash(downloadFile);
                if (downloadedHash.ToLower() != file.Hash.ToLower())
                {
                    throw new InvalidDataException();
                } 

                File.Move(downloadFile, localFile);
             
                updatedList.Add(file.FileName);
            }

            return updatedList.ToArray();
        }

        /// <summary>
        /// Check if has update
        /// </summary>
        /// <returns>UpdateInfo if any, null if no update</returns>
        public async Task<VersionInfo?> CheckUpdateAsync()
        {
            var latestInfo = await GetLatestAsync();
            if (!CompareVersion(Version, latestInfo.Version)) return null;
            return latestInfo;
        }

        /// <summary>
        /// Check if has update
        /// </summary>
        /// <returns>UpdateInfo if any, null if no update</returns>
        public VersionInfo? CheckUpdate()
        {
            var latestInfo = GetLatest();
            if (!CompareVersion(Version, latestInfo.Version)) return null;
            return latestInfo;
        }
    }

    public class UpdateGenerater
    {
        string SrcDir { get; }
        string DstDir { get; }

        public UpdateGenerater(string src, string dst) 
        {
            SrcDir = src;
            DstDir = dst;
        }

        public void Generate(string version, string changelogs)
        {
            // Prepare dir
            if (!Directory.Exists(DstDir))
                Directory.CreateDirectory(DstDir);

            var packDir = Path.Combine(DstDir, version);
            if (!Directory.Exists(packDir))
                Directory.CreateDirectory(packDir);

            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var versionFileName = version + ".json";

            VersionInfo versionInfo = new VersionInfo(timestamp, version, versionFileName, changelogs);
            List<FileInfo> fileList = new List<FileInfo>();

            var files = Directory.EnumerateFiles(SrcDir, "*", SearchOption.AllDirectories)
                    .Select(p => p.Replace(SrcDir, ""));

            foreach (var item in files)
            {
                var filePath = Path.Combine(SrcDir, item);
                var fileName = item.Replace("\\", "/");

                // Calculate hash
                var hash = Updater.ComputeHash(filePath);
                fileList.Add(new FileInfo(fileName, version + "/" + fileName, hash));

                // Copy file
                var dstPath = Path.Combine(packDir, fileName);

                if (!Directory.Exists(Path.GetDirectoryName(dstPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dstPath));

                File.Copy(filePath, dstPath, true);
            }

            File.WriteAllText(Path.Combine(DstDir, "update.json"), JsonConvert.SerializeObject(versionInfo));
            File.WriteAllText(Path.Combine(DstDir, versionFileName), JsonConvert.SerializeObject(fileList));
        }
    }
}
