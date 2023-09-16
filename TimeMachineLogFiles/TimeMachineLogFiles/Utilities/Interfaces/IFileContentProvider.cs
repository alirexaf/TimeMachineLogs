using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMachineLogFiles.Utilities.Interfaces
{
    public interface IFileContentProvider
    {
        string GetFileContent(string filePath);
    }
}
