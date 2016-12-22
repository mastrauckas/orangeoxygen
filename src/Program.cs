using System;
using Autofac;

namespace OrangeOxygen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = Bootstrap.InitializeContainer("/home/melissa/TestPictures/Testing",
                                                            "/home/melissa/TestPictures/MoveLocation/GoodPictures",
                                                            "/home/melissa/TestPictures/MoveLocation/BadDate");

            using (var scope = container.BeginLifetimeScope())
            {
                var rm = scope.Resolve<IResourceManager>();
                rm.ManageFiles();
            }

            //manager.ManageFiles(DigitalFile.GetAllFiles("/home/melissa/TestPictures/Testing", "*", true));

            //var groups = fileInfos.GroupBy(fi => fi.FileHash).OrderByDescending(g => g.Count()).ToList();

            //Console.WriteLine(($"Files found: {fileInfos.Count()}"));

            //foreach (var extension in fileInfos.Select(fi => fi.FileInformation.Extension.ToLower()).Distinct())
            //{
            //    var files = fileInfos.Where(fi => fi.FileInformation.Extension.ToLower() == extension).ToList();
            //    Console.WriteLine(($"Extension {extension}: {files.Count()} Total Size: {FormatBytes(files.Sum(fi => fi.FileInformation.Length))}"));
            //}


        }

        private static string FormatBytes(Int64 bytes)
        {
            if (bytes >= 0x1000000000000000) { return ((double)(bytes >> 50) / 1024).ToString("0.### EB"); }
            if (bytes >= 0x4000000000000) { return ((double)(bytes >> 40) / 1024).ToString("0.### PB"); }
            if (bytes >= 0x10000000000) { return ((double)(bytes >> 30) / 1024).ToString("0.### TB"); }
            if (bytes >= 0x40000000) { return ((double)(bytes >> 20) / 1024).ToString("0.### GB"); }
            if (bytes >= 0x100000) { return ((double)(bytes >> 10) / 1024).ToString("0.### MB"); }
            if (bytes >= 0x400) { return ((double)(bytes) / 1024).ToString("0.###") + " KB"; }
            return bytes.ToString("0 Bytes");
        }
    }
}
