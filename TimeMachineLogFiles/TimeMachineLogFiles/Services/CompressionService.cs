using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMachineLogFiles.Services
{
    public class CompressionService
    {
        public string CompressContent(string content)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(content);
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (DeflateStream compressionStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    compressionStream.Write(inputBytes, 0, inputBytes.Length);
                }
                byte[] compressedBytes = outputStream.ToArray();
                return Convert.ToBase64String(compressedBytes);
            }
        }
    }
}
