using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lotlab.PluginCommon.Updater
{
    public class ExtendedUpdater : Updater
    {
        public string Version { get; set; }
        public string BasePath { get; }

        public ExtendedUpdater(string baseUrl, string basePath) : base(baseUrl)
        {
            BasePath = basePath;
        }

        /// <summary>
        /// Download files in list
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<string[]> UpdateAsync(VersionInfo info)
        {
            var fileList = await GetFileInfosAsync(info.Url);
            return await UpdateAsync(GetChangedFiles(fileList));
        }

        /// <summary>
        /// Download files in list
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Obsolete]
        public string[] Update(VersionInfo info)
        {
            var fileList = GetFileInfos(info.Url);
            return Update(GetChangedFiles(fileList));
        }

        /// <summary>
        /// Update specific file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="force">Set true to ignore old file hash check</param>
        /// <returns>True if success, false if no need to upgrade</returns>
        /// <exception cref="InvalidDataException">Downloaded file is not match with hash</exception>
        async Task<bool> updateFileAsync(FileInfo file, bool force = false)
        {
            // Check if old file is same as new file
            var localFile = Path.Combine(BasePath, file.FileName);

            if (File.Exists(localFile))
            {
                if (!force)
                {
                    var localHash = ComputeHash(localFile);
                    if (localHash.ToLower() == file.Hash.ToLower())
                    {
                        return false;
                    }
                }
            }
            else
            {
                var dirName = Path.GetDirectoryName(localFile);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
            }

            // download file
            var downloadFile = localFile + ".new";
            await HttpWrapper.GetFileAsync(baseUrl + file.Url, downloadFile);

            // calculate hash
            var downloadedHash = ComputeHash(downloadFile);
            if (downloadedHash.ToLower() != file.Hash.ToLower())
            {
                throw new InvalidDataException();
            }

            File.Delete(localFile);
            File.Move(downloadFile, localFile);

            return true;
        }

        /// <summary>
        /// Update specific file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True if success, false if no need to upgrade</returns>
        /// <exception cref="InvalidDataException">Downloaded file is not match with hash</exception>
        bool updateFile(FileInfo file, bool force = false)
        {
            // Check if old file is same as new file
            var localFile = Path.Combine(BasePath, file.FileName);

            if (File.Exists(localFile))
            {
                if (!force)
                {
                    var localHash = ComputeHash(localFile);
                    if (localHash.ToLower() == file.Hash.ToLower())
                    {
                        return false;
                    }
                }
            } 
            else
            {
                var dirName = Path.GetDirectoryName(localFile);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
            }

            // download file
            var downloadFile = localFile + ".new";
            HttpWrapper.GetFile(baseUrl + file.Url, downloadFile);

            // calculate hash
            var downloadedHash = ComputeHash(downloadFile);
            if (downloadedHash.ToLower() != file.Hash.ToLower())
            {
                throw new InvalidDataException();
            }

            File.Delete(localFile);
            File.Move(downloadFile, localFile);

            return true;
        }

        /// <summary>
        /// Get files which hash is not match.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ICollection<FileInfo> GetChangedFiles(ICollection<FileInfo> files)
        {
            List<FileInfo> changed = new List<FileInfo>();
            foreach (var file in files)
            {
                var localFile = Path.Combine(BasePath, file.FileName);
                if (File.Exists(localFile))
                {
                    var localHash = ComputeHash(localFile);
                    if (localHash.ToLower() == file.Hash.ToLower())
                    {
                        continue;
                    }
                }
                changed.Add(file);
            }

            return changed.ToArray();
        }

        /// <summary>
        /// Download files without file version check
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<string[]> UpdateAsync(ICollection<FileInfo> files)
        {
            List<string> updatedList = new List<string>();

            foreach (var file in files)
            {
                if (await updateFileAsync(file, true))
                {
                    updatedList.Add(file.FileName);
                }
            }

            return updatedList.ToArray();
        }

        /// <summary>
        /// Download files without file version check
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string[] Update(ICollection<FileInfo> files)
        {
            List<string> updatedList = new List<string>();

            foreach (var file in files)
            {
                if (updateFile(file, true))
                {
                    updatedList.Add(file.FileName);
                }
            }

            return updatedList.ToArray();
        }

        /// <summary>
        /// Download files in list
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<string[]> UpdateAsync(VersionInfoV2 info)
        {
            var files = GetChangedFiles(info.Files);
            return await UpdateAsync(files);
        }

        /// <summary>
        /// Download files in list
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string[] Update(VersionInfoV2 info)
        {
            var files = GetChangedFiles(info.Files);
            return Update(files);
        }

        /// <summary>
        /// Check if has update
        /// </summary>
        /// <returns>UpdateInfo if any, null if no update</returns>
        [Obsolete]
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
        [Obsolete]
        public VersionInfo? CheckUpdate()
        {
            var latestInfo = GetLatest();
            if (!CompareVersion(Version, latestInfo.Version)) return null;
            return latestInfo;
        }

        /// <summary>
        /// Check if has update
        /// </summary>
        /// <returns>UpdateInfo if any, null if no update</returns>
        public async Task<VersionInfoV2?> CheckUpdateV2Async()
        {
            var latestInfo = await GetLatestV2Async();
            if (!CompareVersion(Version, latestInfo.Version)) return null;
            return latestInfo;
        }

        /// <summary>
        /// Check if has update
        /// </summary>
        /// <returns>UpdateInfo if any, null if no update</returns>
        public VersionInfoV2? CheckUpdateV2()
        {
            var latestInfo = GetLatestV2();
            if (!CompareVersion(Version, latestInfo.Version)) return null;
            return latestInfo;
        }
    }
}
