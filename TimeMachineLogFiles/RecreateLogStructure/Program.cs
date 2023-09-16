using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecreateLogStructure;
using RecreateLogStructure.Services;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("*** Input the name and path to the log structure JSON file: [DEFAULT: logs_tree.json]");
        var jsonInput = Console.ReadLine();
        string jsonFilePath = !String.IsNullOrEmpty(jsonInput) ? jsonInput : "logs_tree.json"; // Input JSON file
        Console.WriteLine("*** Input the path to the where the log folder should be created: [DEFAULT: F:/logs]");
        var output = Console.ReadLine();
        string rootOutputFolder = !String.IsNullOrEmpty(output) ? output : @"F:/logs"; // Output folder for recreated structure

        var decompressionService = new DecompressionService();
        var fileService = new FileService();

        var data = Utilities.DeserializeFromFile(jsonFilePath);
        Dictionary<string, object> fileTree = data.FileTree;
        Dictionary<string, string> sha1ToContent = data.SHA1ToContent;

        fileService.RecreateFileStructure(rootOutputFolder, fileTree, sha1ToContent, decompressionService);
    }
}
