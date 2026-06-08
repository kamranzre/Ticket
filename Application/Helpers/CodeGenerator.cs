using System.Security.Cryptography;

namespace Application.Helpers
{
    public static class CodeGenerator
    {
        private static readonly Random _random = new Random();
        private static readonly HashSet<string> _generatedCodes = new HashSet<string>();

        public static string GenerateTicketCode()
        {
            string code;

            do
            {
                int number = _random.Next(100000, 1000000); // عدد 6 رقمی
                code = $"Tic-{number}";
            }
            while (_generatedCodes.Contains(code));

            _generatedCodes.Add(code);
            return code;
        }

        public static string CodeGenerate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var bytes = RandomNumberGenerator.GetBytes(6);

            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}
