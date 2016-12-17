
using System.Collections.Generic;
using System.Linq;

namespace OrangeOxygen.FileTypeCompares
{
    public class ImageFileTypeCompare : IFileTypeCompare
    {
        public bool CompareFile(DigitalFile file1, DigitalFile file2)
        {
            return file1.FileHash == file2.FileHash;
        }

        public bool CanHandleExention(string extension)
        {
            return m_extentions.Any(ex => ex == extension.ToLower());
        }

        public IEnumerable<string> m_extentions = new List<string>() { ".jpg", ".png" };
    }
}
