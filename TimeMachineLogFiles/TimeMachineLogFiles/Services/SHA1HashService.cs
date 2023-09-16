using System.Security.Cryptography;
using System.Text;

namespace TimeMachineLogFiles.Services
{
    public class SHA1HashService
    {
        public string CalculateSHA1Hash(string content)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

}
