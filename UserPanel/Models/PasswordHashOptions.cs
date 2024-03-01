using System.Security.Cryptography;
namespace UserPanel.Models
{
    public class PasswordHashOptions
    {
        public HashAlgorithmName passwordHasherAlgorithms {  get; set; }
        public int SaltSize { get; set; }
        public int HashSize { get; set; }
        public int Iterations {  get; set; }
    }
}
