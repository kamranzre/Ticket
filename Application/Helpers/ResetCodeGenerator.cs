using System.Security.Cryptography;

namespace Application.Helpers
{
    public static class ResetCodeGenerator
    {
        public static string Generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var bytes = RandomNumberGenerator.GetBytes(6);

            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}
