using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace OrangeOxygen
{
    public class DigitalFile
    {
        private DigitalFile() { }

        public DigitalFile(FileInfo fi)
        {
            if (fi == null)
                throw new NullReferenceException();

            FileInformation = fi;
        }

        public FileInfo FileInformation
        {
            get; private set;
        }

        public string FileAndPath
        {
            get
            {
                return FileInformation.FullName;
            }
        }

        public string FileHash
        {
            get
            {
                lock (m_hashLock)
                    if (m_hash == null)
                        using (var md5 = MD5.Create())
                            m_hash = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(FileAndPath))).Replace("-", "");

                return m_hash;
            }
        }

        public DateTime FileDate
        {
            get
            {
                // Read all metadata from the image
                var directories = ImageMetadataReader.ReadMetadata(FileAndPath);

                // Find the so-called Exif "SubIFD" (which may be null)
                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                // Read the DateTime tag value
                var dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
                return dateTime.HasValue ? dateTime.Value : FileInformation.CreationTime;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return m_hash == ((DigitalFile)obj).FileHash;
        }

        public override int GetHashCode()
        {
            return m_hash.GetHashCode();
        }

        static public IEnumerable<DigitalFile> GetAllFiles(string path, string searchPattern, bool recursive)
        {
            var di = new DirectoryInfo(path);
            return di.GetFiles(searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.AllDirectories)
                    .Select(fi => new DigitalFile(fi));
        }

        private object m_hashLock = new object();
        private string m_hash = null;
    }
}
