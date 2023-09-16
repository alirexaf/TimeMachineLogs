using System.Text;
using TimeMachineLogFiles.Utilities.Interfaces;

namespace TimeMachineLogFiles.Utilities
{
    public class FileContentProvider : IFileContentProvider
    {
        public string GetFileContent(string filePath)
        {
            //return File.ReadAllText(filePath;
            var fileBytes = File.ReadAllBytes(filePath);
            return Encoding.UTF8.GetString(fileBytes,0,fileBytes.Length);
        }
    }
}
