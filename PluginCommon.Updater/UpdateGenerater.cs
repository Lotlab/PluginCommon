using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Lotlab.PluginCommon.Updater
{
    public class UpdateGenerater
    {
        string SrcDir { get; }
        string DstDir { get; }

        public UpdateGenerater(string src, string dst) 
        {
            SrcDir = src;
            DstDir = dst;
        }

        [Obsolete]
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

        public void GenerateV2(string version, string changelogs)
        {
            // Prepare dir
            if (!Directory.Exists(DstDir))
                Directory.CreateDirectory(DstDir);

            const string filesDirName = "files";
            var packDir = Path.Combine(DstDir, filesDirName);
            if (!Directory.Exists(packDir))
                Directory.CreateDirectory(packDir);

            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            List<FileInfo> fileList = new List<FileInfo>();

            var files = Directory.EnumerateFiles(SrcDir, "*", SearchOption.AllDirectories)
                    .Select(p => p.Replace(SrcDir, ""));

            foreach (var item in files)
            {
                var filePath = Path.Combine(SrcDir, item);
                var fileName = item.Replace("\\", "/");

                // Calculate hash
                var hash = Updater.ComputeHash(filePath);
                fileList.Add(new FileInfo(fileName, filesDirName + "/" + fileName, hash));

                // Copy file
                var dstPath = Path.Combine(packDir, fileName);

                if (!Directory.Exists(Path.GetDirectoryName(dstPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dstPath));

                File.Copy(filePath, dstPath, true);
            }

            VersionInfoV2 versionInfo = new VersionInfoV2(timestamp, version, fileList.ToArray(), changelogs);
            File.WriteAllText(Path.Combine(DstDir, "updatev2.json"), JsonConvert.SerializeObject(versionInfo));
        }
    }
}
