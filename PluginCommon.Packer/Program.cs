using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Lotlab.PluginCommon.Updater;

namespace GardeningTracker.Packer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandArgs arg = new CommandArgs(args);

            // Generate update info
            if (arg.V1Compatbility)
            {
                var generatorv1 = new UpdateGenerater(arg.SourceDir, Path.Combine(arg.TargetDir, "update"));
                generatorv1.Generate(arg.Version, "");
            }

            var generator = new UpdateGenerater(arg.SourceDir, Path.Combine(arg.TargetDir, "updatev2"));
            generator.GenerateV2(arg.Version, "");

            // Pack release file
            var packDir = Path.Combine(arg.TargetDir, "pack");
            if (!Directory.Exists(packDir))
                Directory.CreateDirectory(packDir);

            PackZip(arg.SourceDir, Path.Combine(packDir, $"{arg.Name}-{arg.Version}.zip"));
        }

        static void PackZip(string rootDir, string outName)
        {
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var files = Directory.EnumerateFiles(rootDir, "*", SearchOption.AllDirectories)
                        .Select(p => p.Replace(rootDir, ""));

                    foreach (var file in files)
                    {
                        archive.CreateEntryFromFile(Path.Combine(rootDir, file), file);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outName), FileMode.Create))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fileStream);
                }
            }
        }
    }

    class CommandArgs
    {
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Entry { get; set; }
        public bool V1Compatbility { get; set; } = false;

        public CommandArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--src":
                        SourceDir = args[++i];
                        break;
                    case "--dst":
                        TargetDir = args[++i];
                        break;
                    case "--name":
                        Name = args[++i];
                        break;
                    case "--entry":
                        Entry = args[++i];
                        break;
                    case "--version":
                        Version = args[++i];
                        break;
                    case "--v1":
                        V1Compatbility = true;
                        break;
                    default:
                        throw new Exception($"Unexpected arg {args[i]}");
                }
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new Exception("Missing argument: Name");
            }
            if (string.IsNullOrWhiteSpace(SourceDir))
            {
                SourceDir = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (string.IsNullOrWhiteSpace(TargetDir))
            {
                TargetDir = Path.Combine(SourceDir, "..");
            }
            if (string.IsNullOrWhiteSpace(Version) && string.IsNullOrWhiteSpace(Entry))
            {
                throw new Exception("Missing argument: Version or Entry");
            }
            if (string.IsNullOrWhiteSpace(Version))
            {
                var entry = Path.Combine(SourceDir, Entry);
                Version = FileVersionInfo.GetVersionInfo(entry).FileVersion;
            }
        }
    }
}
