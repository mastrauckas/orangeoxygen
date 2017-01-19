using System;
using System.IO;

namespace OrangeOxygen.FileMoving
{
    abstract public class FileDateMoveBase : IFileMove
    {
        public FileDateMoveBase(string digitalFilesMoveDirectory)
        {
            if (string.IsNullOrWhiteSpace(digitalFilesMoveDirectory))
                throw new NullReferenceException("digitalFilesMoveDirectory");

            DigitalFileMoveDirectory = digitalFilesMoveDirectory;
        }
        abstract public bool CanMoveFile(DigitalFile digitalFile);

        virtual public void MoveFile(DigitalFile digitalFile)
        {
            if (!CanMoveFile(digitalFile))
                throw new Exception("Can't move file.");

            if (!Directory.Exists(DigitalFileMoveDirectory))
                Directory.CreateDirectory(DigitalFileMoveDirectory);

            File.Move(digitalFile.FileInformation.FullName, Path.Combine(DigitalFileMoveDirectory, digitalFile.FileInformation.Name));
        }
        virtual protected bool IsValidDateTime(DigitalFile digitalFile)
        {
            return digitalFile.FileDate.HasValue && digitalFile.FileDate != DateTime.MinValue;
        }

        protected string DigitalFileMoveDirectory { get; }

        abstract public string Message { get; }
    }
}
