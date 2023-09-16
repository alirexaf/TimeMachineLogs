using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecreateLogStructure.Services
{
    public class FileService
    {
        public void RecreateFileStructure(string outputFolder, Dictionary<string, object> fileTree, Dictionary<string, string> sha1ToContent, DecompressionService decompressionService)
        {
            foreach (var entry in fileTree)
            {
                string entryName = entry.Key;
                var entryValue = entry.Value.ToString();

                var entryDict = Utilities.IsJsonString(entryValue) ? JsonConvert.DeserializeObject<IDictionary<string, object>>(entryValue) : null;

                string entryPath = Path.Combine(outputFolder, entryName);

                if (entryDict is Dictionary<string, object> subfolder)
                {
                    // Recreate subfolder
                    Directory.CreateDirectory(entryPath);
                    RecreateFileStructure(entryPath, subfolder, sha1ToContent, decompressionService);
                }
                else if (entryValue is string sha1Hash)
                {
                    // Recreate file content
                    if (sha1ToContent.TryGetValue(sha1Hash, out string compressedContent))
                    {
                        string content = decompressionService.DecompressContent(compressedContent);
                        var fileBytes = Encoding.UTF8.GetBytes(content);
                        File.WriteAllBytes(entryPath, fileBytes);
                    }
                    else
                    {
                        Console.WriteLine($"Content for '{entryName}' not found in the SHA1 to Content mapping.");
                    }
                }
            }
        }
    }
}
