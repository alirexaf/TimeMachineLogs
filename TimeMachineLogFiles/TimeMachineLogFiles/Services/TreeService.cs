using TimeMachineLogFiles.Utilities.Interfaces;

namespace TimeMachineLogFiles.Services
{
    public class TreeService
    {
        public Dictionary<string, object> CreateFileTree(string rootFolder, IFileContentProvider fileContentProvider, SHA1HashService sha1HashService)
        {
            var fileTree = new Dictionary<string, object>();

            foreach (var folderPath in Directory.GetDirectories(rootFolder, "*", SearchOption.AllDirectories))
            {
                var relativePath = folderPath.Substring(rootFolder.Length);
                var currentFolder = fileTree;

                foreach (var subfolder in relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
                {
                    if (!currentFolder.ContainsKey(subfolder))
                    {
                        currentFolder[subfolder] = new Dictionary<string, object>();
                    }
                    currentFolder = (Dictionary<string, object>)currentFolder[subfolder];
                }
            }

            foreach (var filePath in Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories))
            {
                var relativePath = filePath.Substring(rootFolder.Length);
                var fileContent = fileContentProvider.GetFileContent(filePath);
                var fileHash = sha1HashService.CalculateSHA1Hash(fileContent);
                var currentFolder = fileTree;

                var subfolders = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                for (int i = 0; i < subfolders.Length - 1; i++)
                {
                    currentFolder = (Dictionary<string, object>)currentFolder[subfolders[i]];
                }

                currentFolder[subfolders[subfolders.Length - 1]] = fileHash;
            }

            return fileTree;
        }

        public void TraverseFileTreeAndStoreContent(string rootFolder, Dictionary<string, object> fileTree, Dictionary<string, string> sha1ToContent, IFileContentProvider fileContentProvider, CompressionService compressionService, SHA1HashService sha1HashService)
        {
            foreach (var entry in fileTree)
            {
                string entryName = entry.Key;
                var entryValue = entry.Value;

                if (entryValue is Dictionary<string, object> subfolder)
                {
                    // Recurse into subfolders
                    string subfolderPath = Path.Combine(rootFolder, entryName);
                    TraverseFileTreeAndStoreContent(subfolderPath, subfolder, sha1ToContent, fileContentProvider, compressionService, sha1HashService);
                }
                else if (entryValue is string fileHash)
                {
                    // Calculate SHA-1 hash for the content of each file
                    string filePath = Path.Combine(rootFolder, entryName);
                    string content = fileContentProvider.GetFileContent(filePath);
                    string compressedContent = compressionService.CompressContent(content); // Compress the content
                    string sha1Hash = sha1HashService.CalculateSHA1Hash(content);

                    // Store the compressed content in the sha1ToContent mapping
                    sha1ToContent[sha1Hash] = compressedContent;
                }
            }
        }
    }
}
