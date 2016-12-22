using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OrangeOxygen.FileMoving;
using OrangeOxygen.FileTypeCompares;

namespace OrangeOxygen
{
    public class ResourceManager : IResourceManager
    {
        private ResourceManager() { }

        public ResourceManager(string digitalFilesBaseDirectory, IEnumerable<IFileMove> fileMoving, IEnumerable<IFileTypeCompare> fileCompares)
        {
            if (digitalFilesBaseDirectory == null)
                throw new NullReferenceException();

            if (!Directory.Exists(digitalFilesBaseDirectory))
                throw new DirectoryNotFoundException($"{digitalFilesBaseDirectory} doesn't exist.");

            if (fileMoving == null)
                throw new NullReferenceException("fileMoving");

            if (fileCompares == null)
                throw new NullReferenceException("fileCompares");

            m_digitalFilesBaseDirectory = digitalFilesBaseDirectory;
            m_fileMoving = fileMoving;
            m_fileCompares = fileCompares;
        }

        public void ManageFiles()
        {
            var files = DigitalFile.GetAllFiles(m_digitalFilesBaseDirectory, "*", true);
            var comparedFiles = GroupSameFiles(files);
            MoveFiles(comparedFiles);
        }

        private IEnumerable<IEnumerable<DigitalFile>> GroupSameFiles(IEnumerable<DigitalFile> digitalFiles)
        {
            return digitalFiles.GroupBy(df => df.FileInformation.Extension.ToLower())
                                            .Where(e => m_fileCompares.Any(fc => fc.CanHandleExention(e.First().FileInformation.Extension)))
                                            .SelectMany(b => b.GroupBy(fi => fi.FileHash, (key, g) => g));
        }

        private void MoveFiles(IEnumerable<IEnumerable<DigitalFile>> digitalFiles)
        {
            foreach (var df in digitalFiles)
            {
                //A collection of the same files.  Just get first for now.
                var file = df.First();
                m_fileMoving.FirstOrDefault(fm => fm.CanMoveFile(file))?.MoveFile(file);
            }
        }

        private string m_digitalFilesBaseDirectory = null;

        private IEnumerable<IFileMove> m_fileMoving = null;
        private IEnumerable<IFileTypeCompare> m_fileCompares = null;
    }
}
