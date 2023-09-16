using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecreateLogStructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecreateLogStructure
{
    public static class Utilities
    {
        public static (Dictionary<string, object> FileTree, Dictionary<string, string> SHA1ToContent) DeserializeFromFile(string jsonFilePath)
        {
            using (var reader = new StreamReader(jsonFilePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                var data = serializer.Deserialize<dynamic>(jsonReader);
                return (
                    FileTree: data["FileTree"].ToObject<Dictionary<string, object>>(),
                    SHA1ToContent: data["SHA1ToContent"].ToObject<Dictionary<string, string>>()
                );
            }
        }
        public static bool IsJsonString(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) { return false; }
            str = str.Trim();
            if ((str.StartsWith("{") && str.EndsWith("}")) ||
                (str.StartsWith("[") && str.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(str);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
