using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utility
{
    public class Crypto
    {
        public static string Encrypt(string password)
        {
            //Logic vir encryption           
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
