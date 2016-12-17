using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OrangeOxygen.FileCopying;
using OrangeOxygen.FileTypeCompares;

namespace OrangeOxygen
{
    public class ResourceManager : IResourceManager
    {
        private ResourceManager() { }

        public ResourceManager(string baseDirectory, IEnumerable<IFileCopy> fileCopies, IEnumerable<IFileTypeCompare> fileCompares)
        {
            if (baseDirectory == null)
                throw new NullReferenceException();

            if (!Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException($"{baseDirectory} doesn't exist.");

            if (fileCopies == null)
                throw new NullReferenceException("fileCopies");

            if (fileCompares == null)
                throw new NullReferenceException("fileCompares");

            m_baseDirectory = baseDirectory;
            m_fileCopies = fileCopies;
            m_fileCompares = fileCompares;
        }

        public void ManageFiles()
        {
            var files = DigitalFile.GetAllFiles(m_baseDirectory, "*", true);
            var comparedFiles = GroupSameFiles(files);
        }

        private IEnumerable<IEnumerable<DigitalFile>> GroupSameFiles(IEnumerable<DigitalFile> digitalFiles)
        {
            return digitalFiles.GroupBy(df => df.FileInformation.Extension.ToLower())
                                            .Where(e => m_fileCompares.Any(fc => fc.CanHandleExention(e.First().FileInformation.Extension)))
                                            .SelectMany(b => b.GroupBy(fi => fi.FileHash, (key, g) => g));
        }

        private string m_baseDirectory = null;
        private IEnumerable<IFileCopy> m_fileCopies = null;
        private IEnumerable<IFileTypeCompare> m_fileCompares = null;
    }
}
