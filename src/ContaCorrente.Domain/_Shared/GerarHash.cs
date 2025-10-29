using System.Security.Cryptography;
using System.Text;

namespace ContaCorrente.Domain._Shared
{
    public class GerarHash
    {
        public string Hash { get; }

        public GerarHash(string senha, string salt)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(senha + salt);
            var hash = sha256.ComputeHash(bytes);

            Hash = Convert.ToBase64String(hash);
        }
    }
}
