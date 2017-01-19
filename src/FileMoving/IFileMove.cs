namespace OrangeOxygen.FileMoving
{
    public interface IFileMove
    {
        bool CanMoveFile(DigitalFile digitalFile);
        void MoveFile(DigitalFile digitalFile);

        string Message { get; }
    }
}
