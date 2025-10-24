using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SERV
{
    public class EncriptacionServicio
    {
        public static string? EncriptarSHA256(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" convierte a hexadecimal de 2 dígitos
                }

                return builder.ToString();
            }
        }
    }
}
