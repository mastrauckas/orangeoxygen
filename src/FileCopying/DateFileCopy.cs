using System;
using System.IO;
using System.Linq;

namespace OrangeOxygen.FileCopying
{
    public class DateFileCopy : IFileCopy
    {
        public void CopyFile(DigitalFile digitalFile, string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new NullReferenceException("baseDirectory");

            if (Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException($"Base directory {baseDirectory} doesn't exist");

            var dt = digitalFile.FileDate;
            var year = dt.Year.ToString();

            //Make sure it's a 4 digit year.
            if (year.Count() < 4)
                throw new Exception("year length must be 4 digits.");

            var month = dt.Month.ToString("00");
            var day = dt.Month.ToString("00");

            var dir = Path.Combine(baseDirectory, year, month, day);
            Directory.CreateDirectory(dir);
            File.Copy(digitalFile.FileInformation.FullName, dir);
        }
    }
}
