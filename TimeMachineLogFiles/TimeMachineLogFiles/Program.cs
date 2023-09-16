using Newtonsoft.Json;
using TimeMachineLogFiles.Services;
using TimeMachineLogFiles.Utilities;
using TimeMachineLogFiles.Utilities.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(@"*** Input the path to the root log folder: [DEFAULT: F:\TM\logs]");
        var rootInput = Console.ReadLine();
        string rootFolder =  !String.IsNullOrEmpty(rootInput) ? rootInput : @"F:\TM\logs";
        Console.WriteLine(@"*** Input the name for the log structure json file: [DEFAULT: logs_tree.json]");
        var jsonInput = Console.ReadLine();
        string jsonFilePath = !String.IsNullOrEmpty(jsonInput) ? jsonInput : "logs_tree.json"; // Output JSON file

        var fileContentProvider = new FileContentProvider();
        var compressionService = new CompressionService();
        var sha1HashService = new SHA1HashService();
        var treeService = new TreeService();

        Dictionary<string, object> fileTree = treeService.CreateFileTree(rootFolder,
            fileContentProvider,
            sha1HashService);

        Dictionary<string, string> sha1ToContent = new Dictionary<string, string>();

        // Traverse the file tree to calculate SHA-1 hashes and store content
        treeService.TraverseFileTreeAndStoreContent(rootFolder,
            fileTree,
            sha1ToContent,
            fileContentProvider,
            compressionService,
            sha1HashService);

        // Serialize the file tree and SHA-1 to content mapping to JSON
        Serializer.SerializeToFile(jsonFilePath, fileTree, sha1ToContent);
    }
}
