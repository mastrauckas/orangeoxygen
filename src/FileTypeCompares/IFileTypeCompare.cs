
namespace OrangeOxygen.FileTypeCompares
{
    public interface IFileTypeCompare
    {
        bool CompareFile(DigitalFile file1, DigitalFile file2);
        bool CanHandleExention(string extention);
    }
}
