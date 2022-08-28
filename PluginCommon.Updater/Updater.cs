using System;
using System.IO;
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

        [Obsolete("Use GetLatestV2 instead.")]
        public VersionInfo GetLatest(string path = "/update.json")
        {
            var content = HttpWrapper.Get(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfo>(content);
        }

        [Obsolete("Use GetLatestV2Async instead.")]
        public async Task<VersionInfo> GetLatestAsync(string path = "/update.json")
        {
            var content = await HttpWrapper.GetAsync(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfo>(content);
        }

        public VersionInfoV2 GetLatestV2(string path = "/updatev2.json")
        {
            var content = HttpWrapper.Get(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfoV2>(content);
        }

        public async Task<VersionInfoV2> GetLatestV2Async(string path = "/updatev2.json")
        {
            var content = await HttpWrapper.GetAsync(baseUrl + path);
            return JsonConvert.DeserializeObject<VersionInfoV2>(content);
        }

        [Obsolete("Use GetLatestV2 instead.")]
        public FileInfo[] GetFileInfos(string path)
        {
            var content = HttpWrapper.Get(baseUrl + path);
            return JsonConvert.DeserializeObject<FileInfo[]>(content);
        }

        [Obsolete("Use GetLatestV2Async instead.")]
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
}
