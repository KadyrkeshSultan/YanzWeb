using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Yanz.Web
{
    public class AuthOptions
    {
        public const string ISSUER = "YanzServer"; // издатель токена
        public const string AUDIENCE = "http://localhost:5001/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 10080; // время жизни токена в минутах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
