namespace OrangeOxygen.FileMoving
{
    public class BadDateFileMove : FileDateMoveBase
    {
        public BadDateFileMove(string digitalFilesMoveDirectory) : base(digitalFilesMoveDirectory) { }

        override public bool CanMoveFile(DigitalFile digitalFile)
        {
            return !IsValidDateTime(digitalFile);
        }
    }
}
