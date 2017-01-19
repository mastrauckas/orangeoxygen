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

            var source = digitalFile.FileInformation.FullName;
            var desination = Path.Combine(DigitalFileMoveDirectory, digitalFile.FileInformation.Name);

            var fileMoveInformation = new FileMoveInformation()
            {
                SourceFileAndDirectory = source,
                DesinationFileAndDirectory = desination
            };

            File.Move(source, desination);
        }
        virtual protected bool IsValidDateTime(DigitalFile digitalFile)
        {
            return digitalFile.FileDate.HasValue && digitalFile.FileDate != DateTime.MinValue;
        }

        protected string DigitalFileMoveDirectory { get; }

        abstract public string Message { get; }
    }
}
