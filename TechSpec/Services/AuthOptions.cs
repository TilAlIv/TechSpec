using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TechSpec.Services;

public class AuthOptions
{
    public const string Issuer = "MyAuthServer"; 
    public const string Audience = "MyAuthClient";
    private const string Key = "B28IJ9le1flZ5GsAhrYHOt1x6Mk44jkA"; 
    public const int Lifetime = 5; 
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }

}