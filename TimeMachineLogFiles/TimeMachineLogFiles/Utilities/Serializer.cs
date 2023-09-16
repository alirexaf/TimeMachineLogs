using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMachineLogFiles.Utilities
{
    public static class Serializer
    {
        public static void SerializeToFile(string jsonFilePath, Dictionary<string, object> fileTree, Dictionary<string, string> sha1ToContent)
        {
            using (var writer = new StreamWriter(jsonFilePath))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer();
                var data = new
                {
                    FileTree = fileTree,
                    SHA1ToContent = sha1ToContent
                };
                serializer.Serialize(jsonWriter, data);
            }
        }
    }
}
