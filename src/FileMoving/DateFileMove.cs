using System;
using System.IO;
using System.Linq;


namespace OrangeOxygen.FileMoving
{
    public class FileDateMove : FileDateMoveBase
    {
        public FileDateMove(string digitalFilesMoveDirectory) : base(digitalFilesMoveDirectory) { }

        override public bool CanMoveFile(DigitalFile digitalFile)
        {
            return IsValidDateTime(digitalFile);
        }

        override public void MoveFile(DigitalFile digitalFile)
        {
            if (!CanMoveFile(digitalFile))
                throw new Exception("Can't move file.");

            if (!Directory.Exists(DigitalFileMoveDirectory))
                Directory.CreateDirectory(DigitalFileMoveDirectory);

            var dt = digitalFile.FileDate.Value;
            var year = dt.Year.ToString();

            //Make sure it's a 4 digit year.
            if (year.Count() < 4)
                throw new Exception("year length must be 4 digits.");

            var month = dt.Month.ToString("00");

            var dir = Path.Combine(DigitalFileMoveDirectory, year, month);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.Move(digitalFile.FileInformation.FullName, Path.Combine(dir, digitalFile.FileInformation.Name));
        }

        override public string Message { get { return "Successful File Move(s)"; } }
    }
}
