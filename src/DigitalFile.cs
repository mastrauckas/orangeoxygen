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
                    using (var md5 = MD5.Create())
                        m_hash = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(FileAndPath))).Replace("-", "");

                return m_hash;
            }
        }

        public DateTime? FileDate
        {
            get
            {
                lock (m_fileDateTimeLock)
                {
                    if (!m_hasCheckedFileDateTime)
                    {
                        // Read all metadata from the image
                        var metaData = ImageMetadataReader.ReadMetadata(FileAndPath);

                        // Find the so-called Exif "SubIFD" (which may be null)
                        var subIfdDirectory = metaData.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                        // Read the DateTime tag value
                        var hasTag = subIfdDirectory?.HasTagName(ExifDirectoryBase.TagDateTimeOriginal);
                        if (hasTag.HasValue && hasTag.Value)
                        {
                            var dt = DateTime.MinValue;
                            var hasDateTime = subIfdDirectory?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out dt);
                            if (hasDateTime.HasValue && hasDateTime.Value)
                                m_fileDateTime = dt;
                        }
                        m_hasCheckedFileDateTime = true;
                    }
                }

                return m_fileDateTime;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var digitalFile = obj as DigitalFile;
            return FileHash == digitalFile.FileHash && digitalFile.FileInformation.Length == FileInformation.Length;
        }

        public override int GetHashCode()
        {
            return FileHash.GetHashCode();
        }

        static public IEnumerable<DigitalFile> GetAllFiles(string path, string searchPattern, bool recursive)
        {
            var di = new DirectoryInfo(path);
            return di.GetFiles(searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.AllDirectories)
                    .Select(fi => new DigitalFile(fi));
        }

        private object m_hashLock = new object();
        private string m_hash = null;

        private object m_fileDateTimeLock = new object();

        private bool m_hasCheckedFileDateTime = false;

        private DateTime? m_fileDateTime = null;
    }
}
